namespace Artemis
{
    #region Using statements

    using global::System.Diagnostics;

    using Artemis.Interface;
    using Artemis.Manager;

    #endregion Using statements

    /// <summary>Fastest Way to get Components from entities.</summary>
    /// <typeparam name="T">The <see langword="Type"/> T.</typeparam>
    public sealed class ComponentMapper<T>
        where T : IComponent
    {
        /// <summary>The entity manager.</summary>
        private readonly EntityManager entityManager;

        /// <summary>The component type.</summary>
        private readonly ComponentType componentType;

        /// <summary>Initializes a new instance of the <see cref="ComponentMapper{T}"/> class.</summary>
        /// <param name="entityWorld">The entity world.</param>
        public ComponentMapper(EntityWorld entityWorld)
        {
            Debug.Assert(entityWorld != null, "Entity world must not be null.");

            this.entityManager = entityWorld.EntityManager;
            this.componentType = ComponentTypeManager.GetTypeFor<T>();
        }

        /// <summary>Gets the component mapper for.</summary>
        /// <typeparam name="TK">The <see langword="Type"/> TK.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="entityWorld">The entity world.</param>
        /// <returns>The specified ComponentMapper.</returns>
        public static ComponentMapper<TK> GetComponentMapperFor<TK>(TK type, EntityWorld entityWorld) where TK : IComponent
        {
            return new ComponentMapper<TK>(entityWorld);
        }

        /// <summary>Gets the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The specified <see langword="Type"/> T.</returns>
        public T Get(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            return (T)this.entityManager.GetComponent(entity, this.componentType);
        }
    }
}