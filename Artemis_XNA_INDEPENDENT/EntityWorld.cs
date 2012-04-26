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
		private Dictionary<String,Stack<int>> cached = new Dictionary<String, Stack<int>>();

		private int delta;
		
		public EntityWorld() {
			entityManager = new EntityManager(this);
			systemManager = new SystemManager(this);
			tagManager = new TagManager(this);
			groupManager = new GroupManager(this);		
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
		public int Delta {
			get { return delta; }
			set { delta = value; }
		}
		
		/**
		 * Delete the provided entity from the world.
		 * @param e entity
		 */
		public void DeleteEntity(Entity e) {
	        deleted.Add(e);
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

        public ArtemisPool Pool
        {
			get { return pool; }
            set { pool = value; }
        }

        public void LoopStart()
        {
            if (!deleted.IsEmpty)
            {
                for (int i = 0, j = deleted.Size(); j > i; i++)
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
                for (int i = 0, j = refreshed.Size(); j > i; i++)
                {
					Entity e = refreshed.Get(i);
                    entityManager.Refresh(e);
					e.RefreshingState = false;
                }
                refreshed.Clear();
            }
        }
		
		public Dictionary<Entity,Bag<Component>> GetCurrentState() {
			Bag<Entity> entities = entityManager.ActiveEntities;
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
				e = CreateEntity(tag);
			} else {
				e = CreateEntity();
			}
			if(groupName != null) {
				groupManager.Set(groupName,e);
			}		
			for(int i = 0, j = components.Size(); i < j; i++) {
				e.AddComponent(components.Get(i));
			}
		}
	}
}