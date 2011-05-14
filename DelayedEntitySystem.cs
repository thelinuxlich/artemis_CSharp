using System;
namespace Artemis
{
	public abstract class DelayedEntitySystem : EntitySystem {
		private int delay;
		private bool running;
		private int acc;
	
		public DelayedEntitySystem(params Component[] types) : base(types) {
		}
	
		protected override void ProcessEntities(Bag<Entity> entities) {
			ProcessEntities(entities, acc);
			Stop();
		}
		
		protected override bool CheckProcessing() {
			if(running) {
				acc += world.GetDelta();
				
				if(acc >= delay) {
					return true;
				}
			}
			return false;
		}
	
		/**
		 * The entities to process with accumulated delta.
		 * @param entities read-only bag of entities.
		 */
		protected abstract void ProcessEntities(Bag<Entity> entities, int accumulatedDelta);
		
		
		
		
		/**
		 * Start processing of entities after a certain amount of milliseconds.
		 * 
		 * Cancels current delayed run and starts a new one.
		 * 
		 * @param delay time delay in milliseconds until processing starts.
		 */
		public void StartDelayedRun(int delay) {
			this.delay = delay;
			this.acc = 0;
			running = true;
		}
	
		/**
		 * Get the initial delay that the system was ordered to process entities after.
		 * 
		 * @return the originally set delay.
		 */
		public int GetInitialTimeDelay() {
			return delay;
		}
		
		public int GetRemainingTimeUntilProcessing() {
			if(running) {
				return delay-acc;
			}
			return 0;
		}
		
		/**
		 * Check if the system is counting down towards processing.
		 * 
		 * @return true if it's counting down, false if it's not running.
		 */
		public bool IsRunning() {
			return running;
		}
		
		/**
		 * Aborts running the system in the future and stops it. Call delayedRun() to start it again.
		 */
		public void Stop() {
			this.running = false;
			this.acc = 0;
		}
	
	}
}