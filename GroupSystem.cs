using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis
{
    public abstract class GroupSystem : EntitySystem
    {
        private String group;
        public GroupSystem(String group) {
            this.group = group;
		}
		
		/**
		 * Process a entity this system is interested in.
		 * @param e the entity to process.
		 */
		public abstract void Process(Entity e);

        public override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            Bag<Entity> groupedEntities = this.world.GetGroupManager().getEntities(group);
            for (int i = 0, j = groupedEntities.Size(); i < j; i++)            {
                Process(groupedEntities.Get(i));
            } 
		}		
    }
}
