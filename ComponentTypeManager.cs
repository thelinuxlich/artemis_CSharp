using System;
using System.Collections.Generic;
namespace Artemis
{
	public class ComponentTypeManager {
        private static Dictionary<Type, ComponentType> componentTypes = new Dictionary<Type, ComponentType>();
		
		public static ComponentType GetTypeFor(Type component)
        { 
			ComponentType type = componentTypes[component];
			if(type == null){ 
				type = new ComponentType();
                componentTypes.Add(component, type);
			}			
			return type;
		}

        public static long GetBit<T>(T component) where T : Component
        {
            return GetTypeFor(component.GetType()).GetBit();
		}
		
		public static int GetId<T>(T component) where T : Component
        {
			return GetTypeFor(component.GetType()).GetId();
		}
	}
}

