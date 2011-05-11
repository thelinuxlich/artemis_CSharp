using System;
namespace Artemis
{
	public abstract class EntityProcessingSystem : EntitySystem {
		
		/**
		 * Create a new EntityProcessingSystem. It requires at least one component.
		 * @param requiredType the required component type.
		 * @param otherTypes other component types.
		 */
		public EntityProcessingSystem(Type requiredType,params Type[] otherTypes) {
			super(getMergedTypes(requiredType, otherTypes));
		}
		
		/**
		 * Process a entity this system is interested in.
		 * @param e the entity to process.
		 */
		protected abstract void process(Entity e);
	
		protected override void processEntities(Bag<Entity> entities) {
			for (int i = 0, s = entities.size(); s > i; i++) {
				process(entities.get(i));
			}
		}
		
		protected override boolean checkProcessing() {
			return true;
		}
		
	}
}

