namespace Artemis.Interface
{
    /// <summary>Interface IComponentPool.</summary>
    /// <typeparam name="T">The <see langword="Type"/> T.</typeparam>
    public interface IComponentPool<T>
        where T : ComponentPoolable
    {
        /// <summary>Cleans up.</summary>
        void CleanUp();

        /// <summary>Get a new pool item.</summary>
        /// <returns>A new pool item.</returns>
        T New();

        /// <summary>Returns the object.</summary>
        /// <param name="item">The item.</param>
        void ReturnObject(T item);
    }
}