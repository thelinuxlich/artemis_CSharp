namespace Artemis.Interface
{
    /// <summary>Interface IEntityTemplate.</summary>
    public interface IEntityTemplate
    {
        /// <summary>Builds the entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <param name="entityWorld">The entityWorld.</param>
        /// <param name="args">The args.</param>
        /// <returns>The build entity.</returns>
        Entity BuildEntity(Entity entity, EntityWorld entityWorld, params object[] args);
    }
}