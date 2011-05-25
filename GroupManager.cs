using System;
using System.Collections.Generic;
namespace Artemis
{
	public class GroupManager {
		private World world;
		private Bag<Entity> EMPTY_BAG;
		private Dictionary<String, Bag<Entity>> entitiesByGroup;
		private Bag<String> groupByEntity;
	
		public GroupManager(World world) {
			this.world = world;
			entitiesByGroup = new Dictionary<String, Bag<Entity>>();
			groupByEntity = new Bag<String>();
			EMPTY_BAG = new Bag<Entity>();
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
			
			groupByEntity.Set(e.GetId(), group);
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
		
		/**
		 * Removes the provided entity from the group it is assigned to, if any.
		 * @param e the entity.
		 */
		public void Remove(Entity e) {
			if(e.GetId() < groupByEntity.GetCapacity()) {
				String group = groupByEntity.Get(e.GetId());
				if(group != null) {
					groupByEntity.Set(e.GetId(), null);
					
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
			if(e.GetId() < groupByEntity.GetCapacity()) {
				return groupByEntity.Get(e.GetId());
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

