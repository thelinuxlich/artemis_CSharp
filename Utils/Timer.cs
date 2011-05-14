using System;
namespace Artemis
{
	public abstract class Timer {

		private int delay;
		private bool repeat;
		private int acc;
		private bool done;
		private bool stopped;
	
		public Timer(int delay, bool repeat) {
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
	
		public bool IsDone() {
			return done;
		}
	
		public bool IsRunning() {
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