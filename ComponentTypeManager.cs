using System;
using System.Collections.Generic;
namespace Artemis
{
	public class ComponentTypeManager {
        private static Dictionary<Type, ComponentType> componentTypes = new Dictionary<Type, ComponentType>();
		
		public static ComponentType getTypeFor<T>() where T : Component
        { 
			ComponentType type = componentTypes[typeof(T)];
			
			if(type == null){ 
				type = new ComponentType();
                componentTypes.Add(typeof(T), type);
			}			
			return type;
		}

        public static long getBit<T>() where T : Component
        {
            return getTypeFor<T>().getBit();
		}
		
		public static int getId<T>() where T : Component
        {
			return getTypeFor<T>().getId();
		}
	}
}

