using System;
namespace Artemis
{
	public class ComponentMapper<Component> {
		private ComponentType type;
		private EntityManager em;
	
		public ComponentMapper(Component type, EntityManager em) {
			this.em = em;
			this.type = ComponentTypeManager.getTypeFor(type);
		}
	
		public Object get(Entity e) {
			return em.getComponent(e, type);
		}
	}
}

