#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParallelEntityProcessingSystem.cs" company="GAMADU.COM">
//     Copyright © 2013 GAMADU.COM. All rights reserved.
//
//     Redistribution and use in source and binary forms, with or without modification, are
//     permitted provided that the following conditions are met:
//
//        1. Redistributions of source code must retain the above copyright notice, this list of
//           conditions and the following disclaimer.
//
//        2. Redistributions in binary form must reproduce the above copyright notice, this list
//           of conditions and the following disclaimer in the documentation and/or other materials
//           provided with the distribution.
//
//     THIS SOFTWARE IS PROVIDED BY GAMADU.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
//     WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//     FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
//     CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//     CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//     SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//     ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//     ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//     The views and conclusions contained in the software and documentation are those of the
//     authors and should not be interpreted as representing official policies, either expressed
//     or implied, of GAMADU.COM.
// </copyright>
// <summary>
//   Class ParallelEntityProcessingSystem.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

#if !PORTABLE
namespace Artemis.System
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;
#if FULLDOTNET || METRO || CLIENTPROFILE || UNITY5
    using global::System.Threading.Tasks;
#else
    using ParallelTasks;
#endif

    #endregion Using statements

    /// <summary>Class ParallelEntityProcessingSystem.</summary>
    public abstract class ParallelEntityProcessingSystem : EntitySystem
    {
#if FULLDOTNET || CLIENTPROFILE
        /// <summary>The factory.</summary>
        private readonly TaskFactory factory;
#endif

        /// <summary>Initializes a new instance of the <see cref="ParallelEntityProcessingSystem" /> class.</summary>
        /// <param name="aspect">The aspect.</param>
        protected ParallelEntityProcessingSystem(Aspect aspect)
            : base(aspect)
        {
#if FULLDOTNET || CLIENTPROFILE
            this.factory = new TaskFactory(TaskScheduler.Default);
#endif
        }

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public abstract void Process(Entity entity);

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
#if !PORTABLE
            float simultaneous = Environment.ProcessorCount * 2;
#else
            float simultaneous = 2;
#endif
            int perThread = (int)Math.Ceiling(entities.Values.Count / simultaneous);
            Entity[] threadEntities = new Entity[entities.Values.Count];
            entities.Values.CopyTo(threadEntities, 0);
            int numberOfEntities = entities.Values.Count - 1;
            List<Task> tasks = new List<Task>();

            for (int processorIndex = 0; processorIndex < simultaneous; ++processorIndex)
            {
                int initial = numberOfEntities;
#if FULLDOTNET || CLIENTPROFILE
                tasks.Add(
                    this.factory.StartNew(
#elif METRO || UNITY5
                tasks.Add(Task.Factory.StartNew(
#else
                tasks.Add(Parallel.Start(
#endif
                        () =>
                            {
                                for (int spartialIndex = initial; spartialIndex > initial - perThread && spartialIndex >= 0; --spartialIndex)
                                {
                                    this.Process(threadEntities[spartialIndex]);
                                }
                            }));
                numberOfEntities -= perThread;
            }
#if FULLDOTNET
            Task.WaitAll(tasks.ToArray());
#else
            foreach (Task task in tasks)
            {
                task.Wait();
            }
#endif
        }
    }
}
#endif