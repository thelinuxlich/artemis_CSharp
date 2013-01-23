using System;
namespace Artemis
{
    /// <summary>
    /// Fastest Way to get Components from entities
    /// </summary>
	public sealed class ComponentMapper<T> where T : Component {
		private ComponentType type;
		private EntityManager em;
   
        /// <summary>
        /// Creates a component mapper within the given Entity World
        /// </summary>
        /// <param name="world">EntityWorld</param>
		public ComponentMapper(EntityWorld world) {
            System.Diagnostics.Debug.Assert(world != null);
			em = world.EntityManager;
			type = ComponentTypeManager.GetTypeFor<T>();
		}     
	
        /// <summary>
        /// Gets the component for the given entity/component type combo
        /// </summary>
        /// <param name="e">Entity in which you are interested</param>
        /// <returns>Component</returns>
		public T Get(Entity e) {
            System.Diagnostics.Debug.Assert(e != null);
			return (T)em.GetComponent(e, type);
		}
        
        /// <summary>
        /// Creates a ComponentMapper for a Type
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="type"></param>
        /// <param name="world"></param>
        /// <returns></returns>
          public static ComponentMapper<K> GetComponentMapperFor<K>(K type, EntityWorld world)
           where K : Component
          {
                return new ComponentMapper<K>(world);
        }


	}
}

