using System;
using System.Collections.Generic;
#if !XBOX && !WINDOWS_PHONE
using System.Numerics;
#endif

#if XBOX || WINDOWS_PHONE
using BigInteger = System.Int32;
#endif

namespace Artemis
{
	public static class ComponentTypeManager {
        private static Dictionary<Type, ComponentType> componentTypes = new Dictionary<Type, ComponentType>();

        /// <summary>
        /// Get the component type for the given component instance
        /// </summary>
        /// <typeparam name="T">Component for which you want the component type</typeparam>
        /// <returns>Component Type</returns>
		public static ComponentType GetTypeFor<T>() where T : Component
        { 
			ComponentType type = null;
			Type receivedType = typeof(T);
			if(!componentTypes.TryGetValue(receivedType,out type)){ 
				type = new ComponentType();
                componentTypes.Add(receivedType, type);
			}			
			return type;
		}

        /// <summary>
        /// Ensure the given component type [tag] is an "official" component type for your solution
        /// If it does not already exist, add it to the bag of available component types
        /// This is a way you can easily add "official" component types to your solution
        /// </summary>
        /// <param name="component">The component type label you want to ensure is an "official" component type</param>
        /// <returns>ComponentType</returns>
		public static ComponentType GetTypeFor(Type component)
        {
            System.Diagnostics.Debug.Assert(component != null);
			ComponentType type = null;
			if(!componentTypes.TryGetValue(component,out type)){ 
				type = new ComponentType();
                componentTypes.Add(component, type);
			}			
			return type;
		}
		
        public static BigInteger GetBit<T>() where T : Component
        {
            return GetTypeFor<T>().Bit;
		}
		
		public static int GetId<T>() where T : Component
        {
			return GetTypeFor<T>().Id;
		}
	}
}

