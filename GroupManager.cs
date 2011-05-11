using System;
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
		public void set(String group, Entity e) {
			remove(e); // Entity can only belong to one group.
			
			Bag<Entity> entities = entitiesByGroup[group];
			if(entities == null) {
				entities = new Bag<Entity>();
				entitiesByGroup.Add(group, entities);
			}
			entities.add(e);
			
			groupByEntity.set(e.getId(), group);
		}
		
		/**
		 * Get all entities that belong to the provided group.
		 * @param group name of the group.
		 * @return read-only bag of entities belonging to the group.
		 */
		public Bag<Entity> getEntities(String group) {
			Bag<Entity> bag = entitiesByGroup[group];
			if(bag == null)
				return EMPTY_BAG;
			return bag;
		}
		
		/**
		 * Removes the provided entity from the group it is assigned to, if any.
		 * @param e the entity.
		 */
		public void remove(Entity e) {
			if(e.getId() < groupByEntity.getCapacity()) {
				String group = groupByEntity.get(e.getId());
				if(group != null) {
					groupByEntity.set(e.getId(), null);
					
					Bag<Entity> entities = entitiesByGroup[group];
					if(entities != null) {
						entities.remove(e);
					}
				}
			}
		}
		
		/**
		 * @param e entity
		 * @return the name of the group that this entity belongs to, null if none.
		 */
		public String getGroupOf(Entity e) {
			if(e.getId() < groupByEntity.getCapacity()) {
				return groupByEntity.get(e.getId());
			}
			return null;
		}
		
		/**
		 * Checks if the entity belongs to any group.
		 * @param e the entity to check.
		 * @return true if it is in any group, false if none.
		 */
		public boolean isGrouped(Entity e) {
			return getGroupOf(e) != null;
		}
	
	}
}

