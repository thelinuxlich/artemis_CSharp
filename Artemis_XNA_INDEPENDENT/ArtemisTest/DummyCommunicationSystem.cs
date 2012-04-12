using System;
using Artemis;
using ArtemisTest.Components;
namespace ArtemisTest.System
{
	public class DummyCommunicationSystem : EntityProcessingSystem {
		private ComponentMapper<Health> healthMapper;

        public DummyCommunicationSystem() : base(typeof(Health)) {

            blackBoard.AddTrigger(
                new SimpleTrigger("Damage",
                    (a,b) =>
                    {
                        return true;
                    }
            ,
            (a) =>
            {
                if(a == TriggerState.VALUE_CHANGED)
                {                    
                    damage = BlackBoard.GetEntry<int>("Damage");
                }
            }
            ));

            damage = BlackBoard.GetEntry<int>("Damage");
        }

        int damage = 10;
		public override void Initialize() {
			healthMapper = new ComponentMapper<Health>(world);
		}
	
		public override void Process(Entity e) {

            healthMapper.Get(e).AddDamage(damage);
		}
	
	}
}

