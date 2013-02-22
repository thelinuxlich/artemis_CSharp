using System;
using Artemis;
using ArtemisTest.Components;
using Artemis.System;

namespace ArtemisTest.System
{
	public class DummySystem3 : EntityProcessingSystem {
		private ComponentMapper<Health> healthMapper;

        public DummySystem3() : base(typeof(Health)) { }
	
		public override void Initialize() {
            healthMapper = new ComponentMapper<Health>(this.EntityWorld);
		}
	
		public override void Process(Entity e) {
			
            for (int i = 0; i < 5; i++)
            {
                double x = Math.Log(i) * Math.Cos(i) * Math.Sinh(i);
            }
		}
	
	}
}

