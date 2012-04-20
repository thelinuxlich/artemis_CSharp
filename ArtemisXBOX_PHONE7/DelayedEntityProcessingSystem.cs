using System;
using System.Collections.Generic;
namespace Artemis
{
	public abstract class DelayedEntityProcessingSystem : DelayedEntitySystem {

		
        /// <summary>
        /// Create a new DelayedEntityProcessingSystem. It requires at least one component.
        /// </summary>
        /// <param name="requiredType">The required component type.</param>
        /// <param name="otherTypes">Other component types.</param>
		public DelayedEntityProcessingSystem(Type requiredType,params Type[] otherTypes) : base(GetMergedTypes(requiredType, otherTypes)){
		}

        /// <summary>
        /// Process an entity this system is interested in.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="accumulatedDelta">The entity to process.</param>
		public abstract void Process(Entity e, int accumulatedDelta);


        /// <summary>
        /// Process all entities with the delayed Entity processing system
        /// </summary>
        /// <param name="entities">Entities to process</param>
        /// <param name="accumulatedDelta">Total Delay</param>
        public override void ProcessEntities(Dictionary<int, Entity> entities, int accumulatedDelta)
        {
			foreach (Entity item in entities.Values)
	        {
		       Process(item, accumulatedDelta);
	        }            
			
		}
	}	
}

