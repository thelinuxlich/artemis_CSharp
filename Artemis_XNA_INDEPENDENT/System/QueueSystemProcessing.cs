#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueueSystemProcessing.cs" company="GAMADU.COM">
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
    /// <para>System Not based On Components.</para>
    /// <para>It Process ONCE everything you explicitly add to it</para>
    /// <para>using the method AddToQueue.</para>
    /// </summary>
    public class QueueSystemProcessing : EntitySystem
    {
        /// <summary>The queue.</summary>
        private readonly Queue<Entity> queue;

        /// <summary>Initializes a new instance of the <see cref="QueueSystemProcessing"/> class.</summary>
        public QueueSystemProcessing()
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

        /// <summary>Override to implement code that gets executed when systems are initialized.</summary>
        public override void LoadContent()
        {
        }

        /// <summary>Override to implement code that gets executed when systems are terminated.</summary>
        public override void UnloadContent()
        {
        }

        /// <summary>Called when the system has received a entity it is interested in, e.g. created or a component was added to it.</summary>
        /// <param name="entity">The entity that was added to this system.</param>
        public override void OnAdded(Entity entity)
        {
        }

        /// <summary>Called when an entity was removed from this system, e.g. deleted or had one of it's components removed.</summary>
        /// <param name="entity">The entity that was removed from this system.</param>
        public override void OnChange(Entity entity)
        {
        }

        /// <summary>Called when [removed].</summary>
        /// <param name="entity">The entity.</param>
        public override void OnRemoved(Entity entity)
        {
        }

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public virtual void Process(Entity entity)
        {
        }

        /// <summary>Processes this instance.</summary>
        public override void Process()
        {
            if (!this.IsEnabled)
            {
                return;
            }

            int size = this.queue.Count > this.EntitiesToProcessEachFrame ? this.EntitiesToProcessEachFrame : this.queue.Count;
            for (int index = 0; index < size; ++index)
            {
                this.Process(this.queue.Dequeue());
            }
        }

        /*
        /// <summary>Des the queue.</summary>
        /// <returns>Entity.</returns>
        private Entity DeQueue()
        {
            if (this.queue.Count > 0)
            {
                return this.queue.Dequeue();
            }

            return null;
        }
        */
    }
}