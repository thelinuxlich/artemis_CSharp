using System;
using Artemis;
using ArtemisTest.Components;
namespace ArtemisTest.System
{
	public class SingleHealthBarRenderSystem : EntityProcessingSystem {
		private ComponentMapper<Health> healthMapper;

        public SingleHealthBarRenderSystem() : base(typeof(Health)) { }
	
		public override void Initialize() {
			healthMapper = new ComponentMapper<Health>(world);
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

