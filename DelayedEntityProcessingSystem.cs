using System;
namespace Artemis
{
	public abstract class DelayedEntityProcessingSystem : DelayedEntitySystem {
	
		/**
		 * Create a new DelayedEntityProcessingSystem. It requires at least one component.
		 * @param requiredType the required component type.
		 * @param otherTypes other component types.
		 */
		public DelayedEntityProcessingSystem(Component requiredType,params Component[] otherTypes) : base(GetMergedTypes(requiredType, otherTypes)){
		}
		
		/**
		 * Process a entity this system is interested in.
		 * @param e the entity to process.
		 */
		protected abstract void Process(Entity e, int accumulatedDelta);
	
		protected override void ProcessEntities(Bag<Entity> entities, int accumulatedDelta) {
			for (int i = 0, s = entities.Size(); s > i; i++) {
				Process(entities.Get(i), accumulatedDelta);
			}
		}
	}	
}

