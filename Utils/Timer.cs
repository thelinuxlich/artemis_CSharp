using System;
namespace Artemis
{
	public abstract class Timer {

		private int delay;
		private boolean repeat;
		private int acc;
		private boolean done;
		private boolean stopped;
	
		public Timer(int delay, boolean repeat) {
			this.delay = delay;
			this.repeat = repeat;
			this.acc = 0;
		}
	
		public void Update(int delta) {
			if (!done && !stopped) {
				acc += delta;
	
				if (acc >= delay) {
					acc -= delay;
	
					if (repeat) {
						Reset();
					} else {
						done = true;
					}
	
					Execute();
				}
			}
		}
	
		public void Reset() {
			stopped = false;
			done = false;
			acc = 0;
		}
	
		public boolean IsDone() {
			return done;
		}
	
		public boolean IsRunning() {
			return !done && acc < delay && !stopped;
		}
	
		public void Stop() {
			stopped = true;
		}
	
		public void SetDelay(int delay) {
			this.delay = delay;
		}
	
		public abstract void Execute();
	
		public float GetPercentageRemaining() {
			if (done)
				return 100;
			else if (stopped)
				return 0;
			else
				return 1 - (float) (delay - acc) / (float) delay;
		}
	
		public int GetDelay() {
			return delay;
		}
	
	}
}