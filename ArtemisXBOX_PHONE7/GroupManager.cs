using System;
using System.Collections.Generic;
namespace Artemis
{
	public sealed class GroupManager {
		private EntityWorld world;
		private Bag<Entity> EMPTY_BAG = new Bag<Entity>();
		private Dictionary<String, Bag<Entity>> entitiesByGroup = new Dictionary<String, Bag<Entity>>();
		private Bag<String> groupByEntity = new Bag<String>();
	
		public GroupManager(EntityWorld world) {
			this.world = world;
		}
		
		/**
		 * Set the group of the entity.
		 * 
		 * @param group group to set the entity into.
		 * @param e entity to set into the group.
		 */
		public void Set(String group, Entity e) {
			Remove(e); // Entity can only belong to one group.
			
			Bag<Entity> entities;
			if(!entitiesByGroup.TryGetValue(group,out entities)) {
				entities = new Bag<Entity>();
				entitiesByGroup.Add(group, entities);
			}
			entities.Add(e);
			
			groupByEntity.Set(e.Id, group);
		}
		
		/**
		 * Get all entities that belong to the provided group.
		 * @param group name of the group.
		 * @return read-only bag of entities belonging to the group.
		 */
		public Bag<Entity> getEntities(String group) {
			Bag<Entity> bag;
			if(!entitiesByGroup.TryGetValue(group,out bag))
				return EMPTY_BAG;
			return bag;
		}

        /// <summary>
        /// Removes an entity from the group it is assigned to, if any.
        /// </summary>
        /// <param name="e">The entity to be removed</param>
		public void Remove(Entity e) {
			int entityId = e.Id;
			if(entityId < groupByEntity.GetCapacity()) {
				String group = groupByEntity.Get(entityId);
				if(group != null) {
					groupByEntity.Set(entityId, null);
					
					Bag<Entity> entities;
					if(entitiesByGroup.TryGetValue(group,out entities)) {
						entities.Remove(e);
					}
				}
			}
		}
		
		/**
		 * @param e entity
		 * @return the name of the group that this entity belongs to, null if none.
		 */
		public String GetGroupOf(Entity e) {
			int entityId = e.Id;
			if(entityId < groupByEntity.GetCapacity()) {
				return groupByEntity.Get(entityId);
			}
			return null;
		}
		
		/**
		 * Checks if the entity belongs to any group.
		 * @param e the entity to check.
		 * @return true if it is in any group, false if none.
		 */
		public bool IsGrouped(Entity e) {
			return GetGroupOf(e) != null;
		}
	
	}
}

