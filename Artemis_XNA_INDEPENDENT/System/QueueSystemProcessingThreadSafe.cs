namespace Artemis.System
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;

    using Artemis.Manager;

    #endregion Using statements

    /// <summary>
    /// <para>System Not based On Components.</para>
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
        /// <returns>Number of queue processing limit.</returns>
        public static int GetQueueProcessingLimit(Type entitySystemType)
        {
            QueueManager queueManager = QueuesManager[entitySystemType];
            queueManager.AcquireLock();
            int result = QueueManager.EntitiesToProcessEachFrame;
            queueManager.ReleaseLock();
            return result;
        }

        /// <summary>Queues the count.</summary>
        /// <param name="entitySystemType">Type of the entity system.</param>
        /// <returns>Number of queues.</returns>
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
            QueueManager.EntitiesToProcessEachFrame = limit;
            queueManager.ReleaseLock();
        }

        /// <summary>Initializes this instance.</summary>
        /// Override to implement code that gets executed when systems are initialized.
        public override void Initialize()
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
                if (count > QueueManager.EntitiesToProcessEachFrame)
                {
                    entities = new Entity[QueueManager.EntitiesToProcessEachFrame];
                    for (int index = 0; index < QueueManager.EntitiesToProcessEachFrame; ++index)
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

        /*
        /// <summary>Des the queue.</summary>
        /// <param name="entitySystemType">Type of the entity system.</param>
        /// <returns>Entity.</returns>
        private static Entity DeQueue(Type entitySystemType)
        {
            QueueManager queueManager = QueuesManager[entitySystemType];
            queueManager.AcquireLock();
            Entity entity = queueManager.Queue.Dequeue();
            queueManager.ReleaseLock();
            return entity;
        }
        */
    }
}