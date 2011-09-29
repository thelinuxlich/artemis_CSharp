using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis
{
    public abstract class TagSystem : EntitySystem
    {
        protected string tag;
        public TagSystem(string tag) {
            this.tag = tag;
		}
		
		/**
		 * Process a entity this system is interested in.
		 * @param e the entity to process.
		 */
		public abstract void Process(Entity e);

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            Entity e = this.world.GetTagManager().GetEntity(tag);
            if (e != null)
            {
                Process(e);
            }
		}
    }
}
