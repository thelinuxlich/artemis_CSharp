using System;
using System.Collections.Generic;
namespace Artemis
{
    /// <summary>
    /// Entity World Class
    /// Main interface of the Entity System
    /// </summary>
	public sealed class EntityWorld {
		private SystemManager systemManager;
		private EntityManager entityManager;
		private TagManager tagManager;
		private GroupManager groupManager;
        private Bag<Entity> refreshed = new Bag<Entity>();
        private Bag<Entity> deleted = new Bag<Entity>();        
		private Dictionary<String,Stack<int>> cached = new Dictionary<String, Stack<int>>();
        private Dictionary<String, IEntityTemplate> entityTemplates = new Dictionary<String, IEntityTemplate>();
		private float elapsedTime;
        private Dictionary<Type, IComponentPool<ComponentPoolable>> pools = new Dictionary<Type, IComponentPool<ComponentPoolable>>();        
        private int poolCleanupDelayCounter = 0;

        /// <summary>
        /// Interval in FrameUpdates between pools cleanup
        /// Default 10
        /// </summary>
        public int PoolCleanupDelay
        {
            get;
            set;
        }

		public EntityWorld() {
			entityManager = new EntityManager(this);
			systemManager = new SystemManager(this);
			tagManager = new TagManager(this);
			groupManager = new GroupManager(this);
            PoolCleanupDelay = 10;
		}

        /// <summary>
        /// Gets the group manager.
        /// </summary>
        /// <value>
        /// The group manager.
        /// </value>
		public GroupManager GroupManager {
			get { return groupManager; }
		}

        /// <summary>
        /// Gets the system manager.
        /// </summary>
        /// <value>
        /// The system manager.
        /// </value>
		public SystemManager SystemManager {
			get { return systemManager; }
		}

        /// <summary>
        /// Gets the entity manager.
        /// </summary>
        /// <value>
        /// The entity manager.
        /// </value>
		public EntityManager EntityManager {
			get { return entityManager; }
		}

        /// <summary>
        /// Gets the tag manager.
        /// </summary>
        /// <value>
        /// The tag manager.
        /// </value>
		public TagManager TagManager {
			get { return tagManager; }
		}
		
		/**
		 * Time since last game loop.
		 * @return delta in milliseconds.
		 */
		public float ElapsedTime {
			get { return elapsedTime; }			
		}
		
		/**
		 * Delete the provided entity from the world.
		 * @param e entity
		 */
		public void DeleteEntity(Entity e) {
            System.Diagnostics.Debug.Assert(e != null);
	        deleted.Add(e);
    	}
		
		/**
		 * Ensure all systems are notified of changes to this entity.
		 * @param e entity
		 */
		internal void RefreshEntity(Entity e) {
            System.Diagnostics.Debug.Assert(e != null);
			refreshed.Add(e);
		}

       	
		/**
		 * Create and return a new or reused entity instance.
		 * @return entity
		 */
		public Entity CreateEntity() {
			return entityManager.Create();
		}

        /// <summary>
        /// Sets the pool for a specific type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="pool">The pool.</param>
        public void SetPool(Type type, IComponentPool<ComponentPoolable> pool)
        {
            System.Diagnostics.Debug.Assert(type != null);
            System.Diagnostics.Debug.Assert(pool != null);
            pools.Add(type, pool);
        }

        /// <summary>
        /// Gets the pool for a Type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IComponentPool<ComponentPoolable> GetPool(Type type)
        {
            System.Diagnostics.Debug.Assert(type != null);
            return pools[type];
        }

        /// <summary>
        /// Initialize the World
        /// </summary>
        /// <param name="processAttributes"></param>
        public void InitializeAll(
#if FULLDOTNET
            bool processAttributes = true
#else
            bool processAttributes = false
#endif
            )
        {
            systemManager.InitializeAll(processAttributes);
        }

        /// <summary>
        /// Gets a component from a pool.
        /// </summary>
        /// <param name="type">The typeof the object to get</param>
        /// <returns></returns>
        public Component GetComponentFromPool(Type type)
        {
            System.Diagnostics.Debug.Assert(type != null);
            if (!pools.ContainsKey(type))
                throw new Exception("There is no pool for the type " + type);

            return pools[type].New();
        }

        /// <summary>
        /// Gets the component from pool.
        /// </summary>
        /// <typeparam name="T">Type of the component</typeparam>
        /// <returns></returns>
        public Component GetComponentFromPool<T>() where T : ComponentPoolable
        {
            var type = typeof(T);
            if (!pools.ContainsKey(type))
                throw new Exception("There is no pool for the type " + type);

            return pools[type].New();
        }


        /// <summary>
        /// Creates a entity from template.
        /// </summary>
        /// <param name="entityTemplateTag">The entity template tag.</param>
        /// <param name="templateArgs">The template args.</param>
        /// <returns></returns>
		public Entity CreateEntityFromTemplate(string entityTemplateTag, params object[] templateArgs) {
            System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(entityTemplateTag));
			Entity e = entityManager.Create();  
            IEntityTemplate entityTemplate;
            entityTemplates.TryGetValue(entityTemplateTag, out entityTemplate);
            if (entityTemplate == null)
                throw new Exception("EntityTemplate for the tag " + entityTemplateTag + " was not registered");
            return entityTemplate.BuildEntity(e, this, templateArgs);       
		}

        public void SetEntityTemplate(string entityTag, IEntityTemplate entityTemplate)
        {
            entityTemplates.Add(entityTag, entityTemplate);
        }
		
		/**
		 * Get a entity having the specified id.
		 * @param entityId
		 * @return entity
		 */
		public Entity GetEntity(int entityId) {
            System.Diagnostics.Debug.Assert(entityId >= 0);
			return entityManager.GetEntity(entityId);
		}

        /// <summary>
        /// Updates the World
        /// </summary>
        /// <param name="executionType">Type of the execution.</param>
        /// <param name="elapsedTime">The elapsed TIME in milliseconds.</param>
        public void Update(float elapsedTime ,ExecutionType executionType = ExecutionType.UpdateSynchronous)
        {
            this.elapsedTime = elapsedTime;

            poolCleanupDelayCounter++;
            if (poolCleanupDelayCounter > PoolCleanupDelay)
            {
                poolCleanupDelayCounter = 0;
                foreach (var item in pools.Keys)
                {
                    pools[item].CleanUp();
                }
            }

            if (!deleted.IsEmpty)
            {
                for (int i = 0, j = deleted.Size; j > i; i++)
                {
                    Entity e = deleted.Get(i);
                    entityManager.Remove(e);
                    groupManager.Remove(e);
                    e.DeletingState = false;
                }
                deleted.Clear();
            }

            if (!refreshed.IsEmpty)
            {
                for (int i = 0, j = refreshed.Size; j > i; i++)
                {
					Entity e = refreshed.Get(i);
                    entityManager.Refresh(e);
					e.RefreshingState = false;
                }
                refreshed.Clear();
            }

            systemManager.Update(executionType);

        }

        /// <summary>
        /// Gets the current state of the workd.
        /// </summary>
        /// <value>
        /// The state of the current.
        /// </value>
		public Dictionary<Entity,Bag<Component>> CurrentState {
            get
            {
                Bag<Entity> entities = entityManager.ActiveEntities;
                Dictionary<Entity, Bag<Component>> currentState = new Dictionary<Entity, Bag<Component>>();
                for (int i = 0, j = entities.Size; i < j; i++)
                {
                    Entity e = entities.Get(i);
                    Bag<Component> components = e.Components;
                    currentState.Add(e, components);
                }
                return currentState;
            }
		}

	    /// <summary>
	    /// Loads the state of the entity.
	    /// </summary>
	    /// <param name="templateTag">The template tag. Can be null</param>
	    /// <param name="groupName">Name of the group. Can be null</param>
	    /// <param name="components">The components.</param>
	    /// <param name="templateArgs">Params for entity template</param>
	    public void LoadEntityState(String templateTag, String groupName,Bag<Component> components, params object[] templateArgs) {
            System.Diagnostics.Debug.Assert(components != null);
			Entity e;
			if(!String.IsNullOrEmpty(templateTag)) {
				e = CreateEntityFromTemplate(templateTag, templateArgs);
			} else {
				e = CreateEntity();
			}
            if (String.IsNullOrEmpty(groupName))
            {
				groupManager.Set(groupName,e);
			}		
			for(int i = 0, j = components.Size; i < j; i++) {
				e.AddComponent(components.Get(i));
			}
		}
	}
}