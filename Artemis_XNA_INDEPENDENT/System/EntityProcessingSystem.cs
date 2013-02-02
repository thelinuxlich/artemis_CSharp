namespace Artemis.System
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;

    #endregion Using statements

    /// <summary>Class EntityProcessingSystem.</summary>
    public abstract class EntityProcessingSystem : EntitySystem
    {
        /// <summary>Initializes a new instance of the <see cref="EntityProcessingSystem"/> class.</summary>
        /// <param name="requiredType">Type of the required.</param>
        /// <param name="otherTypes">The other types.</param>
        protected EntityProcessingSystem(Type requiredType, params Type[] otherTypes)
            : base(EntitySystem.GetMergedTypes(requiredType, otherTypes))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EntityProcessingSystem" /> class.</summary>
        /// <param name="aspect">The aspect.</param>
        protected EntityProcessingSystem(Aspect aspect)
            : base(aspect)
        {
        }

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public abstract void Process(Entity entity);

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(SortedDictionary<int, Entity> entities)
        {
            foreach (Entity item in entities.Values)
            {
                this.Process(item);
            }
        }
    }
}