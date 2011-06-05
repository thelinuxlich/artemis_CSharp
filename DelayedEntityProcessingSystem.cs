using System;
using System.Collections.Generic;
namespace Artemis
{
	public abstract class DelayedEntityProcessingSystem : DelayedEntitySystem {
	
		/**
		 * Create a new DelayedEntityProcessingSystem. It requires at least one component.
		 * @param requiredType the required component type.
		 * @param otherTypes other component types.
		 */
		public DelayedEntityProcessingSystem(Type requiredType,params Type[] otherTypes) : base(GetMergedTypes(requiredType, otherTypes)){
		}
		
		/**
		 * Process a entity this system is interested in.
		 * @param e the entity to process.
		 */
		public abstract void Process(Entity e, int accumulatedDelta);

        public override void ProcessEntities(Dictionary<int, Entity> entities, int accumulatedDelta)
        {
			foreach (Entity item in entities.Values)
	        {
		       Process(item, accumulatedDelta);
	        }            
			
		}
	}	
}

