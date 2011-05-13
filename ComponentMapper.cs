using System;
namespace Artemis
{
	public class ComponentMapper<T> where T : Component {
		private ComponentType type;
		private EntityManager em;
	
		public ComponentMapper(T type, EntityManager em) {
			this.em = em;
			this.type = ComponentTypeManager.GetTypeFor<T>();
		}
	
		public T Get(Entity e) {
			return em.GetComponent(e, type);
		}
	}
}

