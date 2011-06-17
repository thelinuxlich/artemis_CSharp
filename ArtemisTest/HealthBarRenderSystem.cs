using System;
using Artemis;
using ArtemisTest.Components;
namespace ArtemisTest.System
{
	public class HealthBarRenderSystem : EntityProcessingSystem {
		private ComponentMapper<Health> healthMapper;
	
		public HealthBarRenderSystem() : base(typeof(Health)) {}
	
		public override void Initialize() {
			healthMapper = new ComponentMapper<Health>(world);
		}
	
		public override void Process(Entity e) {
			Health health = healthMapper.Get(e);
			health.AddDamage(10);
		}
	
	}
}

