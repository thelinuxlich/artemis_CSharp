using System;
using System.Collections.Generic;
namespace Artemis
{
	public sealed class EntityWorld {
		private SystemManager systemManager;
		private EntityManager entityManager;
		private TagManager tagManager;
		private GroupManager groupManager;
        private Bag<Entity> refreshed = new Bag<Entity>();
        private Bag<Entity> deleted = new Bag<Entity>();
        private ArtemisPool pool;
		private Dictionary<Type,Manager> managers = new Dictionary<Type, Manager>();

		private int delta;
		
		public EntityWorld() {
			entityManager = new EntityManager(this);
			systemManager = new SystemManager(this);
			tagManager = new TagManager(this);
			groupManager = new GroupManager(this);		
		}
		public void SetManager(Manager manager) {
    			managers.Add(manager.GetType(), manager);
  		}
		
  		public T GetManager<T>() where T : Manager {
			Manager m; 
			managers.TryGetValue(typeof(T), out m);
    			return (T)m;
  		}
		
		public GroupManager GetGroupManager() {
			return groupManager;
		}
		
		public SystemManager GetSystemManager() {
			return systemManager;
		}
		
		public EntityManager GetEntityManager() {
			return entityManager;
		}
		
		public TagManager GetTagManager() {
			return tagManager;
		}
		
		/**
		 * Time since last game loop.
		 * @return delta in milliseconds.
		 */
		public int GetDelta() {
			return delta;
		}
		
		/**
		 * You must specify the delta for the game here.
		 * 
		 * @param delta time since last game loop.
		 */
		public void SetDelta(int delta) {
			this.delta = delta;
		}
	
		/**
		 * Delete the provided entity from the world.
		 * @param e entity
		 */
		public void DeleteEntity(Entity e) {
			groupManager.Remove(e);
            if (!deleted.Contains(e))
            {
                deleted.Add(e);
            }
		}
		
		/**
		 * Ensure all systems are notified of changes to this entity.
		 * @param e entity
		 */
		public void RefreshEntity(Entity e) {
			refreshed.Add(e);
		}
		
		/**
		 * Create and return a new or reused entity instance.
		 * @return entity
		 */
		public Entity CreateEntity() {
			return entityManager.Create();
		}
		
		public Entity CreateEntity(string tag) {
			Entity e = entityManager.Create();
			tagManager.Register(tag,e);
			return e;
		}
		
		/**
		 * Get a entity having the specified id.
		 * @param entityId
		 * @return entity
		 */
		public Entity GetEntity(int entityId) {
			return entityManager.GetEntity(entityId);
		}

        public void SetPool(ArtemisPool gamePool)
        {
            pool = gamePool;
        }

        public ArtemisPool GetPool()
        {
            return pool;
        }

        public void LoopStart()
        {
            if (!refreshed.IsEmpty())
            {
                for (int i = 0, j = refreshed.Size(); j > i; i++)
                {
                    entityManager.Refresh(refreshed.Get(i));
                }
                refreshed.Clear();
            }

            if (!deleted.IsEmpty())
            {
                for (int i = 0, j = deleted.Size(); j > i; i++)
                {
                    Entity e = deleted.Get(i);
                    entityManager.Remove(e);
                }
                deleted.Clear();
            }
        }
		
		public Dictionary<Entity,Bag<Component>> GetCurrentState() {
			Bag<Entity> entities = entityManager.GetActiveEntities();
			Dictionary<Entity,Bag<Component>> currentState = new Dictionary<Entity, Bag<Component>>();
			for(int i = 0,j = entities.Size(); i < j; i++) {
				Entity e = entities.Get(i);
				Bag<Component> components = e.GetComponents();
				currentState.Add(e, components);
			}
			return currentState;
		}
		
		public void LoadEntityState(String tag,String groupName,Bag<Component> components) {
			Entity e;
			if(tag != null) {
				e = this.CreateEntity(tag);
			} else {
				e = this.CreateEntity();
			}
			if(groupName != null) {
				this.groupManager.Set(groupName,e);
			}		
			for(int i = 0, j = components.Size(); i < j; i++) {
				e.AddComponent(components.Get(i));
			}
		}
	}
}