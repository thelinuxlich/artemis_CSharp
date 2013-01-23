using System;
namespace Artemis
{
	public abstract class IntervalEntitySystem : EntitySystem {
		private int acc;
		private int interval;
	
		public IntervalEntitySystem(int interval, params Type[] types) : base(types) {
			this.interval = interval;
		}

        public IntervalEntitySystem(int interval, Aspect aspect)
            : base(aspect)
        {
            this.interval = interval;
        }

        protected override bool CheckProcessing()
        {
			acc += world.Delta;
			if(acc >= interval) {
				acc -= interval;
				return enabled;
			}
			return false;
		}
	
	}
}

