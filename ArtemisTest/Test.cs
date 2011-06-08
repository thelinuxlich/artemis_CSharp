using System;
using NUnit.Framework;
using Artemis;
using ArtemisTest.Components;
using ArtemisTest.System;
using System.Collections.Generic;

namespace ArtemisTest
{
	[TestFixture()]
	public class Test
	{
		Bag<Component> healthBag = new Bag<Component>();
		Bag<Entity> entityPool = new Bag<Entity>();
		Dictionary<Type,Bag<Component>> componentPool = new Dictionary<Type, Bag<Component>>();			
			
		private void RemovedComponent(Component c) 
      	{
        	 Console.WriteLine("This was the component removed: "+(c.GetType()));
			 Bag<Component> tempBag;
			 componentPool.TryGetValue(c.GetType(),out tempBag);
			 Console.WriteLine("Health Component Pool has "+tempBag.Size()+" objects");
			 tempBag.Add(c);
			 componentPool.TryGetValue(c.GetType(),out tempBag);
			 Console.WriteLine("Health Component Pool now has "+tempBag.Size()+" objects");
      	}
		
		private void RemovedEntity(Entity e) 
      	{
        	 Console.WriteLine("This was the entity removed: "+(e.GetUniqueId()));
			 entityPool.Add(e);
			 Console.WriteLine("Entity pool has "+entityPool.Size()+" entities");
      	}
		
		[Test()]
		public void TestCase ()
		{
			entityPool.Add(new Entity());
			entityPool.Add(new Entity());
			healthBag.Add(new Health());
			healthBag.Add(new Health());
			componentPool.Add(typeof(Health), healthBag);
			
			Bag<Component> tempBag;
			
			World world = new World();
			SystemManager systemManager = world.GetSystemManager();
			world.GetEntityManager().RemovedComponentEvent += new RemovedComponentHandler(RemovedComponent);
			world.GetEntityManager().RemovedEntityEvent += new RemovedEntityHandler(RemovedEntity);
			EntitySystem hs = systemManager.SetSystem(new HealthBarRenderSystem());
			systemManager.InitializeAll();
			Entity e = world.AddEntity(entityPool.RemoveLast());
			Entity e2 = world.AddEntity(entityPool.RemoveLast());
			e.AddComponent(componentPool[typeof(Health)].RemoveLast());
			e2.AddComponent(componentPool[typeof(Health)].RemoveLast());
			e.GetComponent<Health>().AddHealth(100);
			e2.GetComponent<Health>().AddHealth(1000);
			componentPool.TryGetValue(typeof(Health),out tempBag);
			Console.WriteLine("Health Component Pool now has "+tempBag.Size()+" objects");
		    e.Refresh();
			e2.Refresh();
			for(int i = 0;i < 10;i++) {
                world.LoopStart();
				world.SetDelta(i);
	            hs.Process();
			}
            world.LoopStart();
			world.DeleteEntity(e);
			world.DeleteEntity(e2);
			world.LoopStart();
		}
	}
}