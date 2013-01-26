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
        private Dictionary<Type, ComponentPool<ComponentPoolable>> pools = new Dictionary<Type, ComponentPool<ComponentPoolable>>();        
        private int poolCleanupDelayCounter = 0;

        /// <summary>
        /// Interval in FrameUpdates between pools cleanup
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
		
		public GroupManager GroupManager {
			get { return groupManager; }
		}
		
		public SystemManager SystemManager {
			get { return systemManager; }
		}
		
		public EntityManager EntityManager {
			get { return entityManager; }
		}
		
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

        public void SetPool(Type type, ComponentPool<ComponentPoolable> pool)
        {
            System.Diagnostics.Debug.Assert(type != null);
            System.Diagnostics.Debug.Assert(pool != null);
            pools.Add(type, pool);
        }

        public ComponentPool<ComponentPoolable> GetPool(Type type)
        {
            System.Diagnostics.Debug.Assert(type != null);
            return pools[type];
        }

        /// <summary>
        /// 
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

        public Component GetComponentFromPool(Type type)
        {
            System.Diagnostics.Debug.Assert(type != null);
            if (!pools.ContainsKey(type))
                throw new Exception("There is no pool for the type " + type);

            return pools[type].New();
        }
        		
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
        public void Update(float elapsedTime ,ExecutionType executionType = ExecutionType.UpdateSyncronous)
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