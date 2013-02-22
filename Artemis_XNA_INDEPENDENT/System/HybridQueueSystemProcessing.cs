namespace Artemis.System
{
    #region Using statements

    using Artemis.Manager;

    using global::System;
    using global::System.Collections.Generic;

    #endregion Using statements

    /// <summary>Class HybridQueueSystemProcessing.</summary>
    public abstract class HybridQueueSystemProcessing : EntityProcessingSystem
    {
        /// <summary>The entities to process each frame.</summary>
        public static int EntitiesToProcessEachFrame = 50;

        /// <summary>The comp types.</summary>
        private readonly List<ComponentType> compTypes;

        /// <summary>The queue.</summary>
        private readonly Queue<Entity> queue;

        /// <summary>Initializes a new instance of the <see cref="EntityProcessingSystem" /> class.</summary>
        /// <param name="requiredType">Type of the required.</param>
        /// <param name="otherTypes">The other types.</param>
        protected HybridQueueSystemProcessing(Type requiredType, params Type[] otherTypes)
            : base(requiredType, otherTypes)
        {
            this.queue = new Queue<Entity>();
            this.compTypes = new List<ComponentType>();
            foreach (Type item in GetMergedTypes(requiredType, otherTypes))
            {
                this.compTypes.Add(ComponentTypeManager.GetTypeFor(item));
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

        protected override void ProcessEntities(SortedDictionary<int, Entity> entities)
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