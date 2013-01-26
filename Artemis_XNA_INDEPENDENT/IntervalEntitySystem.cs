using System;
namespace Artemis
{
	public abstract class IntervalEntitySystem : EntitySystem {
		private float acc;
		private float interval;
	
		public IntervalEntitySystem(float interval, params Type[] types) : base(types) {
			this.interval = interval;
		}

        public IntervalEntitySystem(int interval, Aspect aspect)
            : base(aspect)
        {
            this.interval = interval;
        }

        protected override bool CheckProcessing()
        {
			acc += world.ElapsedTime;
			if(acc >= interval) {
				acc -= interval;
				return enabled;
			}
			return false;
		}
	
	}
}

