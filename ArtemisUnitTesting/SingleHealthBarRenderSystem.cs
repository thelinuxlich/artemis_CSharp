using System;
using Artemis;
using ArtemisTest.Components;
using Artemis.System;
namespace ArtemisTest.System
{
	public class SingleHealthBarRenderSystem : EntityProcessingSystem {
		private ComponentMapper<Health> healthMapper;

        public SingleHealthBarRenderSystem() : base(typeof(Health)) { }
	
		public override void Initialize() {
            healthMapper = new ComponentMapper<Health>(this.EntityWorld);
		}
	
		public override void Process(Entity e) {
			Health health = healthMapper.Get(e);
			health.AddDamage(10);

            ///wasting time ....
            for (int i = 0; i < 10; i++)
            {
                double x = Math.Log(i) * Math.Cos(i);
                x = Math.Log(i) * Math.Cos(i);
                x = Math.Log(i) * Math.Cos(i);
                x = Math.Log(i) * Math.Cos(i);
            }
		}
	
	}
}

