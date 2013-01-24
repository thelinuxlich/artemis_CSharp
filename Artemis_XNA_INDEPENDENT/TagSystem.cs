using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis
{

    /// <summary>
    /// Tag System does not fire ANY Events of the EntitySystem
    /// </summary>
    public abstract class TagSystem : EntitySystem
    {
        protected string tag;
        public TagSystem(string tag) {
            System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(tag));
            this.tag = tag;
		}
		
		/**
		 * Process a entity this system is interested in.
		 * @param e the entity to process.
		 */
		public abstract void Process(Entity e);

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            Entity e = world.TagManager.GetEntity(tag);
            if (e != null)
            {
                Process(e);
            }
		}

        public override void OnChange(Entity e)
        {            
        }
    }
}
