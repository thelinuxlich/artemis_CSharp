#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueueSystemProcessingThreadSafe.cs" company="GAMADU.COM">
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

    using global::System;
    using global::System.Collections.Generic;

    using Artemis.Manager;

    #endregion Using statements

    /// <summary>
    /// <para>Queue system not based on components.</para>
    /// <para>It Process ONCE everything you explicitly add to it</para>
    /// <para>using the method AddToQueue.</para>
    /// </summary>
    public abstract class QueueSystemProcessingThreadSafe : EntitySystem
    {
        /// <summary>The id.</summary>
        public readonly Type Id;

        /// <summary>The queues manager.</summary>
        private static readonly Dictionary<Type, QueueManager> QueuesManager = new Dictionary<Type, QueueManager>();

        /// <summary>Initializes a new instance of the <see cref="QueueSystemProcessingThreadSafe"/> class.</summary>
        protected QueueSystemProcessingThreadSafe()
        {
            this.Id = this.GetType();
            if (!QueuesManager.ContainsKey(this.Id))
            {
                QueuesManager[this.Id] = new QueueManager();
            }
            else
            {
                QueuesManager[this.Id].AcquireLock();
                ++QueuesManager[this.Id].RefCount;
                QueuesManager[this.Id].ReleaseLock();
            }
        }

        /// <summary>Finalizes an instance of the <see cref="QueueSystemProcessingThreadSafe"/> class.</summary>
        ~QueueSystemProcessingThreadSafe()
        {
            QueueManager queueManager = QueuesManager[this.Id];
            queueManager.AcquireLock();
            --queueManager.RefCount;
            if (queueManager.RefCount == 0)
            {
                QueuesManager.Remove(this.Id);
            }

            queueManager.ReleaseLock();
        }

        /// <summary>Adds to queue.</summary>
        /// <param name="ent">The entity.</param>
        /// <param name="entitySystemType">Type of the entity system.</param>
        public static void AddToQueue(Entity ent, Type entitySystemType)
        {
            QueueManager queueManager = QueuesManager[entitySystemType];
            queueManager.AcquireLock();
            queueManager.Queue.Enqueue(ent);
            queueManager.ReleaseLock();
        }

        /// <summary>Gets the queue processing limit.</summary>
        /// <param name="entitySystemType">Type of the entity system.</param>
        /// <returns>The limit.</returns>
        public static int GetQueueProcessingLimit(Type entitySystemType)
        {
            QueueManager queueManager = QueuesManager[entitySystemType];
            queueManager.AcquireLock();
            int result = queueManager.EntitiesToProcessEachFrame;
            queueManager.ReleaseLock();
            return result;
        }

        /// <summary>Queues the count.</summary>
        /// <param name="entitySystemType">Type of the entity system.</param>
        /// <returns>The number of queues.</returns>
        public static int QueueCount(Type entitySystemType)
        {
            QueueManager queueManager = QueuesManager[entitySystemType];
            queueManager.AcquireLock();
            int result = queueManager.Queue.Count;
            queueManager.ReleaseLock();
            return result;
        }

        /// <summary>Sets the queue processing limit.</summary>
        /// <param name="limit">The limit.</param>
        /// <param name="entitySystemType">Type of the entity system.</param>
        public static void SetQueueProcessingLimit(int limit, Type entitySystemType)
        {
            QueueManager queueManager = QueuesManager[entitySystemType];
            queueManager.AcquireLock();
            queueManager.EntitiesToProcessEachFrame = limit;
            queueManager.ReleaseLock();
        }

        /// <summary>Override to implement code that gets executed when systems are initialized.</summary>
        public override void LoadContent()
        {
        }

        /// <summary>Override to implement code that gets executed when systems are terminated.</summary>
        public override void UnloadContent()
        {
        }

        /// <summary>Called when [added].</summary>
        /// <param name="entity">The entity.</param>
        public override void OnAdded(Entity entity)
        {
        }

        /// <summary>Called when [change].</summary>
        /// <param name="entity">The entity.</param>
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
            Entity[] entities;
            QueueManager queueManager = QueuesManager[this.Id];
            queueManager.AcquireLock();
            {
                int count = queueManager.Queue.Count;
                if (count > queueManager.EntitiesToProcessEachFrame)
                {
                    entities = new Entity[queueManager.EntitiesToProcessEachFrame];
                    for (int index = 0; index < queueManager.EntitiesToProcessEachFrame; ++index)
                    {
                        entities[index] = queueManager.Queue.Dequeue();
                    }
                }
                else
                {
                    entities = queueManager.Queue.ToArray();
                    queueManager.Queue.Clear();
                }
            }

            queueManager.ReleaseLock();

            foreach (Entity item in entities)
            {
                this.Process(item);
            }
        }
    }

    // NOTE: Please follow only one concept to overload entity system parts.

    /// <summary><para>Queue system not based on entities and components.</para>
    ///   <para>It Process ONCE everything you explicitly add to it.</para>
    ///   <para>Use the static method AddToQueue (second parameter is the type of your specialization of this class).</para>
    /// </summary>
    /// <typeparam name="T">The Type T.</typeparam>
    public abstract class QueueSystemProcessingThreadSafe<T> : EntitySystem
    {
        /// <summary>The id.</summary>
        public readonly Type Id;

        /// <summary>The queues manager.</summary>
        private static readonly Dictionary<Type, QueueManager<T>> QueuesManager = new Dictionary<Type, QueueManager<T>>();

        /// <summary>Initializes a new instance of the <see cref="QueueSystemProcessingThreadSafe{T}"/> class.</summary>
        protected QueueSystemProcessingThreadSafe()
        {
            this.Id = this.GetType();
            if (!QueuesManager.ContainsKey(this.Id))
            {
                QueuesManager[this.Id] = new QueueManager<T>();
            }
            else
            {
                QueuesManager[this.Id].AcquireLock();
                ++QueuesManager[this.Id].RefCount;
                QueuesManager[this.Id].ReleaseLock();
            }
        }

        /// <summary>Finalizes an instance of the <see cref="QueueSystemProcessingThreadSafe{T}"/> class.</summary>
        ~QueueSystemProcessingThreadSafe()
        {
            QueueManager<T> queueManager = QueuesManager[this.Id];
            queueManager.AcquireLock();
            --queueManager.RefCount;
            if (queueManager.RefCount == 0)
            {
                QueuesManager.Remove(this.Id);
            }

            queueManager.ReleaseLock();
        }

        /// <summary>Adds to queue.</summary>
        /// <param name="ent">The entity.</param>
        /// <param name="entitySystemType">Type of the entity system.</param>
        public static void AddToQueue(T ent, Type entitySystemType)
        {
            QueueManager<T> queueManager = QueuesManager[entitySystemType];
            queueManager.AcquireLock();
            queueManager.Queue.Enqueue(ent);
            queueManager.ReleaseLock();
        }

        /// <summary>Gets the queue processing limit.</summary>
        /// <param name="entitySystemType">Type of the entity system.</param>
        /// <returns>The limit.</returns>
        public static int GetQueueProcessingLimit(Type entitySystemType)
        {
            QueueManager<T> queueManager = QueuesManager[entitySystemType];
            queueManager.AcquireLock();
            int result = queueManager.EntitiesToProcessEachFrame;
            queueManager.ReleaseLock();
            return result;
        }

        /// <summary>Queues the count.</summary>
        /// <param name="entitySystemType">Type of the entity system.</param>
        /// <returns>The number of queues.</returns>
        public static int QueueCount(Type entitySystemType)
        {
            QueueManager<T> queueManager = QueuesManager[entitySystemType];
            queueManager.AcquireLock();
            int result = queueManager.Queue.Count;
            queueManager.ReleaseLock();
            return result;
        }

        /// <summary>Sets the queue processing limit.</summary>
        /// <param name="limit">The limit.</param>
        /// <param name="entitySystemType">Type of the entity system.</param>
        public static void SetQueueProcessingLimit(int limit, Type entitySystemType)
        {
            QueueManager<T> queueManager = QueuesManager[entitySystemType];
            queueManager.AcquireLock();
            queueManager.EntitiesToProcessEachFrame = limit;
            queueManager.ReleaseLock();
        }

        /// <summary>Override to implement code that gets executed when systems are initialized.</summary>
        public override void LoadContent()
        {
        }

        /// <summary>Override to implement code that gets executed when systems are terminated.</summary>
        public override void UnloadContent()
        {
        }

        /// <summary>Called when [added].</summary>
        /// <param name="entity">The entity.</param>
        public override void OnAdded(Entity entity)
        {
        }

        /// <summary>Called when [change].</summary>
        /// <param name="entity">The entity.</param>
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
        public void Process(Entity entity)
        {
        }

        /// <summary>Processes this instance.</summary>
        public override void Process()
        {
            T[] entities;
            QueueManager<T> queueManager = QueuesManager[this.Id];
            queueManager.AcquireLock();
            {
                int count = queueManager.Queue.Count;
                if (count > queueManager.EntitiesToProcessEachFrame)
                {
                    entities = new T[queueManager.EntitiesToProcessEachFrame];
                    for (int index = 0; index < queueManager.EntitiesToProcessEachFrame; ++index)
                    {
                        entities[index] = queueManager.Queue.Dequeue();
                    }
                }
                else
                {
                    entities = queueManager.Queue.ToArray();
                    queueManager.Queue.Clear();
                }
            }

            queueManager.ReleaseLock();

            foreach (T item in entities)
            {
                this.Process(item);
            }
        }

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public virtual void Process(T entity)
        {
        }
    }
}