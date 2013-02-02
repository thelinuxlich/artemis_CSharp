namespace Artemis.Manager
{
    #region Using statements

    using Artemis.Utils;

    using global::System.Collections.Generic;
    using global::System.Diagnostics;

    #endregion Using statements

    /// <summary>Class GroupManager.</summary>
    public sealed class GroupManager
    {
        /// <summary>The empty bag.</summary>
        private readonly Bag<Entity> emptyBag;

        /// <summary>The entities by group.</summary>
        private readonly Dictionary<string, Bag<Entity>> entitiesByGroup;

        /// <summary>The group by entity.</summary>
        private readonly Bag<string> groupByEntity;

        internal GroupManager()
        {
            this.groupByEntity = new Bag<string>();
            this.entitiesByGroup = new Dictionary<string, Bag<Entity>>();
            this.emptyBag = new Bag<Entity>();
        }

        /// <summary>Gets the entities.</summary>
        /// <param name="group">The group.</param>
        /// <returns>Bag{Entity}.</returns>
        public Bag<Entity> GetEntities(string group)
        {
            Debug.Assert(!string.IsNullOrEmpty(group), "group must not be null or empty.");

            Bag<Entity> bag;
            if (!this.entitiesByGroup.TryGetValue(group, out bag))
            {
                return this.emptyBag;
            }

            return bag;
        }

        /// <summary>Gets the group of.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>System.String.</returns>
        public string GetGroupOf(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            int entityId = entity.Id;
            if (entityId < this.groupByEntity.Capacity)
            {
                return this.groupByEntity.Get(entityId);
            }

            return null;
        }

        /// <summary>Determines whether the specified entity is grouped.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns><see langword="true" /> if the specified entity is grouped; otherwise, <see langword="false" />.</returns>
        public bool IsGrouped(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            return this.GetGroupOf(entity) != null;
        }

        /// <summary>Removes an entity from the group it is assigned to, if any.</summary>
        /// <param name="entity">The entity to be removed</param>
        internal void Remove(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            int entityId = entity.Id;
            if (entityId < this.groupByEntity.Capacity)
            {
                string group = this.groupByEntity.Get(entityId);
                if (group != null)
                {
                    this.groupByEntity.Set(entityId, null);

                    Bag<Entity> entities;
                    if (this.entitiesByGroup.TryGetValue(group, out entities))
                    {
                        entities.Remove(entity);
                    }
                }
            }
        }

        /// <summary>Sets the specified group.</summary>
        /// <param name="group">The group.</param>
        /// <param name="entity">The entity.</param>
        internal void Set(string group, Entity entity)
        {
            Debug.Assert(!string.IsNullOrEmpty(group), "group must not be null or empty.");
            Debug.Assert(entity != null, "Entity must not be null.");

            // Entity can only belong to one group.
            this.Remove(entity);

            Bag<Entity> entities;
            if (!this.entitiesByGroup.TryGetValue(group, out entities))
            {
                entities = new Bag<Entity>();
                this.entitiesByGroup.Add(group, entities);
            }
            entities.Add(entity);

            this.groupByEntity.Set(entity.Id, group);
        }
    }
}