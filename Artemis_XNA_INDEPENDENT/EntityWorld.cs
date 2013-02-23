#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityWorld.cs" company="GAMADU.COM">
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
//   The Entity World Class. Main interface of the Entity System.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics;

    using Artemis.Interface;
    using Artemis.Manager;
    using Artemis.Utils;

    #endregion Using statements

    /// <summary>
    /// <para>The Entity World Class.</para>
    /// <para>Main interface of the Entity System.</para>
    /// </summary>
    public sealed class EntityWorld
    {
        /// <summary>The deleted.</summary>
        private readonly Bag<Entity> deleted;

        /// <summary>The entity templates.</summary>
        private readonly Dictionary<string, IEntityTemplate> entityTemplates;

        /// <summary>The pools.</summary>
        private readonly Dictionary<Type, IComponentPool<ComponentPoolable>> pools;

        /// <summary>The refreshed.</summary>
        private readonly Bag<Entity> refreshed;

        /// <summary>The pool cleanup delay counter.</summary>
        private int poolCleanupDelayCounter;

#if !XBOX && !WINDOWS_PHONE
        /// <summary>Initializes a new instance of the <see cref="EntityWorld"/> class.</summary>
        /// <param name="isSortedEntities">if set to <c>true</c> [is sorted entities].</param>
        public EntityWorld(bool isSortedEntities = false)
        {
            this.IsSortedEntities = isSortedEntities;
#else
        /// <summary>Initializes a new instance of the <see cref="EntityWorld"/> class.</summary>
        public EntityWorld()
        {
            this.IsSortedEntities = false;
#endif
            this.refreshed = new Bag<Entity>();
            this.pools = new Dictionary<Type, IComponentPool<ComponentPoolable>>();
            this.entityTemplates = new Dictionary<string, IEntityTemplate>();
            this.deleted = new Bag<Entity>();
            this.EntityManager = new EntityManager(this);
            this.SystemManager = new SystemManager(this);
            this.TagManager = new TagManager();
            this.GroupManager = new GroupManager();
            this.PoolCleanupDelay = 10;
        }

        /// <summary>Gets the current state of the entity world.</summary>
        /// <value>The state of the current.</value>
        public Dictionary<Entity, Bag<IComponent>> CurrentState
        {
            get
            {
                Bag<Entity> entities = this.EntityManager.ActiveEntities;
                Dictionary<Entity, Bag<IComponent>> currentState = new Dictionary<Entity, Bag<IComponent>>();
                for (int index = 0, j = entities.Size; index < j; ++index)
                {
                    Entity e = entities.Get(index);
                    Bag<IComponent> components = e.Components;
                    currentState.Add(e, components);
                }

                return currentState;
            }
        }

        /// <summary>Gets the elapsed time.</summary>
        /// <value>The elapsed time.</value>
        public float ElapsedTime { get; private set; }

        /// <summary>Gets the entity manager.</summary>
        /// <value>The entity manager.</value>
        public EntityManager EntityManager { get; private set; }

        /// <summary>Gets the group manager.</summary>
        /// <value>The group manager.</value>
        public GroupManager GroupManager { get; private set; }

        /// <summary>Gets or sets the interval in FrameUpdates between pools cleanup. Default is 10.</summary>
        /// <value>The pool cleanup delay.</value>
        public int PoolCleanupDelay { get; set; }

        /// <summary>Gets the system manager.</summary>
        /// <value>The system manager.</value>
        public SystemManager SystemManager { get; private set; }

        /// <summary>Gets the tag manager.</summary>
        /// <value>The tag manager.</value>
        public TagManager TagManager { get; private set; }

        /// <summary>Gets a value indicating whether this instance is sorted entities.</summary>
        /// <value><see langword="true" /> if this instance is sorted entities; otherwise, <see langword="false" />.</value>
        internal bool IsSortedEntities { get; private set; }

        /// <summary>Creates the entity.</summary>
        /// <returns>A new entity.</returns>
        public Entity CreateEntity()
        {
            return this.EntityManager.Create();
        }

        /// <summary>Creates a entity from template.</summary>
        /// <param name="entityTemplateTag">The entity template tag.</param>
        /// <param name="templateArgs">The template args.</param>
        /// <returns>The created entity.</returns>
        /// <exception cref="Exception">EntityTemplate for the tag "entityTemplateTag" was not registered.</exception>
        public Entity CreateEntityFromTemplate(string entityTemplateTag, params object[] templateArgs)
        {
            Debug.Assert(!string.IsNullOrEmpty(entityTemplateTag), "Entity template tag must not be null or empty.");

            Entity entity = this.EntityManager.Create();
            IEntityTemplate entityTemplate;
            this.entityTemplates.TryGetValue(entityTemplateTag, out entityTemplate);
            if (entityTemplate == null)
            {
                throw new Exception("EntityTemplate for the tag " + entityTemplateTag + " was not registered.");
            }

            return entityTemplate.BuildEntity(entity, this, templateArgs);
        }

        /// <summary>Deletes the entity.</summary>
        /// <param name="entity">The entity.</param>
        public void DeleteEntity(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            this.deleted.Add(entity);
        }

        /// <summary>Gets a component from a pool.</summary>
        /// <param name="type">The type of the object to get.</param>
        /// <returns>The found component.</returns>
        /// <exception cref="Exception">There is no pool for the specified type</exception>
        public IComponent GetComponentFromPool(Type type)
        {
            Debug.Assert(type != null, "Type must not be null.");

            if (!this.pools.ContainsKey(type))
            {
                throw new Exception("There is no pool for the specified type " + type);
            }

            return this.pools[type].New();
        }

        /// <summary>Gets the component from pool.</summary>
        /// <typeparam name="T">Type of the component</typeparam>
        /// <returns>The found component.</returns>
        /// <exception cref="Exception">There is no pool for the type  + type</exception>
        public IComponent GetComponentFromPool<T>() where T : ComponentPoolable
        {
            Type type = typeof(T);
            if (!this.pools.ContainsKey(type))
            {
                throw new Exception("There is no pool for the specified type " + type);
            }

            return this.pools[type].New();
        }

        /// <summary>Gets the entity.</summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>The specified entity.</returns>
        public Entity GetEntity(int entityId)
        {
            Debug.Assert(entityId >= 0, "Id must be at least 0.");

            return this.EntityManager.GetEntity(entityId);
        }

        /// <summary>Gets the pool for a Type.</summary>
        /// <param name="type">The type.</param>
        /// <returns>The specified ComponentPool{ComponentPool-able}.</returns>
        public IComponentPool<ComponentPoolable> GetPool(Type type)
        {
            Debug.Assert(type != null, "Type must not be null.");

            return this.pools[type];
        }

        /// <summary>Initialize the EntityWorld.</summary>
        /// <param name="processAttributes">if set to <see langword="true" /> [process attributes].</param>
#if FULLDOTNET
        public void InitializeAll(bool processAttributes = true)
#else
        public void InitializeAll(bool processAttributes = false)
#endif
        {
            this.SystemManager.InitializeAll(processAttributes);
        }

        /// <summary>Loads the state of the entity.</summary>
        /// <param name="templateTag">The template tag. Can be null.</param>
        /// <param name="groupName">Name of the group. Can be null.</param>
        /// <param name="components">The components.</param>
        /// <param name="templateArgs">Parameters for entity template.</param>
        public void LoadEntityState(string templateTag, string groupName, Bag<IComponent> components, params object[] templateArgs)
        {
            Debug.Assert(components != null, "Components must not be null.");

            Entity entity;
            if (!string.IsNullOrEmpty(templateTag))
            {
                entity = this.CreateEntityFromTemplate(templateTag, templateArgs);
            }
            else
            {
                entity = this.CreateEntity();
            }

            if (string.IsNullOrEmpty(groupName))
            {
                this.GroupManager.Set(groupName, entity);
            }

            for (int index = 0, j = components.Size; index < j; ++index)
            {
                entity.AddComponent(components.Get(index));
            }
        }

        /// <summary>Sets the entity template.</summary>
        /// <param name="entityTag">The entity tag.</param>
        /// <param name="entityTemplate">The entity template.</param>
        public void SetEntityTemplate(string entityTag, IEntityTemplate entityTemplate)
        {
            this.entityTemplates.Add(entityTag, entityTemplate);
        }

        /// <summary>Sets the pool for a specific type</summary>
        /// <param name="type">The type.</param>
        /// <param name="pool">The pool.</param>
        public void SetPool(Type type, IComponentPool<ComponentPoolable> pool)
        {
            Debug.Assert(type != null, "Type must not be null.");
            Debug.Assert(pool != null, "Component pool must not be null.");

            this.pools.Add(type, pool);
        }

        /// <summary>Updates the EntityWorld</summary>
        /// <param name="elapsedTime">The elapsed TIME in milliseconds.</param>
        /// <param name="executionType">Type of the execution.</param>
        public void Update(float elapsedTime, ExecutionType executionType = ExecutionType.UpdateSynchronous)
        {
            this.ElapsedTime = elapsedTime;

            ++this.poolCleanupDelayCounter;
            if (this.poolCleanupDelayCounter > this.PoolCleanupDelay)
            {
                this.poolCleanupDelayCounter = 0;
                foreach (Type item in this.pools.Keys)
                {
                    this.pools[item].CleanUp();
                }
            }

            if (!this.deleted.IsEmpty)
            {
                for (int index = 0, j = this.deleted.Size; j > index; ++index)
                {
                    Entity e = this.deleted.Get(index);
                    this.EntityManager.Remove(e);
                    this.GroupManager.Remove(e);
                    e.DeletingState = false;
                }

                this.deleted.Clear();
            }

            if (!this.refreshed.IsEmpty)
            {
                for (int index = 0, j = this.refreshed.Size; j > index; ++index)
                {
                    Entity entity = this.refreshed.Get(index);
                    this.EntityManager.Refresh(entity);
                    entity.RefreshingState = false;
                }

                this.refreshed.Clear();
            }

            this.SystemManager.Update(executionType);
        }

        /// <summary>Refreshes the entity.</summary>
        /// <param name="entity">The entity.</param>
        internal void RefreshEntity(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            this.refreshed.Add(entity);
        }
    }
}