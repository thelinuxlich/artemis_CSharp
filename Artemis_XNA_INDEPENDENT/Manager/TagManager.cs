namespace Artemis.Manager
{
    #region Using statements

    using global::System.Collections.Generic;
    using global::System.Diagnostics;
    using global::System.Linq;

    #endregion Using statements

    /// <summary>Class TagManager.</summary>
    public sealed class TagManager
    {
        /// <summary>The entity by tag.</summary>
        private readonly Dictionary<string, Entity> entityByTag;

        /// <summary>Initializes a new instance of the <see cref="TagManager" /> class.</summary>
        internal TagManager()
        {
            this.entityByTag = new Dictionary<string, Entity>();
        }

        /// <summary>Gets the entity.</summary>
        /// <param name="tag">The tag.</param>
        /// <returns>The specified entity.</returns>
        public Entity GetEntity(string tag)
        {
            Debug.Assert(!string.IsNullOrEmpty(tag), "Tag must not be null or empty.");

            Entity entity;
            this.entityByTag.TryGetValue(tag, out entity);
            if (entity != null && entity.IsActive)
            {
                return entity;
            }

            this.Unregister(tag);
            return null;
        }

        /// <summary>Gets the tag of entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The tag of the specified entity. Is empty if not set.</returns>
        public string GetTagOfEntity(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            string tag = string.Empty;
            foreach (KeyValuePair<string, Entity> pair in this.entityByTag.Where(pair => pair.Value.Equals(entity)))
            {
                tag = pair.Key;
                break;
            }

            return tag;
        }

        /// <summary>Determines whether the specified tag is registered.</summary>
        /// <param name="tag">The tag.</param>
        /// <returns><see langword="true" /> if the specified tag is registered; otherwise, <see langword="false" />.</returns>
        public bool IsRegistered(string tag)
        {
            Debug.Assert(!string.IsNullOrEmpty(tag), "Tag must not be null or empty.");

            return this.entityByTag.ContainsKey(tag);
        }

        /// <summary>Registers the specified tag.</summary>
        /// <param name="tag">The tag.</param>
        /// <param name="entity">The entity.</param>
        internal void Register(string tag, Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            Debug.Assert(!string.IsNullOrEmpty(tag), "Tag must not be null or empty.");

            this.entityByTag.Add(tag, entity);
        }

        /// <summary>Unregisters the specified tag.</summary>
        /// <param name="tag">The tag.</param>
        internal void Unregister(string tag)
        {
            Debug.Assert(!string.IsNullOrEmpty(tag), "Tag must not be null or empty.");

            this.entityByTag.Remove(tag);
        }
    }
}