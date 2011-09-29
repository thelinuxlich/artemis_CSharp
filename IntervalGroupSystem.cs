using System;

namespace Artemis
{
	public abstract class IntervalGroupSystem : GroupSystem
	{
		private int acc;
		private int interval;
	
		public IntervalGroupSystem(int interval, string group) : base(group) {
			this.interval = interval;
		}

        protected override bool CheckProcessing()
        {
			acc += world.GetDelta();
			if(acc >= interval) {
				acc -= interval;
				return enabled;
			}
			return false;
		}
	}
}

