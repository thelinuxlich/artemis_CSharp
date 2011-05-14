using System;
namespace Artemis
{
	public class ComponentMapper {
		private ComponentType type;
		private EntityManager em;
	
		public ComponentMapper(Type type, EntityManager em) {
			this.em = em;
			this.type = ComponentTypeManager.GetTypeFor(type);
		}
	
		public T Get<T>(Entity e) where T : Component {
			return (T)em.GetComponent(e, type);
		}
	}
}

