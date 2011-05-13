using System;
using System.Collections.Generic;
namespace Artemis
{
	public class ComponentTypeManager {
        private static Dictionary<Type, ComponentType> componentTypes = new Dictionary<Type, ComponentType>();
		
		public static ComponentType GetTypeFor<T>() where T : Component
        { 
			ComponentType type = componentTypes[typeof(T)];
			
			if(type == null){ 
				type = new ComponentType();
                componentTypes.Add(typeof(T), type);
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

