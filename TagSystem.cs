using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis
{
    public abstract class TagSystem : EntitySystem
    {
        private String tag;
        public TagSystem(String tag) {
            this.tag = tag;
		}
		
		/**
		 * Process a entity this system is interested in.
		 * @param e the entity to process.
		 */
		public abstract void Process(Entity e);

        public override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            Entity e = this.world.GetTagManager().GetEntity(tag);
            Process(e);
		}
		
		public override bool CheckProcessing() {
			return true;
		}
    }
}
