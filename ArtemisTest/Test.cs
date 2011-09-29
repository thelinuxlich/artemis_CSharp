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

            EntitySystemsManager manager = new EntitySystemsManager();
            Bag<Component> tempBag;
            EntityWorld world = new EntityWorld();
            SystemManager systemManager = world.GetSystemManager();
            world.GetEntityManager().RemovedComponentEvent += new RemovedComponentHandler(RemovedComponent);
            world.GetEntityManager().RemovedEntityEvent += new RemovedEntityHandler(RemovedEntity);

            EntitySystem hs = systemManager.SetSystem(new MultHealthBarRenderSystem());
            //EntitySystem hs = systemManager.SetSystem(new SingleHEAVYHealthBarRenderSystem());
            manager.AddSystem(hs);
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
                manager.UpdateSyncronous();
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

            EntitySystemsManager manager = new EntitySystemsManager();
            Bag<Component> tempBag;
            EntityWorld world = new EntityWorld();
            SystemManager systemManager = world.GetSystemManager();
            world.GetEntityManager().RemovedComponentEvent += new RemovedComponentHandler(RemovedComponent);
            world.GetEntityManager().RemovedEntityEvent += new RemovedEntityHandler(RemovedEntity);            
            EntitySystem hs = systemManager.SetSystem(new SingleHealthBarRenderSystem());
            manager.AddSystem(hs);
            hs = systemManager.SetSystem(new DummySystem());
            manager.AddSystem(hs);
            hs = systemManager.SetSystem(new DummySystem2());
            manager.AddSystem(hs);
            hs = systemManager.SetSystem(new DummySystem3());
            manager.AddSystem(hs);
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
                //manager.UpdateAsyncronous();
                manager.UpdateSyncronous();
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
            multi();
            //multsystem();
		}
	}
}