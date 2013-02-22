namespace Artemis.Manager
{
    #region Using statements

    using Artemis.Interface;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics;
#if !XBOX && !WINDOWS_PHONE
    using global::System.Numerics;
#endif
#if XBOX || WINDOWS_PHONE
    using BigInteger = global::System.Int32;
#endif

    #endregion Using statements

    /// <summary>Class ComponentTypeManager.</summary>
    public static class ComponentTypeManager
    {
        /// <summary>The component types.</summary>
        private static readonly Dictionary<Type, ComponentType> ComponentTypes = new Dictionary<Type, ComponentType>();

        /// <summary>Gets the bit.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>BigInteger.</returns>
        public static BigInteger GetBit<T>() where T : IComponent
        {
            return GetTypeFor<T>().Bit;
        }

        /// <summary>Gets the id.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>System.Int32.</returns>
        public static int GetId<T>() where T : IComponent
        {
            return GetTypeFor<T>().Id;
        }

        /// <summary>Get the component type for the given component instance.</summary>
        /// <typeparam name="T">Component for which you want the component type.</typeparam>
        /// <returns>Component Type.</returns>
        public static ComponentType GetTypeFor<T>() where T : IComponent
        {
            ComponentType result;
            Type receivedType = typeof(T);
            if (!ComponentTypes.TryGetValue(receivedType, out result))
            {
                result = new ComponentType();
                ComponentTypes.Add(receivedType, result);
            }

            return result;
        }

        /// <summary><para>Ensure the given component type [tag] is an "official"</para>
        ///   <para>component type for your solution. If it does not already</para>
        ///   <para>exist, add it to the bag of available component types.</para>
        ///   <para>This is a way you can easily add "official" component</para>
        ///   <para>types to your solution.</para></summary>
        /// <param name="component">The component type label you want to ensure is an "official" component type</param>
        /// <returns>ComponentType</returns>
        public static ComponentType GetTypeFor(Type component)
        {
            Debug.Assert(component != null, "Component must not be null.");

            ComponentType result;
            if (!ComponentTypes.TryGetValue(component, out result))
            {
                result = new ComponentType();
                ComponentTypes.Add(component, result);
            }

            return result;
        }
    }
}