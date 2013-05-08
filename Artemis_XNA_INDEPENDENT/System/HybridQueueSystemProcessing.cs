#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HybridQueueSystemProcessing.cs" company="GAMADU.COM">
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
//   Class HybridQueueSystemProcessing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.System
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;

    using Artemis.Manager;

    #endregion Using statements

    /// <summary>Class HybridQueueSystemProcessing.</summary>
    public abstract class HybridQueueSystemProcessing : EntityProcessingSystem
    {
        /// <summary>The entities to process each frame.</summary>
        public const int EntitiesToProcessEachFrame = 50;

        /// <summary>The comp types.</summary>
        private readonly List<ComponentType> compTypes;

        /// <summary>The queue.</summary>
        private readonly Queue<Entity> queue;

        /// <summary>Initializes a new instance of the <see cref="HybridQueueSystemProcessing" /> class.</summary>
        /// <param name="requiredType">Type of the required.</param>
        /// <param name="otherTypes">The other types.</param>
        protected HybridQueueSystemProcessing(Type requiredType, params Type[] otherTypes)
            : base(requiredType, otherTypes)
        {
            this.queue = new Queue<Entity>();
            this.compTypes = new List<ComponentType>();
            foreach (Type type in this.Types)
            {
                this.compTypes.Add(ComponentTypeManager.GetTypeFor(type));
            }
        }

        /// <summary>Gets the queue count.</summary>
        /// <value>The queue count.</value>
        public int QueueCount
        {
            get
            {
                return this.queue.Count;
            }
        }

        /// <summary>Adds to queue.</summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="Exception">This EntitySystem does not process this kind of entity</exception>
        public void AddToQueue(Entity entity)
        {
            if (!this.Interests(entity))
            {
                throw new Exception("This EntitySystem does not process this kind of entity.");
            }

            this.queue.Enqueue(entity);
        }

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            if (!this.IsEnabled)
            {
                return;
            }

            int size = this.queue.Count > EntitiesToProcessEachFrame ? EntitiesToProcessEachFrame : this.queue.Count;
            for (int index = 0; index < size; ++index)
            {
                this.Process(this.queue.Dequeue());
            }

            base.ProcessEntities(entities);
        }
    }
}