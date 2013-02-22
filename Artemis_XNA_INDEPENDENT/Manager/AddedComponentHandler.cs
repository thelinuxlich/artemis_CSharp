namespace Artemis.Manager
{
    #region Using statements

    using Artemis.Interface;

    #endregion Using statements

    /// <summary>Delegate AddedComponentHandler.</summary>
    /// <param name="entity">The entity.</param>
    /// <param name="component">The component.</param>
    public delegate void AddedComponentHandler(Entity entity, IComponent component);
}