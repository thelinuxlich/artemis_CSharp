namespace Artemis.System
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;

    #endregion Using statements

    /// <summary>Class IntervalEntityProcessingSystem.</summary>
    public abstract class IntervalEntityProcessingSystem : IntervalEntitySystem
    {
        /// <summary>Initializes a new instance of the <see cref="IntervalEntityProcessingSystem"/> class.</summary>
        /// <param name="interval">The interval.</param>
        /// <param name="requiredType">Type of the required.</param>
        /// <param name="otherTypes">The other types.</param>
        protected IntervalEntityProcessingSystem(int interval, Type requiredType, params Type[] otherTypes)
            : base(interval, EntitySystem.GetMergedTypes(requiredType, otherTypes))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="IntervalEntityProcessingSystem" /> class.</summary>
        /// <param name="interval">The interval.</param>
        /// <param name="aspect">The aspect.</param>
        protected IntervalEntityProcessingSystem(int interval, Aspect aspect)
            : base(interval, aspect)
        {
        }

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public abstract void Process(Entity entity);

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(SortedDictionary<int, Entity> entities)
        {
            for (int index = 0, s = entities.Count; s > index; ++index)
            {
                this.Process(entities[index]);
            }
        }
    }
}