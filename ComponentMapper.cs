using System;
namespace Artemis
{
	public class ComponentMapper<T> where T : Component {
		private ComponentType type;
		private EntityManager em;
	
		public ComponentMapper(T type, EntityManager em) {
			this.em = em;
			this.type = ComponentTypeManager.getTypeFor(type);
		}
	
		public T get(Entity e) {
			return em.getComponent(e, type);
		}
	}
}

