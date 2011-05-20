using System;
using NUnit.Framework;
using Artemis;
using ArtemisTest.Components;
using ArtemisTest.System;

namespace ArtemisTest
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void TestCase ()
		{
			World world = new World();
			SystemManager systemManager = world.GetSystemManager();
			HealthBarRenderSystem hs = systemManager.SetSystem<HealthBarRenderSystem>(new HealthBarRenderSystem());
			systemManager.InitializeAll();
			Entity e = world.CreateEntity();
		    e.AddComponent(new Health(100));
		    e.Refresh();
			int entityId = e.GetId();
			for(int i = 0;i < 10;i++) {
				float oldHealth = e.GetComponent<Health>().GetHealth();
				world.LoopStart();
    		    world.SetDelta(i);
	            hs.Process();
				float newHealth = e.GetComponent<Health>().GetHealth();
				Assert.Greater(oldHealth,newHealth);
			}
			float actualHealth = e.GetComponent<Health>().GetHealth();
			Assert.IsTrue(actualHealth == 0);
			world.DeleteEntity(e);
			world.LoopStart();
			Assert.IsTrue(world.GetEntity(entityId) == null);
		}
	}
}

