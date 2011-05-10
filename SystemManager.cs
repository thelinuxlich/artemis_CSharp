using System;
namespace Artemis
{
	public class SystemManager {
		private World world;
		private Dictionary<Object, EntitySystem> systems;
		private Bag<EntitySystem> bagged;
		
		public SystemManager(World world) {
			this.world = world;
			systems = new Dictionary<Object, EntitySystem>();
			bagged = new Bag<EntitySystem>();
		}
		
		public EntitySystem setSystem(EntitySystem system) {
			system.setWorld(world);
			
			systems.Add(system.GetType(), system);
			
			if(!bagged.contains(system))
				bagged.add(system);
			
			system.setSystemBit(SystemBitManager.getBitFor(system.GetType()));
			
			return system;
		}
		
		public T getSystem<T>(T type) {
			return systems.TryGetValue(type);
		}
		
		public Bag<EntitySystem> getSystems() {
			return bagged;
		}
		
		/**
		 * After adding all systems to the world, you must initialize them all.
		 */
		public void initializeAll() {
		   for (int i = 0; i < bagged.size(); i++) {
		      bagged.get(i).initialize();
		   }
		} 
	
	}
}