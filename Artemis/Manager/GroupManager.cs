#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupManager.cs" company="GAMADU.COM">
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
//   Class GroupManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Manager
{
    #region Using statements

    using global::System.Collections.Generic;
    using global::System.Diagnostics;

    using Artemis.Utils;

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

        /// <summary>Initializes a new instance of the <see cref="GroupManager"/> class.</summary>
        internal GroupManager()
        {
            this.groupByEntity = new Bag<string>();
            this.entitiesByGroup = new Dictionary<string, Bag<Entity>>();
            this.emptyBag = new Bag<Entity>();
        }

        /// <summary>Gets the entities.</summary>
        /// <param name="group">The group.</param>
        /// <returns>All entities related to the specified group in a Bag{Entity}.</returns>
        public Bag<Entity> GetEntities(string group)
        {
            Debug.Assert(!string.IsNullOrEmpty(group), "Group must not be null or empty.");

            Bag<Entity> bag;
            if (!this.entitiesByGroup.TryGetValue(group, out bag))
            {
                return this.emptyBag;
            }

            return bag;
        }

        /// <summary>Gets the group of.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The group name.</returns>
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
            Debug.Assert(!string.IsNullOrEmpty(group), "Group must not be null or empty.");
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