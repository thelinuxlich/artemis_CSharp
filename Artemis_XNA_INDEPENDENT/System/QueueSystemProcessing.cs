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
        /// <summary>The entities to process each frame.</summary>
        public const int EntitiesToProcessEachFrame = 50;

        /// <summary>The queue.</summary>
        private readonly Queue<Entity> queue;

        /// <summary>Initializes a new instance of the <see cref="QueueSystemProcessing"/> class.</summary>
        public QueueSystemProcessing()
        {
            this.queue = new Queue<Entity>();
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
        public void AddToQueue(Entity entity)
        {
            this.queue.Enqueue(entity);
        }

        /// <summary>Initializes this instance.</summary>
        /// Override to implement code that gets executed when systems are initialized.
        public override void Initialize()
        {
        }

        /// <summary>Called when [added].</summary>
        /// <param name="entity">The entity.</param>
        /// Called if the system has received a entity it is interested in, e.g. created or a component was added to it.
        /// @param entity the entity that was added to this system.
        public override void OnAdded(Entity entity)
        {
        }

        /// <summary>Called when [change].</summary>
        /// <param name="entity">The entity.</param>
        /// Called if a entity was removed from this system, e.g. deleted or had one of it's components removed.
        /// @param entity the entity that was removed from this system.
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

            int size = this.queue.Count > EntitiesToProcessEachFrame ? EntitiesToProcessEachFrame : this.queue.Count;
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