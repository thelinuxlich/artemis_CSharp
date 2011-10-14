using System;

namespace Artemis
{
	public abstract class IntervalTagSystem : TagSystem
	{
		private int acc;
		private int interval;
	
		public IntervalTagSystem(int interval, string tag) : base(tag) {
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

