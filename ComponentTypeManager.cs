using System;
using System.Collections.Generic;
namespace Artemis
{
	public static class ComponentTypeManager {
        private static Dictionary<Type, ComponentType> componentTypes = new Dictionary<Type, ComponentType>();
		
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
		
		public static ComponentType GetTypeFor(Type component)
        { 
			ComponentType type = null;
			if(!componentTypes.TryGetValue(component,out type)){ 
				type = new ComponentType();
                componentTypes.Add(component, type);
			}			
			return type;
		}
		
        public static long GetBit<T>() where T : Component
        {
            return GetTypeFor<T>().GetBit();
		}
		
		public static int GetId<T>() where T : Component
        {
			return GetTypeFor<T>().GetId();
		}
	}
}

