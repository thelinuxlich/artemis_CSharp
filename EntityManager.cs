using System;
namespace Artemis
{
	public sealed class EntityManager {
		private World world;
		private Bag<Entity> activeEntities = new Bag<Entity>();
		private Bag<Entity> removedAndAvailable = new Bag<Entity>();
		private int nextAvailableId;
		private int count;
		private long uniqueEntityId;
		private long totalCreated;
		private long totalRemoved;
		
		private Bag<Bag<Component>> componentsByType = new Bag<Bag<Component>>();
		
		private Bag<Component> entityComponents = new Bag<Component>(); // Added for debug support.
	
		public EntityManager(World world) {
			this.world = world;
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
			int entityId = e.GetId();
			for(int a = 0,b = componentsByType.Size(); b > a; a++) {
				Bag<Component> components = componentsByType.Get(a);
				if(components != null && entityId < components.Size()) {
					components.Set(entityId, null);
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
			ComponentType type = ComponentTypeManager.GetTypeFor(component.GetType());
			
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
			ComponentType type = ComponentTypeManager.GetTypeFor(component.GetType());
			RemoveComponent(e, type);
		}
		
		public void RemoveComponent(Entity e, ComponentType type) {
			Bag<Component> components = componentsByType.Get(type.GetId());
			components.Set(e.GetId(), null);
			e.RemoveTypeBit(type.GetBit());
		}
		
		public Component GetComponent(Entity e, ComponentType type) {
			int entityId = e.GetId();
			Bag<Component> bag = componentsByType.Get(type.GetId());
			if(bag != null && entityId < bag.GetCapacity())
				return bag.Get(entityId);
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
			int entityId = e.GetId();
			for(int a = 0,b = componentsByType.Size(); b > a; a++) {
				Bag<Component> components = componentsByType.Get(a);
				if(components != null && entityId < components.Size()) {
					Component component = components.Get(entityId);
					if(component != null) {
						entityComponents.Add(component);
					}
				}
			}
			return entityComponents;
		}
	
	}
}

