using System;
namespace Artemis
{
	public class ComponentMapper<T> where T : Component {
		private ComponentType type;
		private EntityManager em;
	
		public ComponentMapper(T type, EntityManager em) {
			this.em = em;
			this.type = ComponentTypeManager.GetTypeFor(type);
		}
	
		public T Get(Entity e) {
			return (T)em.GetComponent(e, type);
		}
	}
}

