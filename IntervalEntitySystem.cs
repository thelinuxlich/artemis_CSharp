using System;
namespace Artemis
{
	public abstract class IntervalEntitySystem : EntitySystem {
		private int acc;
		private int interval;
	
		public IntervalEntitySystem(int interval, params Component[] types) : base(types) {
			this.interval = interval;
		}
	
		public override bool CheckProcessing() {
			acc += world.GetDelta();
			if(acc >= interval) {
				acc -= interval;
				return true;
			}
			return false;
		}
	
	}
}

