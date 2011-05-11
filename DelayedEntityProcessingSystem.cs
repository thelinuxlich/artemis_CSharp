using System;
namespace Artemis
{
	public abstract class DelayedEntityProcessingSystem : DelayedEntitySystem {
	
		/**
		 * Create a new DelayedEntityProcessingSystem. It requires at least one component.
		 * @param requiredType the required component type.
		 * @param otherTypes other component types.
		 */
		public DelayedEntityProcessingSystem(Type requiredType,params Type[] otherTypes) {
			super(getMergedTypes(requiredType, otherTypes));
		}
		
		/**
		 * Process a entity this system is interested in.
		 * @param e the entity to process.
		 */
		protected abstract void process(Entity e, int accumulatedDelta);
	
		protected override void processEntities(Bag<Entity> entities, int accumulatedDelta) {
			for (int i = 0, s = entities.size(); s > i; i++) {
				process(entities.get(i), accumulatedDelta);
			}
		}
	}	
}

