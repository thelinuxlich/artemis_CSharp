using System;
using System.Collections.Generic;
namespace Artemis
{
	public abstract class DelayedEntitySystem : EntitySystem {
		private int delay;
		private bool running;
		private int acc;
	
		public DelayedEntitySystem(params Type[] types) : base(types) {
		}

        public DelayedEntitySystem(Aspect aspect)
            : base(aspect)
        {
        }

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
			ProcessEntities(entities, acc);
			Stop();
		}

        protected override bool CheckProcessing()
        {
			if(running) {
				acc += world.Delta;
				
				if(acc >= delay) {
					return enabled;
				}
			}
			return false;
		}
	
		/**
		 * The entities to process with accumulated delta.
		 * @param entities read-only bag of entities.
		 */
        public abstract void ProcessEntities(Dictionary<int, Entity> entities, int accumulatedDelta);
		
		
		
		
		/**
		 * Start processing of entities after a certain amount of milliseconds.
		 * 
		 * Cancels current delayed run and starts a new one.
		 * 
		 * @param delay time delay in milliseconds until processing starts.
		 */
		public void StartDelayedRun(int delay) {
			this.delay = delay;
			acc = 0;
			running = true;
		}
	
		/**
		 * Get the initial delay that the system was ordered to process entities after.
		 * 
		 * @return the originally set delay.
		 */
		public int InitialTimeDelay {
			get { return delay;}
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
			running = false;
			acc = 0;
		}
	
	}
}