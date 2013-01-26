using System;

namespace Artemis
{
	public abstract class IntervalTagSystem : TagSystem
	{
		private float acc;
		private float interval;
	
		public IntervalTagSystem(float interval, string tag) : base(tag) {
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

