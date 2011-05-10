using System;
namespace Artemis
{
	public abstract class IntervalEntityProcessingSystem : IntervalEntitySystem {

		/**
		 * Create a new IntervalEntityProcessingSystem. It requires at least one component.
		 * @param requiredType the required component type.
		 * @param otherTypes other component types.
		 */
		public IntervalEntityProcessingSystem(int interval, Component requiredType, params Component[] otherTypes) {
			super(interval, getMergedTypes(requiredType, otherTypes));
		}
		
		/**
		 * Process a entity this system is interested in.
		 * @param e the entity to process.
		 */
		protected abstract void process(Entity e);
		
		protected void processEntities(Bag<Entity> entities) {
			for (int i = 0, s = entities.size(); s > i; i++) {
				process(entities.get(i));
			}
		}
	
	}
}

