namespace Artemis.System
{
    #region Using statements

    using global::System.Collections.Generic;
    using global::System.Diagnostics;

    #endregion Using statements

    /// <summary>Tag System does not fire ANY Events of the EntitySystem</summary>
    public abstract class TagSystem : EntitySystem
    {
        /// <summary>The tag.</summary>
        protected string Tag;

        /// <summary>Initializes a new instance of the <see cref="TagSystem"/> class.</summary>
        /// <param name="tag">The tag.</param>
        protected TagSystem(string tag)
        {
            Debug.Assert(!string.IsNullOrEmpty(tag), "tag must not be null.");

            this.Tag = tag;
        }

        /// <summary>Called when [change].</summary>
        /// <param name="entity">The entity.</param>
        public override void OnChange(Entity entity)
        {
        }

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public abstract void Process(Entity entity);

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(SortedDictionary<int, Entity> entities)
        {
            Entity entity = this.EntityWorld.TagManager.GetEntity(this.Tag);
            if (entity != null)
            {
                this.Process(entity);
            }
        }
    }
}