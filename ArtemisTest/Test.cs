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
			HealthBarRenderSystem hs = (HealthBarRenderSystem)systemManager.SetSystem(new HealthBarRenderSystem());
			systemManager.InitializeAll();
			Entity e = world.CreateEntity();
		    e.AddComponent(new Health(100));
		    e.Refresh();
			for(int i = 0;i < 10;i++) {
				float oldHealth = e.GetComponent<Health>(typeof(Health)).GetHealth();
				world.LoopStart();
    		    world.SetDelta(i);
	            hs.Process();
				float newHealth = e.GetComponent<Health>(typeof(Health)).GetHealth();
				Assert.Greater(oldHealth,newHealth);
			}
			float actualHealth = e.GetComponent<Health>(typeof(Health)).GetHealth();
			Assert.IsTrue(actualHealth == 0);
		}
	}
}

