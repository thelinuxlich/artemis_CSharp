using System;
using System.Collections.Generic;
namespace Artemis
{
	public class ComponentTypeManager {
        private static Dictionary<Type, ComponentType> componentTypes = new Dictionary<Type, ComponentType>();
		
		public static ComponentType GetTypeFor<T>(T component) where T : Component
        { 
			ComponentType type = componentTypes[component.GetType()];
			if(type == null){ 
				type = new ComponentType();
                componentTypes.Add(component.GetType(), type);
			}			
			return type;
		}

        public static long GetBit<T>(T component) where T : Component
        {
            return GetTypeFor(component).GetBit();
		}
		
		public static int GetId<T>(T component) where T : Component
        {
			return GetTypeFor(component).GetId();
		}
	}
}

