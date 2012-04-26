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
        	 Console.WriteLine("This was the entity removed: "+(e.UniqueId));
      	}


        public static void multi()
        {
            healthBag.Add(new Health());
            healthBag.Add(new Health());
            componentPool.Add(typeof(Health), healthBag);

            Bag<Component> tempBag;
            EntityWorld world = new EntityWorld();
            SystemManager systemManager = world.SystemManager;
            world.EntityManager.RemovedComponentEvent += new RemovedComponentHandler(RemovedComponent);
            world.EntityManager.RemovedEntityEvent += new RemovedEntityHandler(RemovedEntity);

            EntitySystem hs = systemManager.SetSystem(new MultHealthBarRenderSystem(),ExecutionType.Update);
            //EntitySystem hs = systemManager.SetSystem(new SingleHEAVYHealthBarRenderSystem(),ExecutionType.Update);
            systemManager.InitializeAll();

            List<Entity> l = new List<Entity>();
            for (int i = 0; i < 1000; i++)
            {
                Entity et = world.CreateEntity();
                et.AddComponent(new Health());
                et.GetComponent<Health>().HP += 100;
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
                if (item.GetComponent<Health>().HP == 90)
                {
                    df++;
                }
            }

             
        }

        public static void multsystem()
        {
            healthBag.Clear();
            componentPool.Clear();

            healthBag.Add(new Health());
            healthBag.Add(new Health());
            componentPool.Add(typeof(Health), healthBag);

            Bag<Component> tempBag;
            EntityWorld world = new EntityWorld();
            SystemManager systemManager = world.SystemManager;
            world.EntityManager.RemovedComponentEvent += new RemovedComponentHandler(RemovedComponent);
            world.EntityManager.RemovedEntityEvent += new RemovedEntityHandler(RemovedEntity);            
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
                et.GetComponent<Health>().HP += 100;
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
        }

#if !MONOTOUCH
        static void Main(String[] args)
        {
	            multi();
	            multsystem();
	            QueueSystemTeste();
	            HybridQueueSystemTeste();
	            SystemComunicationTeste();
		}
#endif		

        public static void SystemComunicationTeste()
        {
            EntitySystem.BlackBoard.SetEntry<int>("Damage", 5);

            EntityWorld world = new EntityWorld();
            SystemManager systemManager = world.SystemManager;
            DummyCommunicationSystem DummyCommunicationSystem = new DummyCommunicationSystem();
            systemManager.SetSystem(DummyCommunicationSystem, ExecutionType.Update);
            systemManager.InitializeAll();            

            List<Entity> l = new List<Entity>();
            for (int i = 0; i < 100; i++)
            {
                Entity et = world.CreateEntity();
                et.AddComponent(new Health());
                et.GetComponent<Health>().HP += 100;
                et.Refresh();
                l.Add(et);
            }
            
            {
                DateTime dt = DateTime.Now;
                world.LoopStart();
                systemManager.UpdateSynchronous(ExecutionType.Update);
                Console.WriteLine((DateTime.Now - dt).TotalMilliseconds);
            }

            EntitySystem.BlackBoard.SetEntry<int>("Damage", 10);

            {
                DateTime dt = DateTime.Now;
                world.LoopStart();
                systemManager.UpdateSynchronous(ExecutionType.Update);
                Console.WriteLine((DateTime.Now - dt).TotalMilliseconds);
            }

            foreach (var item in l)
            {
                Debug.Assert(item.GetComponent<Health>().HP == 85);
            }
            
        }

        public static void QueueSystemTeste()
        {
            EntityWorld world = new EntityWorld();
            SystemManager systemManager = world.SystemManager;
            QueueSystemTest QueueSystemTest = new ArtemisTest.QueueSystemTest();
            QueueSystemTest QueueSystemTest2 = new ArtemisTest.QueueSystemTest();
            systemManager.SetSystem(QueueSystemTest, ExecutionType.Update);
            systemManager.SetSystem(QueueSystemTest2, ExecutionType.Update);
            
            systemManager.InitializeAll();

            QueueSystemTest.SetQueueProcessingLimit(20, QueueSystemTest.Id);
            Debug.Assert(QueueSystemTest.GetQueueProcessingLimit(QueueSystemTest.Id) == QueueSystemTest.GetQueueProcessingLimit(QueueSystemTest2.Id));
            
            
            QueueSystemTest2 QueueSystemTestteste = new ArtemisTest.QueueSystemTest2();
            Debug.Assert(QueueSystemTest.GetQueueProcessingLimit(QueueSystemTestteste.Id) != QueueSystemTest.GetQueueProcessingLimit(QueueSystemTest2.Id));
            
            QueueSystemTest.SetQueueProcessingLimit(1000, QueueSystemTest.Id);            

            List<Entity> l = new List<Entity>();
            for (int i = 0; i < 1000000; i++)
            {
                Entity et = world.CreateEntity();
                et.AddComponent(new Health());
                et.GetComponent<Health>().HP = 100;
                QueueSystemTest.AddToQueue(et, QueueSystemTest.Id);
                l.Add(et);
            }

            Console.WriteLine("Start");
            while (QueueSystemTest.QueueCount(QueueSystemTest.Id) > 0 && QueueSystemTest.QueueCount(QueueSystemTest2.Id) > 0)
            {
                DateTime dt = DateTime.Now;
                world.LoopStart();
                systemManager.UpdateAsynchronous(ExecutionType.Update);
                Console.WriteLine("Count: " + QueueSystemTest.QueueCount(QueueSystemTest.Id));
                Console.WriteLine("Time: " + (DateTime.Now - dt).TotalMilliseconds);

            }
            Console.WriteLine("End");

            foreach (var item in l)
            {
                Debug.Assert(item.GetComponent<Health>().HP == 90);
            }
        }


        public static void HybridQueueSystemTeste()
        {

            EntityWorld world = new EntityWorld();
            SystemManager systemManager = world.SystemManager;
            HybridQueueSystemTest HybridQueueSystemTest = new ArtemisTest.HybridQueueSystemTest();
            EntitySystem hs = systemManager.SetSystem(HybridQueueSystemTest, ExecutionType.Update);
            systemManager.InitializeAll();

            List<Entity> l = new List<Entity>();
            for (int i = 0; i < 100; i++)
            {
                Entity et = world.CreateEntity();
                et.AddComponent(new Health());
                et.GetComponent<Health>().HP += 100;
                et.Refresh();
                l.Add(et);
            }

            for (int i = 0; i < 100; i++)
            {
                Entity et = world.CreateEntity();
                et.AddComponent(new Health());
                et.GetComponent<Health>().HP += 100;
                HybridQueueSystemTest.AddToQueue(et);
                l.Add(et);
            }

            int j = 0;
            while (HybridQueueSystemTest.QueueCount > 0) 
            {
                j++;
                DateTime dt = DateTime.Now;
                world.LoopStart();                
                systemManager.UpdateSynchronous(ExecutionType.Update);
                Console.WriteLine((DateTime.Now - dt).TotalMilliseconds);
            }

            for (int i = 0; i < 100; i++)
            {
                Debug.Assert(l[i].GetComponent<Health>().HP == 100 - (10 * j));
            }

            for (int i = 100; i < 200; i++)
            {
                Debug.Assert(l[i].GetComponent<Health>().HP == 90);
            }
            
        }
          
	}
}