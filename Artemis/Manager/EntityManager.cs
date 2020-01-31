#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityManager.cs" company="GAMADU.COM">
//     Copyright � 2013 GAMADU.COM. All rights reserved.
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
//   The Entity Manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Manager
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics;

    using Artemis.Interface;
    using Artemis.System;
    using Artemis.Utils;
#if UNITY5
    using MathNet.Numerics;
#endif

#endregion Using statements

    /// <summary>The Entity Manager.</summary>
    public sealed class EntityManager
    {
        /// <summary>The components by type.</summary>
        private readonly Bag<Bag<IComponent>> componentsByType;

        /// <summary>The removed and available.</summary>
        private readonly Bag<Entity> removedAndAvailable;

        /// <summary>Components to be removed in the next update</summary>
#if XBOX || WINDOWS_PHONE || PORTABLE
   		private readonly Bag<Tuple<Entity, ComponentType>> componentsToBeRemoved = new Bag<Tuple<Entity, ComponentType>>();
#else
        private readonly HashSet<Tuple<Entity, ComponentType>> componentsToBeRemoved = new HashSet<Tuple<Entity, ComponentType>>();
#endif

        /// <summary>Map unique id to entities</summary>
        private readonly Dictionary<long, Entity> uniqueIdToEntities;

        /// <summary>The entity world.</summary>
        private readonly EntityWorld entityWorld;

        /// <summary>The next available id.</summary>
        private int nextAvailableId;

        private readonly Bag<int> identifierPool;

        /// <summary>Initializes a new instance of the <see cref="EntityManager" /> class.</summary>
        /// <param name="entityWorld">The entity world.</param>
        public EntityManager(EntityWorld entityWorld)
        {
            Debug.Assert(entityWorld != null, "EntityWorld must not be null.");

            this.uniqueIdToEntities = new Dictionary<long, Entity>();
            this.removedAndAvailable = new Bag<Entity>();
            this.componentsByType = new Bag<Bag<IComponent>>();
            this.ActiveEntities = new Bag<Entity>();
            this.identifierPool = new Bag<int>(4);
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

#if DEBUG
        /// <summary>Gets how many entities are currently active. Only available in debug mode.</summary>
        /// <value>The active entities count.</value>
        /// <returns>How many entities are currently active.</returns>
        public int EntitiesRequestedCount { get; private set; }
#endif
        /// <summary>Gets or sets the removed entities retention.</summary>
        /// <value>The removed entities retention.</value>
        public int RemovedEntitiesRetention { get; set; }
#if DEBUG
        /// <summary>Gets how many entities have been created since start. Only available in debug mode.</summary>
        /// <value>The total created.</value>
        /// <returns>The total number of entities created.</returns>
        public long TotalCreated { get; private set; }

        /// <summary>Gets how many entities have been removed since start. Only available in debug mode.</summary>
        /// <value>The total removed.</value>
        /// <returns>The total number of removed entities.</returns>
        public long TotalRemoved { get; private set; }
#endif
        /// <summary>Create a new, "blank" entity.</summary>
        /// <param name="uniqueid">The unique id.</param>
        /// <returns>New entity.</returns>
        public Entity Create(long? uniqueid = null)
        {
            long id = uniqueid.HasValue ? uniqueid.Value : BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);

            Entity result = this.removedAndAvailable.RemoveLast();
            if (result == null)
            {
                int entityId = this.identifierPool.IsEmpty
                    ? this.nextAvailableId++
                    : this.identifierPool.RemoveLast();

                result = new Entity(this.entityWorld, entityId);
            }
            else
            {
                result.Reset();
            }

            result.UniqueId = id;
            this.uniqueIdToEntities[result.UniqueId] = result;
            this.ActiveEntities.Set(result.Id, result);
#if DEBUG
            ++this.EntitiesRequestedCount;

            if (this.TotalCreated < long.MaxValue)
            {
                ++this.TotalCreated;
            }
#endif
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
            //Debug.Assert(entity.entityManager == this, "");  // TODO

            Bag<IComponent> entityComponents = new Bag<IComponent>();
            int entityId = entity.Id;
            for (int index = 0, b = this.componentsByType.Count; b > index; ++index)
            {
                Bag<IComponent> components = this.componentsByType.Get(index);
                if (components != null && entityId < components.Count)
                {
                    IComponent component = components.Get(entityId);
                    if (component != null)
                    {
                        entityComponents.Add(component);
                    }
                }
            }

            return entityComponents;
        }

        /// <summary>Gets the entities.</summary>
        /// <param name="aspect">The aspect.</param>
        /// <returns>The filled Bag{Entity}.</returns>
        public Bag<Entity> GetEntities(Aspect aspect)
        {
            Bag<Entity> entitiesBag = new Bag<Entity>();
            for (int index = 0; index < this.ActiveEntities.Count; ++index)
            {
                Entity entity = this.ActiveEntities.Get(index);
                if (entity != null && aspect.Interests(entity))
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

        /// <summary>Gets the entity by unique ID. Note: that UniqueId is different from Id.</summary>
        /// <param name="entityUniqueId">The entity unique id.</param>
        /// <returns>The Entity.</returns>
        public Entity GetEntityByUniqueId(long entityUniqueId)
        {
            Debug.Assert(entityUniqueId != -1, "Id must != -1");
            Entity entity;
            this.uniqueIdToEntities.TryGetValue(entityUniqueId, out entity);
            return entity;
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
            //Debug.Assert(entity.entityManager == this, "");  // TODO

            this.ActiveEntities.Set(entity.Id, null);
            this.RemoveComponentsOfEntity(entity);
#if DEBUG
            --this.EntitiesRequestedCount;

            if (this.TotalRemoved < long.MaxValue)
            {
                ++this.TotalRemoved;
            }
#endif
            if (this.removedAndAvailable.Count < this.RemovedEntitiesRetention)
            {
                this.removedAndAvailable.Add(entity);
            }
            else
            {
                this.identifierPool.Add(entity.Id);
            }

            if (this.RemovedEntityEvent != null)
            {
                this.RemovedEntityEvent(entity);
            }

            this.uniqueIdToEntities.Remove(entity.UniqueId);
        }

        /// <summary>Add the given component to the given entity.</summary>
        /// <param name="entity">Entity for which you want to add the component.</param>
        /// <param name="component">Component you want to add.</param>
        internal void AddComponent(Entity entity, IComponent component)
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            Debug.Assert(component != null, "Component must not be null.");

            ComponentType type = ComponentTypeManager.GetTypeFor(component.GetType());

            this.AddComponent(entity, component, type);
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

            this.AddComponent(entity, component, type);
        }

        /// <summary>Adds the component.</summary>
        /// <param name="entity">The entity.</param>
        /// <param name="component">The component.</param>
        /// <param name="type">The type.</param>
        internal void AddComponent(Entity entity, IComponent component, ComponentType type)
        {
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

            this.entityWorld.RefreshEntity(entity);
        }

        /// <summary>Get the component instance of the given component type for the given entity.</summary>
        /// <param name="entity">The entity for which you want to get the component</param>
        /// <param name="componentType">The desired component type</param>
        /// <returns>Component instance</returns>
        internal IComponent GetComponent(Entity entity, ComponentType componentType)
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            Debug.Assert(componentType != null, "Component type must not be null.");

            if (componentType.Id >= this.componentsByType.Capacity)
            {
                return null;
            }

            int entityId = entity.Id;
            Bag<IComponent> bag = this.componentsByType.Get(componentType.Id);

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
            for (int index = 0, s = systems.Count; s > index; ++index)
            {
                systems.Get(index).OnChange(entity);
            }
        }

        /// <summary>Removes the given component from the given entity.</summary>
        /// <typeparam name="T">The type of the component you want to remove.</typeparam>
        /// <param name="entity">The entity for which you are removing the component.</param>
        internal void RemoveComponent<T>(Entity entity) where T : IComponent
        {
            this.RemoveComponent(entity, ComponentType<T>.CType);
        }

        /// <summary>Removes the given component type from the given entity.</summary>
        /// <param name="entity">The entity for which you want to remove the component.</param>
        /// <param name="componentType">The component type you want to remove.</param>
        internal void RemoveComponent(Entity entity, ComponentType componentType)
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            Debug.Assert(componentType != null, "Component type must not be null.");

            var pair = new Tuple<Entity, ComponentType>(entity, componentType);
#if XBOX || WINDOWS_PHONE || PORTABLE
            if (!this.componentsToBeRemoved.Contains(pair))
            {
                this.componentsToBeRemoved.Add(pair);
            }
#else
            this.componentsToBeRemoved.Add(pair);
#endif
        }

        /// <summary>Removes components marked for removal</summary>
        internal void RemoveMarkedComponents()
        {
#if XBOX || WINDOWS_PHONE || PORTABLE
            for (int index = this.componentsToBeRemoved.Count - 1; index >= 0; --index)
            {
                var pair = this.componentsToBeRemoved.Get(index);
#else
            foreach (var pair in this.componentsToBeRemoved)
            {
#endif
                var entity = pair.Item1;
                var componentType = pair.Item2;

                int entityId = entity.Id;
                Bag<IComponent> components = this.componentsByType.Get(componentType.Id);

                if (components != null && entityId < components.Count)
                {
                    IComponent componentToBeRemoved = components.Get(entityId);
                    if (this.RemovedComponentEvent != null && componentToBeRemoved != null)
                    {
                        this.RemovedComponentEvent(entity, componentToBeRemoved);
                    }

                    entity.RemoveTypeBit(componentType.Bit);
                    this.entityWorld.RefreshEntity(entity);
                    components.Set(entityId, null);
                }
            }
            this.componentsToBeRemoved.Clear();
        }

        /// <summary>Strips all components from the given entity.</summary>
        /// <param name="entity">Entity for which you want to remove all components</param>
        internal void RemoveComponentsOfEntity(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            entity.TypeBits = 0;
            this.entityWorld.RefreshEntity(entity);

            int entityId = entity.Id;
            for (int index = this.componentsByType.Count - 1; index >= 0; --index)
            {
                Bag<IComponent> components = this.componentsByType.Get(index);
                if (components != null && entityId < components.Count)
                {
                    IComponent componentToBeRemoved = components.Get(entityId);
                    if (this.RemovedComponentEvent != null && componentToBeRemoved != null)
                    {
                        this.RemovedComponentEvent(entity, componentToBeRemoved);
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
            ComponentPoolable componentPoolable = component as ComponentPoolable;
            if (componentPoolable != null)
            {
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
