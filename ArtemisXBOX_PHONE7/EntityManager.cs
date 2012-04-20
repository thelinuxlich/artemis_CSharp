using System;
namespace Artemis
{
	public delegate void RemovedComponentHandler(Entity e,Component c);
	public delegate void RemovedEntityHandler(Entity e);
	public delegate void AddedComponentHandler(Entity e,Component c);
	public delegate void AddedEntityHandler(Entity e);
	
	public sealed class EntityManager {
		private EntityWorld world;
		private Bag<Entity> activeEntities = new Bag<Entity>();
		private Bag<Entity> removedAndAvailable = new Bag<Entity>();
		private int nextAvailableId;
		private int count;
		private long uniqueEntityId;
		private long totalCreated;
		private long totalRemoved;
		public event RemovedComponentHandler RemovedComponentEvent;
		public event RemovedEntityHandler RemovedEntityEvent;
		public event AddedComponentHandler AddedComponentEvent;
		public event AddedEntityHandler AddedEntityEvent;
		
		private Bag<Bag<Component>> componentsByType = new Bag<Bag<Component>>();
		
		private Bag<Component> entityComponents = new Bag<Component>(); // Added for debug support.
	
		public EntityManager(EntityWorld world) {
			this.world = world;
		}
	

        /// <summary>
        /// Create a new, "blank" entity
        /// </summary>
        /// <returns>New entity</returns>
		public Entity Create() {
			Entity e = removedAndAvailable.RemoveLast();
			if (e == null) {
				e = new Entity(world, nextAvailableId++);
			} else {
				e.Reset();
			}
			e.UniqueId = uniqueEntityId++;
			activeEntities.Set(e.Id,e);
			count++;
			totalCreated++;
			if(AddedEntityEvent != null) {
				AddedEntityEvent(e);
			}
			return e;
		}


        /// <summary>
        /// Remove an entity from the world
        /// </summary>
        /// <param name="e">Entity you want to remove</param>
		public void Remove(Entity e) {
			activeEntities.Set(e.Id, null);
			
			e.TypeBits = 0;
			
			Refresh(e);
			
			RemoveComponentsOfEntity(e);
			
			count--;
			totalRemoved++;
	
			removedAndAvailable.Add(e);
			if(RemovedEntityEvent != null) {
				RemovedEntityEvent(e);
			}	
		}
	

        /// <summary>
        /// Strips all components from the given entity
        /// </summary>
        /// <param name="e">Entity for which you want to remove all components</param>
		private void RemoveComponentsOfEntity(Entity e) {
			int entityId = e.Id;
			for(int a = 0,b = componentsByType.Size(); b > a; a++) {
				Bag<Component> components = componentsByType.Get(a);
				if(components != null && entityId < components.Size()) {
					if(RemovedComponentEvent != null) {
						RemovedComponentEvent(e,components.Get(entityId));
					}	
					components.Set(entityId, null);
				}
			}
		}
		
        /// <summary>
        /// Check if this entity is active, or has been deleted, within the framework.
        /// </summary>
        /// <param name="entityId">entityId</param>
        /// <returns>active or not.</returns>
		public bool IsActive(int entityId) {
			return activeEntities.Get(entityId) != null;
		}
		

        /// <summary>
        /// Add the given component to the given entity
        /// </summary>
        /// <param name="e">Entty for which you want to add the component</param>
        /// <param name="component">Component you want to add</param>
		public void AddComponent(Entity e, Component component) {
			ComponentType type = ComponentTypeManager.GetTypeFor(component.GetType());

			if(type.Id >= componentsByType.GetCapacity()) {
				componentsByType.Set(type.Id, null);
			}

			Bag<Component> components = componentsByType.Get(type.Id);
			if(components == null) {
				components = new Bag<Component>();
				componentsByType.Set(type.Id, components);
			}

			components.Set(e.Id, component);

			e.AddTypeBit(type.Bit);
			if(AddedComponentEvent != null) {
				AddedComponentEvent(e,component);
			}
		}
		


        /// <summary>
        /// Add a component to the given entity
        /// If the component's type does not already exist, add it to the bag of availalbe component types
        /// </summary>
        /// <typeparam name="T">Component type you want to add</typeparam>
        /// <param name="e">The entity to which you want to add the component</param>
        /// <param name="component">The component instance you want to add</param>
		public void AddComponent<T>(Entity e, Component component) where T : Component {
			ComponentType type = ComponentTypeManager.GetTypeFor<T>();
			
			if(type.Id >= componentsByType.GetCapacity()) {
				componentsByType.Set(type.Id, null);
			}
			
			Bag<Component> components = componentsByType.Get(type.Id);
			if(components == null) {
				components = new Bag<Component>();
				componentsByType.Set(type.Id, components);
			}
			
			components.Set(e.Id, component);
	
			e.AddTypeBit(type.Bit);
			if(AddedComponentEvent != null) {
				AddedComponentEvent(e,component);
			}
		}


        /// <summary>
        /// Ensure the any changes to components are synced up with the entity - ensure systems "see" all components
        /// </summary>
        /// <param name="e">The entity whose components you want to refresh</param>
		public void Refresh(Entity e) {
			SystemManager systemManager = world.SystemManager;
			Bag<EntitySystem> systems = systemManager.Systems;
			for(int i = 0, s=systems.Size(); s > i; i++) {
				systems.Get(i).Change(e);
			}
		}



        /// <summary>
        /// Removes the given component from the given entity
        /// </summary>
        /// <typeparam name="T">The type of the component you want to remove</typeparam>
        /// <param name="e">The entity for which you are removing the component</param>
        /// <param name="component">The specific component instance you want removed</param>
		public void RemoveComponent<T>(Entity e, Component component) where T : Component {
			ComponentType type = ComponentTypeManager.GetTypeFor<T>();
			RemoveComponent(e, type);
		}
		


        /// <summary>
        /// Reemoves the given component type from the given entity
        /// </summary>
        /// <param name="e">The entity for which you want to remove the component</param>
        /// <param name="type">The component type you want to remove</param>
		public void RemoveComponent(Entity e, ComponentType type) {
			int entityId = e.Id;
			Bag<Component> components = componentsByType.Get(type.Id);
			if(RemovedComponentEvent != null) {
				RemovedComponentEvent(e,components.Get(entityId));
			}	
			components.Set(entityId, null);
			e.RemoveTypeBit(type.Bit);
		}
		

        /// <summary>
        /// Get the component instance of the given component type for the given entity
        /// </summary>
        /// <param name="e">The entity for which you want to get the component</param>
        /// <param name="type">The desired component type</param>
        /// <returns>Component instance</returns>
		public Component GetComponent(Entity e, ComponentType type) {
			int entityId = e.Id;
			Bag<Component> bag = componentsByType.Get(type.Id);
			if (type.Id >= componentsByType.GetCapacity()) {
 	 		  	return null;
			}
			if(bag != null && entityId < bag.GetCapacity())
				return bag.Get(entityId);
			return null;
		}
		

        /// <summary>
        /// Get the entity for the given entityId
        /// </summary>
        /// <param name="entityId">Desired EntityId</param>
        /// <returns>Entity</returns>
		public Entity GetEntity(int entityId) {
			return activeEntities.Get(entityId);
		}

        /// <summary>
        /// Get how many entities are currently active
        /// </summary>
        /// <returns>How many entities are currently active</returns>
		public int EntityCount {
			get { return count;}
		}
		
        /// <summary>
        /// Get how many entities have been created since start.
        /// </summary>
        /// <returns>The total number of entities created</returns>
		public long TotalCreated {
			get { return totalCreated;}
		}
		
        /// <summary>
        /// Gets how many entities have been removed since start.
        /// </summary>
        /// <returns>The total number of removed entities</returns>
		public long TotalRemoved {
			get { return totalRemoved;}
		}
	
        /// <summary>
        /// Get all components assigned to an entity
        /// </summary>
        /// <param name="e">Entity for which you want the components</param>
        /// <returns>Bag of components</returns>
		public Bag<Component> GetComponents(Entity e) {
			entityComponents.Clear();
			int entityId = e.Id;
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
		
        /// <summary>
        /// Get all active Entities
        /// </summary>
        /// <returns>Bag of active entities</returns>
		public Bag<Entity> ActiveEntities {
			get { return activeEntities;}
		}
	}
}

