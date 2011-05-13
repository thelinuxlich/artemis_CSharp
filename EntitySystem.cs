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
	
		public EntitySystem(params Type[] types) {
			actives = new Bag<Entity>();
	
			foreach (Component type in types) {
				ComponentType ct = ComponentTypeManager.GetTypeFor<type>();
				typeFlags |= ct.GetBit();
			}
		}
		
		protected void SetSystemBit(long bit) {
			this.systemBit = bit;
		}
		
		/**
		 * Called before processing of entities begins. 
		 */
		protected void Begin() {
			
		}
	
		public virtual void Process() {
			if(CheckProcessing()) {
				Begin();
				ProcessEntities(actives);
				End();
			}
		}
		
		/**
		 * Called after the processing of entities ends.
		 */
		protected void End() {
		}
		
		/**
		 * Any implementing entity system must implement this method and the logic
		 * to process the given entities of the system.
		 * 
		 * @param entities the entities this system contains.
		 */
		protected abstract void ProcessEntities(Bag<Entity> entities);
		
		/**
		 * 
		 * @return true if the system should be processed, false if not.
		 */
		protected virtual boolean CheckProcessing();
	
		/**
		 * Override to implement code that gets executed when systems are initialized.
		 */
		protected void Initialize() {}
	
		/**
		 * Called if the system has received a entity it is interested in, e.g. created or a component was added to it.
		 * @param e the entity that was added to this system.
		 */
		protected void Added(Entity e) {}
	
		/**
		 * Called if a entity was removed from this system, e.g. deleted or had one of it's components removed.
		 * @param e the entity that was removed from this system.
		 */
		protected void Removed(Entity e) {}
	
		protected sealed void Change(Entity e) {
			boolean contains = (systemBit & e.GetSystemBits()) == systemBit;
			boolean interest = (typeFlags & e.GetTypeBits()) == typeFlags;
	
			if (interest && !contains && typeFlags > 0) {
				actives.Add(e);
				e.AddSystemBit(systemBit);
				Added(e);
			} else if (!interest && contains && typeFlags > 0) {
				Remove(e);
			}
		}
	
		private void Remove(Entity e) {
			actives.Remove(e);
			e.RemoveSystemBit(systemBit);
			Removed(e);
		}
	
		protected sealed void SetWorld(World world) {
			this.world = world;
		}
		
		/**
		 * Merge together a required type and a array of other types. Used in derived systems.
		 * @param requiredType
		 * @param otherTypes
		 * @return
		 */
		protected static Component[] GetMergedTypes(Type requiredType, params Type[] otherTypes) {
			Component[] types = new Class[1+otherTypes.length];
			types[0] = requiredType;
			for(int i = 0; otherTypes.length > i; i++) {
				types[i+1] = otherTypes[i];
			}
			return types;
		}
	
	}
}