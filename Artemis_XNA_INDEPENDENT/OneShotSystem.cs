using System;
using System.Collections.Generic;

namespace Artemis
{
	public abstract class OneShotSystem : EntitySystem
	{
		public OneShotSystem () : base()
		{
		}
		
		public override void Change(Entity e)
        {
        }
		
		public override void Initialize() {
			Enable();
		}

        public override void Added(Entity e)
        {
			Enable();
        }
		
		protected override void End()
		{
			Disable();
		}
		
		public abstract void Process(Entity e);

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            foreach (Entity item in entities.Values)
            {
                if (item.IsEnabled())
                {
                    Process(item);
					Remove (item);
                }
            }            
		}
	}
}

