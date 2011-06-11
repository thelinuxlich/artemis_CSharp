using System;
namespace Artemis
{
	public sealed class ComponentMapper {
		private ComponentType type;
		private EntityManager em;

        public ComponentMapper() { }

		public ComponentMapper(Type type, EntityManager em) {
			this.em = em;
			this.type = ComponentTypeManager.GetTypeFor(type);
		}

        public void SetType(Type type)
        {
            this.type = ComponentTypeManager.GetTypeFor(type);
        }

        public void SetEntityManager(EntityManager em)
        {
            this.em = em;
        }
	
		public T Get<T>(Entity e) where T : Component {
			return (T)em.GetComponent(e, type);
		}
	}
}

