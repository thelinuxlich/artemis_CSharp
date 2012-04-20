using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if WINDOWS
using System.Threading.Tasks;
#else
using ParallelTasks;
#endif
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

        private Dictionary<int, Bag<EntitySystem>> Updatelayers = new Dictionary<int, Bag<EntitySystem>>();
        private Dictionary<int, Bag<EntitySystem>> Drawlayers = new Dictionary<int, Bag<EntitySystem>>();
		private Bag<EntitySystem> mergedBag = new Bag<EntitySystem>();
		
		public SystemManager(EntityWorld world) {
			this.world = world;
		}
		
		public T SetSystem<T>(T system,ExecutionType execType , int layer = 0) where T : EntitySystem {
			system.SetWorld(world);
			
			systems.Add(typeof(T), (EntitySystem)system);
			
			if(execType == ExecutionType.Draw) {

                if (!Drawlayers.ContainsKey(layer))
                    Drawlayers[layer] = new Bag<EntitySystem>();

                Bag<EntitySystem> drawBag = Drawlayers[layer];
                if (drawBag == null)
                {
                    drawBag = Drawlayers[layer] = new Bag<EntitySystem>();
                }
				if(!drawBag.Contains((EntitySystem)system))
					drawBag.Add((EntitySystem)system);

			} else if(execType == ExecutionType.Update) {

                if (!Updatelayers.ContainsKey(layer))
                    Updatelayers[layer] = new Bag<EntitySystem>();                

                Bag<EntitySystem> updateBag = Updatelayers[layer];
                if (updateBag == null)
                {
                    updateBag = Updatelayers[layer] = new Bag<EntitySystem>();
                }
				if(!updateBag.Contains((EntitySystem)system))
					updateBag.Add((EntitySystem)system);
			}
			if(!mergedBag.Contains((EntitySystem)system))
					mergedBag.Add((EntitySystem)system);
			system.SystemBit = SystemBitManager.GetBitFor(system);
			
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


        void UpdatebagSync(Bag<EntitySystem> temp) 
        {
            for (int i = 0, j = temp.Size(); i < j; i++)
            {
                temp.Get(i).Process();
            }             
        }
		
		public void UpdateSynchronous(ExecutionType execType )
        {            
			if(execType == ExecutionType.Draw) {

                foreach (int item in Drawlayers.Keys)                
                {
                    UpdatebagSync(Drawlayers[item]);
                } 				
			} else if(execType == ExecutionType.Update) {
                foreach (int item in Updatelayers.Keys)
                {
                    UpdatebagSync(Updatelayers[item]);
                } 
			}	
        }
#if WINDOWS
        TaskFactory factory = new TaskFactory(TaskScheduler.Default);
        List<Task> tasks = new List<Task>();
#else
        List<Task> tasks = new List<Task>();
#endif

        void UpdatebagASSync(Bag<EntitySystem> temp)
        {
            tasks.Clear();
            for (int i = 0, j = temp.Size(); i < j; i++)
            {
                EntitySystem es = temp.Get(i);
                #if WINDOWS
                tasks.Add(factory.StartNew(
                #else
                tasks.Add(Parallel.Start(
                #endif

                    () =>
                    {
                        es.Process();
                    }
                ));

            }
#if WINDOWS
            Task.WaitAll(tasks.ToArray());
#else
            foreach (var item in tasks)
            {
                item.Wait();
            }
#endif

        }
        public void UpdateAsynchronous(ExecutionType execType )
        {
            if (execType == ExecutionType.Draw)
            {
                foreach (int item in Drawlayers.Keys)
                {
                    UpdatebagSync(Drawlayers[item]);
                }
            }
            else if (execType == ExecutionType.Update)
            {
                foreach (int item in Updatelayers.Keys)
                {
                    UpdatebagSync(Updatelayers[item]);
                }
            }	
            
        }
		
	}
}