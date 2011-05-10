using System;
namespace Artemis
{
	public abstract class EntitySystem {
		private long systemBit;
	
		private long typeFlags;
	
		protected World world;
	
		private Bag<Entity> actives;
		
		public EntitySystem() {
		}
	
		public EntitySystem(params Component[] types) {
			actives = new Bag<Entity>();
	
			foreach (Component type in types) {
				ComponentType ct = ComponentTypeManager.getTypeFor(type);
				typeFlags |= ct.getBit();
			}
		}
		
		protected void setSystemBit(long bit) {
			this.systemBit = bit;
		}
		
		/**
		 * Called before processing of entities begins. 
		 */
		protected void begin() {
			
		}
	
		public sealed void process() {
			if(checkProcessing()) {
				begin();
				processEntities(actives);
				end();
			}
		}
		
		/**
		 * Called after the processing of entities ends.
		 */
		protected void end() {
		}
		
		/**
		 * Any implementing entity system must implement this method and the logic
		 * to process the given entities of the system.
		 * 
		 * @param entities the entities this system contains.
		 */
		protected abstract void processEntities(Bag<Entity> entities);
		
		/**
		 * 
		 * @return true if the system should be processed, false if not.
		 */
		protected abstract boolean checkProcessing();
	
		/**
		 * Override to implement code that gets executed when systems are initialized.
		 */
		protected void initialize() {}
	
		/**
		 * Called if the system has received a entity it is interested in, e.g. created or a component was added to it.
		 * @param e the entity that was added to this system.
		 */
		protected void added(Entity e) {}
	
		/**
		 * Called if a entity was removed from this system, e.g. deleted or had one of it's components removed.
		 * @param e the entity that was removed from this system.
		 */
		protected void removed(Entity e) {}
	
		protected sealed void change(Entity e) {
			boolean contains = (systemBit & e.getSystemBits()) == systemBit;
			boolean interest = (typeFlags & e.getTypeBits()) == typeFlags;
	
			if (interest && !contains && typeFlags > 0) {
				actives.add(e);
				e.addSystemBit(systemBit);
				added(e);
			} else if (!interest && contains && typeFlags > 0) {
				remove(e);
			}
		}
	
		private void remove(Entity e) {
			actives.remove(e);
			e.removeSystemBit(systemBit);
			removed(e);
		}
	
		protected sealed void setWorld(World world) {
			this.world = world;
		}
		
		/**
		 * Merge together a required type and a array of other types. Used in derived systems.
		 * @param requiredType
		 * @param otherTypes
		 * @return
		 */
		protected static Component[] getMergedTypes(Component requiredType, params Component[] otherTypes) {
			Component[] types = new Class[1+otherTypes.length];
			types[0] = requiredType;
			for(int i = 0; otherTypes.length > i; i++) {
				types[i+1] = otherTypes[i];
			}
			return types;
		}
	
	}
}