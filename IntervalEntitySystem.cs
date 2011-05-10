using System;
namespace Artemis
{
	public abstract class IntervalEntitySystem : EntitySystem {
		private int acc;
		private int interval;
	
		public IntervalEntitySystem(int interval, params Component[] types) {
			super(types);
			this.interval = interval;
		}
	
		protected boolean checkProcessing() {
			acc += world.getDelta();
			if(acc >= interval) {
				acc -= interval;
				return true;
			}
			return false;
		}
	
	}
}

