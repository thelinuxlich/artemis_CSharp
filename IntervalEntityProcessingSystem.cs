using System;
using System.Collections.Generic;
namespace Artemis
{
	public abstract class IntervalEntityProcessingSystem : IntervalEntitySystem {

		/**
		 * Create a new IntervalEntityProcessingSystem. It requires at least one component.
		 * @param requiredType the required component type.
		 * @param otherTypes other component types.
		 */
		public IntervalEntityProcessingSystem(int interval, Type requiredType, params Type[] otherTypes) : base(interval, GetMergedTypes(requiredType, otherTypes)) {
		}
		
		/**
		 * Process a entity this system is interested in.
		 * @param e the entity to process.
		 */
		public abstract void Process(Entity e);

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
			for (int i = 0, s = entities.Count; s > i; i++) {
				Process(entities[i]);
			}
		}
	
	}
}

