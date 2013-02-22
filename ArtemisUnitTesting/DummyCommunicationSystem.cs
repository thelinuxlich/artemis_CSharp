using System;
using Artemis;
using ArtemisTest.Components;
using Artemis.System;
using Artemis.Blackboard;
namespace ArtemisTest.System
{
	public class DummyCommunicationSystem : EntityProcessingSystem {
		private ComponentMapper<Health> healthMapper;

        public DummyCommunicationSystem() : base(typeof(Health)) {
                        
            BlackBoard.AddTrigger(
                new SimpleTrigger("Damage",
                    (a,b) =>
                    {
                        return true;
                    }
            ,
            (a) =>
            {
                if (a == TriggerStateType.ValueChanged)
                {                    
                    damage = BlackBoard.GetEntry<int>("Damage");
                }
            }
            ));

            damage = BlackBoard.GetEntry<int>("Damage");
        }

        int damage = 10;
		public override void Initialize() {
            healthMapper = new ComponentMapper<Health>(this.EntityWorld);
		}
	
		public override void Process(Entity e) {

            healthMapper.Get(e).AddDamage(damage);
		}
	
	}
}

