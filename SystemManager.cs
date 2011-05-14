using System;
using System.Collections.Generic;
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
		
		public EntitySystem SetSystem(EntitySystem system) {
			system.SetWorld(world);
			
			systems.Add(system.GetType(), system);
			
			if(!bagged.Contains(system))
				bagged.Add(system);
			
			system.SetSystemBit(SystemBitManager.GetBitFor(system));
			
			return system;
		}
		
		public T GetSystem<T>() where T : EntitySystem {
			return (T)systems[typeof(T)];
		}
		
		public Bag<EntitySystem> GetSystems() {
			return bagged;
		}
		
		/**
		 * After adding all systems to the world, you must initialize them all.
		 */
		public void InitializeAll() {
		   for (int i = 0; i < bagged.Size(); i++) {
		      bagged.Get(i).Initialize();
		   }
		} 
	
	}
}