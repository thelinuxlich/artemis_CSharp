using System;
using Artemis;
using ArtemisTest.Components;
using ArtemisTest.System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ArtemisTest
{	
	public class Test
	{
		static Bag<Component> healthBag = new Bag<Component>();
		static Dictionary<Type,Bag<Component>> componentPool = new Dictionary<Type, Bag<Component>>();			
			
		private static void RemovedComponent(Entity e,Component c) 
      	{
        	 Console.WriteLine("This was the component removed: "+(c.GetType()));
			 Bag<Component> tempBag;
			 componentPool.TryGetValue(c.GetType(),out tempBag);
			 Console.WriteLine("Health Component Pool has "+tempBag.Size()+" objects");
			 tempBag.Add(c);
			 componentPool.TryGetValue(c.GetType(),out tempBag);
			 Console.WriteLine("Health Component Pool now has "+tempBag.Size()+" objects");
      	}
		
		private static void RemovedEntity(Entity e) 
      	{
        	 Console.WriteLine("This was the entity removed: "+(e.GetUniqueId()));
      	}


        static void multi()
        {
            healthBag.Add(new Health());
            healthBag.Add(new Health());
            componentPool.Add(typeof(Health), healthBag);

            Bag<Component> tempBag;
            EntityWorld world = new EntityWorld();
            SystemManager systemManager = world.GetSystemManager();
            world.GetEntityManager().RemovedComponentEvent += new RemovedComponentHandler(RemovedComponent);
            world.GetEntityManager().RemovedEntityEvent += new RemovedEntityHandler(RemovedEntity);

            EntitySystem hs = systemManager.SetSystem(new MultHealthBarRenderSystem(),ExecutionType.Update);
            //EntitySystem hs = systemManager.SetSystem(new SingleHEAVYHealthBarRenderSystem(),ExecutionType.Update);
            systemManager.InitializeAll();

            List<Entity> l = new List<Entity>();
            for (int i = 0; i < 1000; i++)
            {
                Entity et = world.CreateEntity();
                et.AddComponent(new Health());
                et.GetComponent<Health>().AddHealth(100);
                et.Refresh();
                l.Add(et);
            }

            for (int i = 0; i < 100; i++)
            {
                DateTime dt = DateTime.Now;
                world.LoopStart();                
                systemManager.UpdateSynchronous(ExecutionType.Update);
                Console.WriteLine((DateTime.Now - dt).TotalMilliseconds);
            }            

            int df = 0;
            foreach (var item in l)
            {
                if (item.GetComponent<Health>().GetHealth() == 90)
                {
                    df++;
                }
            }

 
            Console.ReadLine();
        }

        static void multsystem()
        {
            healthBag.Add(new Health());
            healthBag.Add(new Health());
            componentPool.Add(typeof(Health), healthBag);

            Bag<Component> tempBag;
            EntityWorld world = new EntityWorld();
            SystemManager systemManager = world.GetSystemManager();
            world.GetEntityManager().RemovedComponentEvent += new RemovedComponentHandler(RemovedComponent);
            world.GetEntityManager().RemovedEntityEvent += new RemovedEntityHandler(RemovedEntity);            
            EntitySystem hs = systemManager.SetSystem(new SingleHealthBarRenderSystem(),ExecutionType.Update);
            hs = systemManager.SetSystem(new DummySystem(),ExecutionType.Update);
            hs = systemManager.SetSystem(new DummySystem2(),ExecutionType.Update);
            hs = systemManager.SetSystem(new DummySystem3(),ExecutionType.Update);
            systemManager.InitializeAll();           
            

            List<Entity> l = new List<Entity>();
            for (int i = 0; i < 100000; i++)
            {
                Entity et = world.CreateEntity();
                et.AddComponent(new Health());
                et.GetComponent<Health>().AddHealth(100);
                et.Refresh();
                l.Add(et);
            }

            for (int i = 0; i < 100; i++)
            {
                DateTime dt = DateTime.Now;
                world.LoopStart();
                systemManager.UpdateAsynchronous(ExecutionType.Update);
                //systemManager.UpdateSynchronous(ExecutionType.Update);
                Console.WriteLine((DateTime.Now - dt).TotalMilliseconds);
            }

            //int df = 0;
            //foreach (var item in l)
            //{
            //    if (item.GetComponent<Health>().GetHealth() == 90)
            //    {
            //        df++;
            //    }
            //    else
            //    {
            //        Console.WriteLine("errro");
            //    }
            //}

            Console.ReadLine();
        }


        static void Main(String[] args)
        {
            //multi();
            //multsystem();
            QueueSystemTeste();
		}


        public static void QueueSystemTeste()
        {
            EntityWorld world = new EntityWorld();
            SystemManager systemManager = world.GetSystemManager();
            EntitySystem hs = systemManager.SetSystem(new QueueSystemProcessingThreadSafe(), ExecutionType.Update);
            systemManager.InitializeAll();

            QueueSystemProcessingThreadSafe.EntitiesToProcessEachFrame = 1000;
            for (int i = 0; i < 10000000; i++)
            {
                Entity et = world.CreateEntity();
                et.AddComponent(new Health());
                et.GetComponent<Health>().AddHealth(100);
                QueueSystemProcessingThreadSafe.AddToQueue(et);
            }

            Console.WriteLine("Start");
            while (QueueSystemProcessingThreadSafe.QueueCount > 0)
            {
                DateTime dt = DateTime.Now;
                world.LoopStart();                
                systemManager.UpdateSynchronous(ExecutionType.Update);
                Console.WriteLine("Count: " + QueueSystemProcessingThreadSafe.QueueCount);
                Console.WriteLine("Time: " + (DateTime.Now - dt).TotalMilliseconds);

            }
            Console.WriteLine("End");

        }
	}
}