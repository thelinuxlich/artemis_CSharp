using System;
using System.Collections.Generic;
namespace Artemis
{
	public abstract class EntityProcessingSystem : EntitySystem {
		
		/**
		 * Create a new EntityProcessingSystem. It requires at least one component.
		 * @param requiredType the required component type.
		 * @param otherTypes other component types.
		 */
		public EntityProcessingSystem(Type requiredType,params Type[] otherTypes) : base(GetMergedTypes(requiredType, otherTypes)) {
		}
		
		/**
		 * Process a entity this system is interested in.
		 * @param e the entity to process.
		 */
		public abstract void Process(Entity e);

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            foreach (Entity item in entities.Values)
            {
                Process(item);
            }            
		}
	}
}

