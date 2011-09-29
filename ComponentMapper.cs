using System;
namespace Artemis
{
	public sealed class ComponentMapper<T> where T : Component {
		private ComponentType type;
		private EntityManager em;

        public ComponentMapper() { }

		public ComponentMapper(EntityWorld world) {
			this.em = world.GetEntityManager();
			this.type = ComponentTypeManager.GetTypeFor<T>();
		}

        public void SetEntityManager(EntityManager em)
        {
            this.em = em;
        }
	
		public T Get(Entity e) {
			return (T)em.GetComponent(e, type);
		}
	}
}

