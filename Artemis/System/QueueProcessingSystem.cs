#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueueProcessingSystem.cs" company="GAMADU.COM">
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
//   System Not based On Components. It Process ONCE everything you explicitly add to it using the method AddToQueue.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.System
{
    #region Using statements

    using global::System.Collections.Generic;

    #endregion Using statements

    /// <summary>
    /// <para>System not based on Components.</para>
    /// <para>It processes ONCE everything you explicitly add to it</para>
    /// <para>using the method AddToQueue.</para>
    /// <para>Use <see cref="EntitiesToProcessEachFrame" /> property to set processing batch size.</para>
    /// </summary>
    public abstract class QueueProcessingSystem : ProcessingSystem
    {
        /// <summary>The queue.</summary>
        private readonly Queue<Entity> queue;

        /// <summary>Initializes a new instance of the <see cref="QueueProcessingSystem"/> class.</summary>
        public QueueProcessingSystem()
        {
            this.EntitiesToProcessEachFrame = 50;
            this.queue = new Queue<Entity>();
        }

        /// <summary>Gets or sets the entities to process each frame.</summary>
        /// <value>The entities to process each frame.</value>
        public int EntitiesToProcessEachFrame { get; set; }

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
        public void AddToQueue(Entity entity)
        {
            this.queue.Enqueue(entity);
        }

        public override void ProcessSystem()
        {
            int size = this.queue.Count > this.EntitiesToProcessEachFrame ? this.EntitiesToProcessEachFrame : this.queue.Count;
            for (int index = 0; index < size; ++index)
            {
                this.Process(this.queue.Dequeue());
            }
        }

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public abstract void Process(Entity entity);
    }
}