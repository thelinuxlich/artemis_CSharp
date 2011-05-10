using System;
namespace Artemis
{
	public class ComponentTypeManager {
		private static Dictionary<Type, ComponentType> componentTypes = new Dictionary<Type, ComponentType>();
		
		public static ComponentType getTypeFor(Component c){
			ComponentType type = componentTypes.TryGetValue(typeof(c));
			
			if(type == null){ 
				type = new ComponentType();
				componentTypes.Add(typeof(c), type);
			}
			
			return type;
		}
	
		public static long getBit(Component c){
			return getTypeFor(c).getBit();
		}
		
		public static int getId(Component c){
			return getTypeFor(c).getId();
		}
	}
}

