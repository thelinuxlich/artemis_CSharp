namespace Artemis.Manager
{
    #region Using statements

    using global::System;
    using global::System.Diagnostics;

    using Artemis.Interface;
    using Artemis.System;
    using Artemis.Utils;

    #endregion Using statements

    /// <summary>The Entity Manager.</summary>
    public sealed class EntityManager
    {
        /// <summary>The components by type.</summary>
        private readonly Bag<Bag<IComponent>> componentsByType;

        /// <summary>The entity components. Added for debug support.</summary>
        private readonly Bag<IComponent> entityComponents;

        /// <summary>The removed and available.</summary>
        private readonly Bag<Entity> removedAndAvailable;

        /// <summary>The entity world.</summary>
        private readonly EntityWorld entityWorld;

        /// <summary>The next available id.</summary>
        private int nextAvailableId;

        /// <summary>Initializes a new instance of the <see cref="EntityManager" /> class.</summary>
        /// <param name="entityWorld">The entity world.</param>
        public EntityManager(EntityWorld entityWorld)
        {
            Debug.Assert(entityWorld != null, "EntityWorld must not be null.");

            this.removedAndAvailable = new Bag<Entity>();
            this.entityComponents = new Bag<IComponent>();
            this.componentsByType = new Bag<Bag<IComponent>>();
            this.ActiveEntities = new Bag<Entity>();
            this.RemovedEntitiesRetention = 100;
            this.entityWorld = entityWorld;
            this.RemovedComponentEvent += this.EntityManagerRemovedComponentEvent;
        }

        /// <summary>Occurs when [added component event].</summary>
        public event AddedComponentHandler AddedComponentEvent;

        /// <summary>Occurs when [added entity event].</summary>
        public event AddedEntityHandler AddedEntityEvent;

        /// <summary>Occurs when [removed component event].</summary>
        public event RemovedComponentHandler RemovedComponentEvent;

        /// <summary>Occurs when [removed entity event].</summary>
        public event RemovedEntityHandler RemovedEntityEvent;

        /// <summary>Gets all active Entities.</summary>
        /// <value>The active entities.</value>
        /// <returns>Bag of active entities.</returns>
        public Bag<Entity> ActiveEntities { get; private set; }

        /// <summary>Gets the number of entities are currently active.</summary>
        /// <value>The active entities count.</value>
        /// <returns>The number of entities are currently active.</returns>
        public int ActiveEntitiesCount { get; private set; }

        /// <summary>Gets or sets the removed entities retention.</summary>
        /// <value>The removed entities retention.</value>
        public int RemovedEntitiesRetention { get; set; }

        /// <summary>Gets the number of entities have been created since start.</summary>
        /// <value>The total created.</value>
        /// <returns>The total number of entities created.</returns>
        public long TotalCreated { get; private set; }

        /// <summary>Gets how many entities have been removed since start.</summary>
        /// <value>The total removed.</value>
        /// <returns>The total number of removed entities.</returns>
        public long TotalRemoved { get; private set; }

        /// <summary>Create a new, "blank" entity.</summary>
        /// <returns>New entity</returns>
        public Entity Create()
        {
            Entity result = this.removedAndAvailable.RemoveLast();
            if (result == null)
            {
                result = new Entity(this.entityWorld, this.nextAvailableId++);
            }
            else
            {
                result.Reset();
            }

            result.UniqueId = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
            this.ActiveEntities.Set(result.Id, result);

            // TODO: Prevent buffer overflows here!
            ++this.ActiveEntitiesCount;

            // TODO: Prevent buffer overflows here!
            ++this.TotalCreated;

            if (this.AddedEntityEvent != null)
            {
                this.AddedEntityEvent(result);
            }

            return result;
        }

        /// <summary>Get all components assigned to an entity.</summary>
        /// <param name="entity">Entity for which you want the components.</param>
        /// <returns>Bag of components</returns>
        public Bag<IComponent> GetComponents(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            this.entityComponents.Clear();
            int entityId = entity.Id;
            for (int index = 0, b = this.componentsByType.Size; b > index; ++index)
            {
                Bag<IComponent> components = this.componentsByType.Get(index);
                if (components != null && entityId < components.Size)
                {
                    IComponent component = components.Get(entityId);
                    if (component != null)
                    {
                        this.entityComponents.Add(component);
                    }
                }
            }

            return this.entityComponents;
        }

        /// <summary>Gets the entities.</summary>
        /// <param name="aspect">The aspect.</param>
        /// <returns>A filtered by aspects Bag{Entity}.</returns>
        public Bag<Entity> GetEntities(Aspect aspect)
        {
            Bag<Entity> entitiesBag = new Bag<Entity>();
            for (int index = 0; index < this.ActiveEntities.Size; ++index)
            {
                Entity entity = this.ActiveEntities.Get(index);
                if (aspect.Interests(entity))
                {
                    entitiesBag.Add(entity);
                }
            }

            return entitiesBag;
        }

        /// <summary>Get the entity for the given entityId</summary>
        /// <param name="entityId">Desired EntityId</param>
        /// <returns>The specified Entity.</returns>
        public Entity GetEntity(int entityId)
        {
            Debug.Assert(entityId >= 0, "Id must be at least 0.");

            return this.ActiveEntities.Get(entityId);
        }

        /// <summary>Check if this entity is active, or has been deleted, within the framework.</summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns><see langword="true" /> if the specified entity is active; otherwise, <see langword="false" />.</returns>
        public bool IsActive(int entityId)
        {
            return this.ActiveEntities.Get(entityId) != null;
        }

        /// <summary>Remove an entity from the entityWorld.</summary>
        /// <param name="entity">Entity you want to remove.</param>
        public void Remove(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            this.ActiveEntities.Set(entity.Id, null);

            entity.TypeBits = 0;

            this.Refresh(entity);

            this.RemoveComponentsOfEntity(entity);

            --this.ActiveEntitiesCount;

            // TODO: Prevent buffer overflows here!
            ++this.TotalRemoved;

            if (this.removedAndAvailable.Size < this.RemovedEntitiesRetention)
            {
                this.removedAndAvailable.Add(entity);
            }

            if (this.RemovedEntityEvent != null)
            {
                this.RemovedEntityEvent(entity);
            }
        }

        /// <summary>Add the given component to the given entity.</summary>
        /// <param name="entity">Entity for which you want to add the component.</param>
        /// <param name="component">Component you want to add.</param>
        internal void AddComponent(Entity entity, IComponent component)
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            Debug.Assert(component != null, "Component must not be null.");

            ComponentType type = ComponentTypeManager.GetTypeFor(component.GetType());

            if (type.Id >= this.componentsByType.Capacity)
            {
                this.componentsByType.Set(type.Id, null);
            }

            Bag<IComponent> components = this.componentsByType.Get(type.Id);
            if (components == null)
            {
                components = new Bag<IComponent>();
                this.componentsByType.Set(type.Id, components);
            }

            components.Set(entity.Id, component);

            entity.AddTypeBit(type.Bit);
            if (this.AddedComponentEvent != null)
            {
                this.AddedComponentEvent(entity, component);
            }
        }

        /// <summary>
        /// <para>Add a component to the given entity.</para>
        /// <para>If the component's type does not already exist,</para>
        /// <para>add it to the bag of available component types.</para>
        /// </summary>
        /// <typeparam name="T">Component type you want to add.</typeparam>
        /// <param name="entity">The entity to which you want to add the component.</param>
        /// <param name="component">The component instance you want to add.</param>
        internal void AddComponent<T>(Entity entity, IComponent component) where T : IComponent
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            Debug.Assert(component != null, "Component must not be null.");

            ComponentType type = ComponentTypeManager.GetTypeFor<T>();

            if (type.Id >= this.componentsByType.Capacity)
            {
                this.componentsByType.Set(type.Id, null);
            }

            Bag<IComponent> components = this.componentsByType.Get(type.Id);
            if (components == null)
            {
                components = new Bag<IComponent>();
                this.componentsByType.Set(type.Id, components);
            }

            components.Set(entity.Id, component);

            entity.AddTypeBit(type.Bit);
            if (this.AddedComponentEvent != null)
            {
                this.AddedComponentEvent(entity, component);
            }
        }

        /// <summary>Get the component instance of the given component type for the given entity.</summary>
        /// <param name="entity">The entity for which you want to get the component</param>
        /// <param name="componentType">The desired component type</param>
        /// <returns>Component instance</returns>
        internal IComponent GetComponent(Entity entity, ComponentType componentType)
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            Debug.Assert(componentType != null, "Component type must not be null.");

            int entityId = entity.Id;
            Bag<IComponent> bag = this.componentsByType.Get(componentType.Id);
            if (componentType.Id >= this.componentsByType.Capacity)
            {
                return null;
            }

            if (bag != null && entityId < bag.Capacity)
            {
                return bag.Get(entityId);
            }

            return null;
        }

        /// <summary>Ensure the any changes to components are synced up with the entity - ensure systems "see" all components.</summary>
        /// <param name="entity">The entity whose components you want to refresh</param>
        internal void Refresh(Entity entity)
        {
            SystemManager systemManager = this.entityWorld.SystemManager;
            Bag<EntitySystem> systems = systemManager.Systems;
            for (int index = 0, s = systems.Size; s > index; ++index)
            {
                systems.Get(index).OnChange(entity);
            }
        }

        /// <summary>Removes the given component from the given entity.</summary>
        /// <typeparam name="T">The type of the component you want to remove.</typeparam>
        /// <param name="entity">The entity for which you are removing the component.</param>
        /// <param name="component">The specific component instance you want removed.</param>
        internal void RemoveComponent<T>(Entity entity, IComponent component) where T : IComponent
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            Debug.Assert(component != null, "Component must not be null.");

            ComponentType type = ComponentTypeManager.GetTypeFor<T>();
            this.RemoveComponent(entity, type);
        }

        /// <summary>Removes the given component type from the given entity.</summary>
        /// <param name="entity">The entity for which you want to remove the component.</param>
        /// <param name="componentType">The component type you want to remove.</param>
        internal void RemoveComponent(Entity entity, ComponentType componentType)
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            Debug.Assert(componentType != null, "Component type must not be null.");

            int entityId = entity.Id;
            Bag<IComponent> components = this.componentsByType.Get(componentType.Id);
            if (this.RemovedComponentEvent != null)
            {
                this.RemovedComponentEvent(entity, components.Get(entityId));
            }

            components.Set(entityId, null);
            entity.RemoveTypeBit(componentType.Bit);
        }

        /// <summary>Strips all components from the given entity.</summary>
        /// <param name="entity">Entity for which you want to remove all components</param>
        internal void RemoveComponentsOfEntity(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            int entityId = entity.Id;
            for (int index = 0, b = this.componentsByType.Size; b > index; ++index)
            {
                Bag<IComponent> components = this.componentsByType.Get(index);
                if (components != null && entityId < components.Size)
                {
                    if (this.RemovedComponentEvent != null)
                    {
                        this.RemovedComponentEvent(entity, components.Get(entityId));
                    }

                    components.Set(entityId, null);
                }
            }
        }

        /// <summary>Entities the manager removed component event.</summary>
        /// <param name="entity">The entity.</param>
        /// <param name="component">The component.</param>
        private void EntityManagerRemovedComponentEvent(Entity entity, IComponent component)
        {
            if (component is ComponentPoolable)
            {
                ComponentPoolable componentPoolable = component as ComponentPoolable;
                if (componentPoolable.PoolId < 0)
                {
                    return;
                }

                IComponentPool<ComponentPoolable> pool = this.entityWorld.GetPool(component.GetType());
                if (pool != null)
                {
                    pool.ReturnObject(componentPoolable);
                }
            }
        }
    }
}