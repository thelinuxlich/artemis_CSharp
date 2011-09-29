using System;
using Artemis;
using ArtemisTest.Components;
namespace ArtemisTest.System
{
	public class DummySystem : EntityProcessingSystem {
		private ComponentMapper<Health> healthMapper;

        public DummySystem() : base(typeof(Health)) { }
	
		public override void Initialize() {
			healthMapper = new ComponentMapper<Health>(world);
		}
	
		public override void Process(Entity e) {
			
            for (int i = 0; i < 10; i++)
            {
                double x = Math.Log(i) * Math.Cos(i);
            }
		}
	
	}
}

