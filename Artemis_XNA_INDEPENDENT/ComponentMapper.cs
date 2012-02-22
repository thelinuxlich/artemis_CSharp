using System;
namespace Artemis
{
	public sealed class ComponentMapper<T> where T : Component {
		private ComponentType type;
		private EntityManager em;

        /// <summary>
        /// Allows you to interact with components that are mapped to entities
        /// </summary>
        public ComponentMapper() { }

        /// <summary>
        /// Creates a component mapper within the given Entity World
        /// </summary>
        /// <param name="world">EntityWorld</param>
		public ComponentMapper(EntityWorld world) {
			this.em = world.GetEntityManager();
			this.type = ComponentTypeManager.GetTypeFor<T>();
		}

        /// <summary>
        /// Sets the entity manager for this component mapper
        /// </summary>
        /// <param name="em">Entity Manager that manages the component</param>
        public void SetEntityManager(EntityManager em)
        {
            this.em = em;
        }
	
        /// <summary>
        /// Gets the component for the given entity/component type combo
        /// </summary>
        /// <param name="e">Entity in which you are interested</param>
        /// <returns>Component</returns>
		public T Get(Entity e) {
			return (T)em.GetComponent(e, type);
		}
	}
}

