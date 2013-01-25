using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if FULLDOTNET
using System.Threading.Tasks;
using Artemis.Attributes;
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
        private Dictionary<Type, List<EntitySystem>> systems = new Dictionary<Type, List<EntitySystem>>();

        private Dictionary<int, Bag<EntitySystem>> Updatelayers = new Dictionary<int, Bag<EntitySystem>>();
        private Dictionary<int, Bag<EntitySystem>> Drawlayers = new Dictionary<int, Bag<EntitySystem>>();
		private Bag<EntitySystem> mergedBag = new Bag<EntitySystem>();
		
		internal SystemManager(EntityWorld world) {
			this.world = world;
		}
		
		public T SetSystem<T>(T system,ExecutionType execType , int layer = 0) where T : EntitySystem {
			system.World = world;
						
            if(systems.ContainsKey(typeof(T)))
            {
                systems[typeof(T)].Add((EntitySystem)system);
            }
            else
            {
                systems[typeof(T)] = new List<EntitySystem>();
                systems[typeof(T)].Add((EntitySystem)system);
            }

			
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
				Drawlayers = (from d in Drawlayers orderby d.Key ascending select d).ToDictionary(pair => pair.Key, pair => pair.Value);
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
				Updatelayers = (from d in Updatelayers orderby d.Key ascending select d).ToDictionary(pair => pair.Key, pair => pair.Value);
			}
			if(!mergedBag.Contains((EntitySystem)system))
					mergedBag.Add((EntitySystem)system);
			system.SystemBit = SystemBitManager.GetBitFor(system);
			
			return (T)system;
		}
		
		public List<EntitySystem> GetSystem<T>() where T : EntitySystem {
            List<EntitySystem> system;

            systems.TryGetValue(typeof(T), out system);

            return system;
		}
		
		public Bag<EntitySystem> Systems {
			get { return mergedBag;}
		}

        private static ComponentPoolable CreateInstance(Type type) 
        {
            return (ComponentPoolable) Activator.CreateInstance(type);
        }


		/**
		 * After adding all systems to the world, you must initialize them all.
		 */
		public void InitializeAll() {

            var types = AttributesProcessor.Process(AttributesProcessor.SupportedAttributes);
            foreach (var item in types)
            {
                if (typeof(EntitySystem).IsAssignableFrom(item.Key))
                {
                    var type = item.Key;
                    AttributeEntitySystem pee = (AttributeEntitySystem) item.Value[0];
                    var instance = (EntitySystem)Activator.CreateInstance(type);
                    this.SetSystem<EntitySystem>(instance, pee.ExecutionType, pee.Layer);                
                }
                else if (typeof(IEntityTemplate).IsAssignableFrom(item.Key))
                {
                    var type = item.Key;
                    AttributeEntityTemplate pee = (AttributeEntityTemplate)item.Value[0];
                    var instance = (IEntityTemplate)Activator.CreateInstance(type);
                    this.world.SetEntityTemplate(pee.Name, instance);
                }
                else if (typeof(ComponentPoolable).IsAssignableFrom(item.Key))
                {
                    AttributeComponentPool PropertyComponentPool = null;                   

                    foreach (var val in item.Value)
                    {
                        if (val is AttributeComponentPool)
                            PropertyComponentPool = (AttributeComponentPool)val;                        
                    }
                    
                    var type = item.Key;
                    var methods = type.GetMethods();
                    
                    Func<Type,ComponentPoolable> create = null;
                    foreach (var meth in methods)
                    {
                        var attributes = meth.GetCustomAttributes(false);
                        foreach (var att in attributes)
                        {
                            if (att is AttributeComponentCreate)
                            {
                                create = (Func<Type,ComponentPoolable>)Delegate.CreateDelegate(typeof(Func<Type,ComponentPoolable>), meth);                                
                            }
                        }
                    }

                    if (create == null)
                        create = CreateInstance;

                    Pool<ComponentPoolable> pool = new Pool<ComponentPoolable>(PropertyComponentPool.InitialSize, PropertyComponentPool.Resizes, create);                    
                    world.SetPool(type, pool);
                }
            }

		   for (int i = 0, j = mergedBag.Size; i < j; i++) {
		      mergedBag.Get(i).Initialize();
		   }
		}


        void UpdatebagSync(Bag<EntitySystem> temp) 
        {
            for (int i = 0, j = temp.Size; i < j; i++)
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

#if FULLDOTNET
        TaskFactory factory = new TaskFactory(TaskScheduler.Default);
#endif
        List<Task> tasks = new List<Task>();

        void UpdatebagASSync(Bag<EntitySystem> temp)
        {
            tasks.Clear();
            for (int i = 0, j = temp.Size; i < j; i++)
            {
                EntitySystem es = temp.Get(i);
#if FULLDOTNET
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
#if FULLDOTNET
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
                    UpdatebagASSync(Drawlayers[item]);
                }
            }
            else if (execType == ExecutionType.Update)
            {
                foreach (int item in Updatelayers.Keys)
                {
                    UpdatebagASSync(Updatelayers[item]);
                }
            }	
            
        }
		
	}
}