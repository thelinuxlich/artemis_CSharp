using System;
using System.Collections.Generic;
namespace Artemis
{
	public sealed class SystemManager {
		private World world;
		private Dictionary<Type, EntitySystem> systems = new Dictionary<Type, EntitySystem>();
		private Bag<EntitySystem> bagged = new Bag<EntitySystem>();
		
		public SystemManager(World world) {
			this.world = world;
		}
		
		public T SetSystem<T>(T system) where T : EntitySystem {
			system.SetWorld(world);
			
			systems.Add(typeof(T), (EntitySystem)system);
			
			if(!bagged.Contains((EntitySystem)system))
				bagged.Add((EntitySystem)system);
			
			system.SetSystemBit(SystemBitManager.GetBitFor(system));
			
			return (T)system;
		}
		
		public T GetSystem<T>() where T : EntitySystem {
            EntitySystem system;
            bool hasSystem = systems.TryGetValue(typeof(T), out system);
			return (T)system;
		}
		
		public Bag<EntitySystem> GetSystems() {
			return bagged;
		}
		
		/**
		 * After adding all systems to the world, you must initialize them all.
		 */
		public void InitializeAll() {
		   for (int i = 0, j = bagged.Size(); i < j; i++) {
		      bagged.Get(i).Initialize();
		   }
		} 
	
	}
}