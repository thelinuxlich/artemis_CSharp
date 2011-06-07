using System;
namespace Artemis
{
	public class World {
		private SystemManager systemManager;
		private EntityManager entityManager;
		private TagManager tagManager;
		private GroupManager groupManager;
		
		private int delta;
		
		public World() {
			entityManager = new EntityManager(this);
			systemManager = new SystemManager(this);
			tagManager = new TagManager(this);
			groupManager = new GroupManager(this);		
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
			entityManager.Remove(e);
		}
		
		/**
		 * Ensure all systems are notified of changes to this entity.
		 * @param e entity
		 */
		public void RefreshEntity(Entity e) {
			entityManager.Refresh(e);
		}
		
		/**
		 * Create and return a new or reused entity instance.
		 * @return entity
		 */
		public Entity CreateEntity() {
			return entityManager.Create();
		}
		
		/**
		 * Get a entity having the specified id.
		 * @param entityId
		 * @return entity
		 */
		public Entity GetEntity(int entityId) {
			return entityManager.GetEntity(entityId);
		}
	}
}