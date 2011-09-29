using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Artemis
{
	public enum ExecutionType
    {
        Draw,
        Update
    }
	
	public sealed class SystemManager {
		private EntityWorld world;
		private Dictionary<Type, EntitySystem> systems = new Dictionary<Type, EntitySystem>();
		private Bag<EntitySystem> drawBag = new Bag<EntitySystem>();
		private Bag<EntitySystem> updateBag = new Bag<EntitySystem>();
		private Bag<EntitySystem> mergedBag = new Bag<EntitySystem>();
		
		public SystemManager(EntityWorld world) {
			this.world = world;
		}
		
		public T SetSystem<T>(T system,ExecutionType execType = ExecutionType.Update) where T : EntitySystem {
			system.SetWorld(world);
			
			systems.Add(typeof(T), (EntitySystem)system);
			
			if(execType == ExecutionType.Draw) {
				if(!drawBag.Contains((EntitySystem)system))
					drawBag.Add((EntitySystem)system);
			} else if(execType == ExecutionType.Update) {	
				if(!updateBag.Contains((EntitySystem)system))
					updateBag.Add((EntitySystem)system);
			}
			if(!mergedBag.Contains((EntitySystem)system))
					mergedBag.Add((EntitySystem)system);
			system.SetSystemBit(SystemBitManager.GetBitFor(system));
			
			return (T)system;
		}
		
		public T GetSystem<T>() where T : EntitySystem {
            EntitySystem system;
            systems.TryGetValue(typeof(T), out system);
			return (T)system;
		}
		
		public Bag<EntitySystem> GetSystems() {
			return mergedBag;
		}
		
		/**
		 * After adding all systems to the world, you must initialize them all.
		 */
		public void InitializeAll() {
		   for (int i = 0, j = mergedBag.Size(); i < j; i++) {
		      mergedBag.Get(i).Initialize();
		   }
		} 
		
		public void UpdateSynchronous(ExecutionType execType = ExecutionType.Update)
        {
			Bag<EntitySystem> temp = new Bag<EntitySystem>();
			if(execType == ExecutionType.Draw) {
				temp = drawBag;
			} else if(execType == ExecutionType.Update) {
				temp = updateBag;
			}	
            for (int i = 0, j = temp.Size(); i < j; i++) 
			{
                temp.Get(i).Process();
			}             
        }

        TaskFactory factory = new TaskFactory(TaskScheduler.Default);
        List<Task> tasks = new List<Task>();
        public void UpdateAsynchronous(ExecutionType execType = ExecutionType.Update)
        {
			Bag<EntitySystem> temp = new Bag<EntitySystem>();
			if(execType == ExecutionType.Draw) {
				temp = drawBag;
			} else if(execType == ExecutionType.Update) {
				temp = updateBag;
			}	
            for (int i = 0, j = temp.Size(); i < j; i++)
            {
                tasks.Add(factory.StartNew(
                    () =>
                    {
                        temp.Get(i).Process();
                    }
                ));
                
            }             
            Task.WaitAll(tasks.ToArray());
        }
		
	}
}