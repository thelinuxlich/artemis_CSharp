using System;
using Artemis;
using ArtemisTest.Components;
namespace ArtemisTest.System
{
	public class HealthBarRenderSystem : EntityProcessingSystem {
		private ComponentMapper healthMapper;
	
		public HealthBarRenderSystem() : base(typeof(Health)) {}
	
		public override void Initialize() {
			healthMapper = new ComponentMapper(typeof(Health), world.GetEntityManager());
		}
	
		public override void Process(Entity e) {
			Health health = healthMapper.Get<Health>(e);
			health.AddDamage(10);
		}
	
	}
}

