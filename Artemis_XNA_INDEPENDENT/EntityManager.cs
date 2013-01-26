using System;

namespace Artemis
{
	public delegate void RemovedComponentHandler(Entity e, Component c);

	public delegate void RemovedEntityHandler(Entity e);

	public delegate void AddedComponentHandler(Entity e, Component c);

	public delegate void AddedEntityHandler(Entity e);

    /// <summary>
    /// Entity Manager
    /// </summary>
	public sealed class EntityManager
	{
		private EntityWorld world;
		private Bag<Entity> activeEntities = new Bag<Entity>();
		private Bag<Entity> removedAndAvailable = new Bag<Entity>();
		private int nextAvailableId;
		private int count;
		private long totalCreated;
		private long totalRemoved;
        private int removedEntitiesRetention = 100;

        public int RemovedEntitiesRetention
        {
            get
            {
                return removedEntitiesRetention;
            }
            set
            {
                this.removedEntitiesRetention = value;
            }
        }

		public event RemovedComponentHandler RemovedComponentEvent;

		public event RemovedEntityHandler RemovedEntityEvent;

		public event AddedComponentHandler AddedComponentEvent;

		public event AddedEntityHandler AddedEntityEvent;

		private Bag<Bag<Component>> componentsByType = new Bag<Bag<Component>>();

		private Bag<Component> entityComponents = new Bag<Component>(); // Added for debug support.

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityManager"/> class.
        /// </summary>
        /// <param name="world">The world.</param>
		public EntityManager(EntityWorld world)
		{
			System.Diagnostics.Debug.Assert(world != null);
			this.world = world;
            this.RemovedComponentEvent += EntityManager_RemovedComponentEvent;
		}

        void EntityManager_RemovedComponentEvent(Entity e, Component c)
        {
            if (c is ComponentPoolable)
            {
                ComponentPoolable ComponentPoolable = c as ComponentPoolable;
                if (ComponentPoolable.poolId < 0)
                    return;

                var pool = this.world.GetPool(c.GetType());
                if (pool != null)
                {
                    pool.ReturnObject(ComponentPoolable);
                }
            }
        }

		/// <summary>
		/// Create a new, "blank" entity
		/// </summary>
		/// <returns>New entity</returns>
		public Entity Create()
		{
			Entity e = removedAndAvailable.RemoveLast();
			if (e == null)
			{
				e = new Entity(world, nextAvailableId++);
			}
			else
			{
				e.Reset();
			}
			e.UniqueId = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
			activeEntities.Set(e.Id, e);
			count++;
			totalCreated++;
			if (AddedEntityEvent != null)
			{
				AddedEntityEvent(e);
			}
			return e;
		}

		/// <summary>
		/// Remove an entity from the world
		/// </summary>
		/// <param name="e">Entity you want to remove</param>
		public void Remove(Entity e)
		{
			System.Diagnostics.Debug.Assert(e != null);
			activeEntities.Set(e.Id, null);

			e.TypeBits = 0;

			Refresh(e);

			RemoveComponentsOfEntity(e);

			count--;
			totalRemoved++;

            if (removedAndAvailable.Size < removedEntitiesRetention)
            {
                removedAndAvailable.Add(e);
            }

			if (RemovedEntityEvent != null)
			{
				RemovedEntityEvent(e);
			}
		}

		/// <summary>
		/// Strips all components from the given entity
		/// </summary>
		/// <param name="e">Entity for which you want to remove all components</param>
		internal void RemoveComponentsOfEntity(Entity e)
		{
			System.Diagnostics.Debug.Assert(e != null);
			int entityId = e.Id;
			for (int a = 0, b = componentsByType.Size; b > a; a++)
			{
				Bag<Component> components = componentsByType.Get(a);
				if (components != null && entityId < components.Size)
				{
					if (RemovedComponentEvent != null)
					{
						RemovedComponentEvent(e, components.Get(entityId));
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
		public bool IsActive(int entityId)
		{
			return activeEntities.Get(entityId) != null;
		}

		/// <summary>
		/// Add the given component to the given entity
		/// </summary>
		/// <param name="e">Entty for which you want to add the component</param>
		/// <param name="component">Component you want to add</param>
		internal void AddComponent(Entity e, Component component)
		{
			System.Diagnostics.Debug.Assert(e != null);
			System.Diagnostics.Debug.Assert(component != null);
			ComponentType type = ComponentTypeManager.GetTypeFor(component.GetType());

			if (type.Id >= componentsByType.Capacity)
			{
				componentsByType.Set(type.Id, null);
			}

			Bag<Component> components = componentsByType.Get(type.Id);
			if (components == null)
			{
				components = new Bag<Component>();
				componentsByType.Set(type.Id, components);
			}

			components.Set(e.Id, component);

			e.AddTypeBit(type.Bit);
			if (AddedComponentEvent != null)
			{
				AddedComponentEvent(e, component);
			}
		}

		/// <summary>
		/// Add a component to the given entity
		/// If the component's type does not already exist, add it to the bag of availalbe component types
		/// </summary>
		/// <typeparam name="T">Component type you want to add</typeparam>
		/// <param name="e">The entity to which you want to add the component</param>
		/// <param name="component">The component instance you want to add</param>
		internal void AddComponent<T>(Entity e, Component component) where T : Component
		{
			System.Diagnostics.Debug.Assert(component != null);
			System.Diagnostics.Debug.Assert(e != null);
			ComponentType type = ComponentTypeManager.GetTypeFor<T>();

			if (type.Id >= componentsByType.Capacity)
			{
				componentsByType.Set(type.Id, null);
			}

			Bag<Component> components = componentsByType.Get(type.Id);
			if (components == null)
			{
				components = new Bag<Component>();
				componentsByType.Set(type.Id, components);
			}

			components.Set(e.Id, component);

			e.AddTypeBit(type.Bit);
			if (AddedComponentEvent != null)
			{
				AddedComponentEvent(e, component);
			}
		}

		/// <summary>
		/// Ensure the any changes to components are synced up with the entity - ensure systems "see" all components
		/// </summary>
		/// <param name="e">The entity whose components you want to refresh</param>
		internal void Refresh(Entity e)
		{
			SystemManager systemManager = world.SystemManager;
			Bag<EntitySystem> systems = systemManager.Systems;
			for (int i = 0, s = systems.Size; s > i; i++)
			{
				systems.Get(i).OnChange(e);
			}
		}

		/// <summary>
		/// Removes the given component from the given entity
		/// </summary>
		/// <typeparam name="T">The type of the component you want to remove</typeparam>
		/// <param name="e">The entity for which you are removing the component</param>
		/// <param name="component">The specific component instance you want removed</param>
		internal void RemoveComponent<T>(Entity e, Component component) where T : Component
		{
			System.Diagnostics.Debug.Assert(component != null);
			System.Diagnostics.Debug.Assert(e != null);
			ComponentType type = ComponentTypeManager.GetTypeFor<T>();
			RemoveComponent(e, type);
		}

        /// <summary>
        /// Removes the given component type from the given entity
        /// </summary>
        /// <typeparam name="T">The type of the component you want to remove</typeparam>
        /// <param name="e">The entity for which you are removing the component</param>
        /// <param name="component">The specific component type you want removed</param>
        internal void RemoveComponent<T>(Entity e, ComponentType componentType) where T : Component
        {
            System.Diagnostics.Debug.Assert(componentType != null);
            System.Diagnostics.Debug.Assert(e != null);
            RemoveComponent(e, componentType);
        }


		/// <summary>
		/// Reemoves the given component type from the given entity
		/// </summary>
		/// <param name="e">The entity for which you want to remove the component</param>
		/// <param name="type">The component type you want to remove</param>
		internal void RemoveComponent(Entity e, ComponentType type)
		{
			System.Diagnostics.Debug.Assert(e != null);
			System.Diagnostics.Debug.Assert(type != null);
			int entityId = e.Id;
			Bag<Component> components = componentsByType.Get(type.Id);
			if (RemovedComponentEvent != null)
			{
				RemovedComponentEvent(e, components.Get(entityId));
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
		internal Component GetComponent(Entity e, ComponentType type)
		{
			System.Diagnostics.Debug.Assert(e != null);
			System.Diagnostics.Debug.Assert(type != null);
			int entityId = e.Id;
			Bag<Component> bag = componentsByType.Get(type.Id);
			if (type.Id >= componentsByType.Capacity)
			{
				return null;
			}
			if (bag != null && entityId < bag.Capacity)
				return bag.Get(entityId);
			return null;
		}

		/// <summary>
		/// Get the entity for the given entityId
		/// </summary>
		/// <param name="entityId">Desired EntityId</param>
		/// <returns>Entity</returns>
		public Entity GetEntity(int entityId)
		{
			System.Diagnostics.Debug.Assert(entityId >= 0);

			return activeEntities.Get(entityId);
		}

		public Bag<Entity> GetEntities (Aspect aspect)
		{
			Bag<Entity> entitiesBag = new Bag<Entity> ();
			for (int i = 0; i < activeEntities.Size; i++) {
				Entity e = activeEntities.Get(i);
				if(aspect.Interests(e)) {
					entitiesBag.Add(e);
				}
			}
			return entitiesBag;
		}

		/// <summary>
		/// Get how many entities are currently active
		/// </summary>
		/// <returns>How many entities are currently active</returns>
		public int ActiveEntitiesCount
		{
			get { return count; }
		}

		/// <summary>
		/// Get how many entities have been created since start.
		/// </summary>
		/// <returns>The total number of entities created</returns>
		public long TotalCreated
		{
			get { return totalCreated; }
		}

		/// <summary>
		/// Gets how many entities have been removed since start.
		/// </summary>
		/// <returns>The total number of removed entities</returns>
		public long TotalRemoved
		{
			get { return totalRemoved; }
		}

		/// <summary>
		/// Get all components assigned to an entity
		/// </summary>
		/// <param name="e">Entity for which you want the components</param>
		/// <returns>Bag of components</returns>
		public Bag<Component> GetComponents(Entity e)
		{
			System.Diagnostics.Debug.Assert(e != null);
			entityComponents.Clear();
			int entityId = e.Id;
			for (int a = 0, b = componentsByType.Size; b > a; a++)
			{
				Bag<Component> components = componentsByType.Get(a);
				if (components != null && entityId < components.Size)
				{
					Component component = components.Get(entityId);
					if (component != null)
					{
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
		public Bag<Entity> ActiveEntities
		{
			get { return activeEntities; }
		}
	}
}