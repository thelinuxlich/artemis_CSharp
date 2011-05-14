using System;
namespace Artemis
{
	public abstract class IntervalEntityProcessingSystem : IntervalEntitySystem {

		/**
		 * Create a new IntervalEntityProcessingSystem. It requires at least one component.
		 * @param requiredType the required component type.
		 * @param otherTypes other component types.
		 */
		public IntervalEntityProcessingSystem(int interval, Component requiredType, params Component[] otherTypes) : base(interval, GetMergedTypes(requiredType, otherTypes)) {
		}
		
		/**
		 * Process a entity this system is interested in.
		 * @param e the entity to process.
		 */
		protected abstract void Process(Entity e);
		
		protected override void ProcessEntities(Bag<Entity> entities) {
			for (int i = 0, s = entities.Size(); s > i; i++) {
				Process(entities.Get(i));
			}
		}
	
	}
}

