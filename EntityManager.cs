using System;
namespace Artemis
{
	public class EntityManager {
		private World world;
		private Bag<Entity> activeEntities;
		private Bag<Entity> removedAndAvailable;
		private int nextAvailableId;
		private int count;
		private long uniqueEntityId;
		private long totalCreated;
		private long totalRemoved;
		
		private Bag<Bag<Component>> componentsByType;
		
		private Bag<Component> entityComponents; // Added for debug support.
	
		public EntityManager(World world) {
			this.world = world;
			
			activeEntities = new Bag<Entity>();
			removedAndAvailable = new Bag<Entity>();
			
			componentsByType = new Bag<Bag<Component>>();
			
			entityComponents = new Bag<Component>();
		}
	
		public Entity Create() {
			Entity e = removedAndAvailable.RemoveLast();
			if (e == null) {
				e = new Entity(world, nextAvailableId++);
			} else {
				e.Reset();
			}
			e.SetUniqueId(uniqueEntityId++);
			activeEntities.Set(e.GetId(),e);
			count++;
			totalCreated++;
			return e;
		}
	
		public void Remove(Entity e) {
			activeEntities.Set(e.GetId(), null);
			
			e.SetTypeBits(0);
			
			Refresh(e);
			
			RemoveComponentsOfEntity(e);
			
			count--;
			totalRemoved++;
	
			removedAndAvailable.Add(e);
		}
	
		private void RemoveComponentsOfEntity(Entity e) {
			for(int a = 0; componentsByType.Size() > a; a++) {
				Bag<Component> components = componentsByType.Get(a);
				if(components != null && e.GetId() < components.Size()) {
					components.Set(e.GetId(), null);
				}
			}
		}
		
		/**
		 * Check if this entity is active, or has been deleted, within the framework.
		 * 
		 * @param entityId
		 * @return active or not.
		 */
		public bool IsActive(int entityId) {
			return activeEntities.Get(entityId) != null;
		}
		
		public void AddComponent(Entity e, Component component) {
			ComponentType type = ComponentTypeManager.GetTypeFor(component);
			
			if(type.GetId() >= componentsByType.GetCapacity()) {
				componentsByType.Set(type.GetId(), null);
			}
			
			Bag<Component> components = componentsByType.Get(type.GetId());
			if(components == null) {
				components = new Bag<Component>();
				componentsByType.Set(type.GetId(), components);
			}
			
			components.Set(e.GetId(), component);
	
			e.AddTypeBit(type.GetBit());
		}
		
		public void Refresh(Entity e) {
			SystemManager systemManager = world.GetSystemManager();
			Bag<EntitySystem> systems = systemManager.GetSystems();
			for(int i = 0, s=systems.Size(); s > i; i++) {
				systems.Get(i).Change(e);
			}
		}
		
		public void RemoveComponent(Entity e, Component component) {
			ComponentType type = ComponentTypeManager.GetTypeFor(component);
			RemoveComponent(e, type);
		}
		
		public void RemoveComponent(Entity e, ComponentType type) {
			Bag<Component> components = componentsByType.Get(type.GetId());
			components.Set(e.GetId(), null);
			e.RemoveTypeBit(type.GetBit());
		}
		
		public Component GetComponent(Entity e, ComponentType type) {
			Bag<Component> bag = componentsByType.Get(type.GetId());
			if(bag != null && e.GetId() < bag.GetCapacity())
				return bag.Get(e.GetId());
			return null;
		}
		
		public Entity GetEntity(int entityId) {
			return activeEntities.Get(entityId);
		}
		
		/**
		 * 
		 * @return how many entities are currently active.
		 */
		public int GetEntityCount() {
			return count;
		}
		
		/**
		 * 
		 * @return how many entities have been created since start.
		 */
		public long GetTotalCreated() {
			return totalCreated;
		}
		
		/**
		 * 
		 * @return how many entities have been removed since start.
		 */
		public long GetTotalRemoved() {
			return totalRemoved;
		}
	
		public Bag<Component> GetComponents(Entity e) {
			entityComponents.Clear();
			for(int a = 0; componentsByType.Size() > a; a++) {
				Bag<Component> components = componentsByType.Get(a);
				if(components != null && e.GetId() < components.Size()) {
					Component component = components.Get(e.GetId());
					if(component != null) {
						entityComponents.Add(component);
					}
				}
			}
			return entityComponents;
		}
	
	}
}

