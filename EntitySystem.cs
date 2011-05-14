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
				ComponentType ct = ComponentTypeManager.GetTypeFor(type);
				typeFlags |= ct.GetBit();
			}
		}
		
		public void SetSystemBit(long bit) {
			this.systemBit = bit;
		}
		
		/**
		 * Called before processing of entities begins. 
		 */
		public void Begin() {
			
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
		public void End() {
		}
		
		/**
		 * Any implementing entity system must implement this method and the logic
		 * to process the given entities of the system.
		 * 
		 * @param entities the entities this system contains.
		 */
		public abstract void ProcessEntities(Bag<Entity> entities);
		
		/**
		 * 
		 * @return true if the system should be processed, false if not.
		 */
		public virtual bool CheckProcessing();
	
		/**
		 * Override to implement code that gets executed when systems are initialized.
		 */
		public void Initialize() {}
	
		/**
		 * Called if the system has received a entity it is interested in, e.g. created or a component was added to it.
		 * @param e the entity that was added to this system.
		 */
		public void Added(Entity e) {}
	
		/**
		 * Called if a entity was removed from this system, e.g. deleted or had one of it's components removed.
		 * @param e the entity that was removed from this system.
		 */
		public void Removed(Entity e) {}
	
		public sealed void Change(Entity e) {
			bool contains = (systemBit & e.GetSystemBits()) == systemBit;
			bool interest = (typeFlags & e.GetTypeBits()) == typeFlags;
	
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
	
		public sealed void SetWorld(World world) {
			this.world = world;
		}
		
		/**
		 * Merge together a required type and a array of other types. Used in derived systems.
		 * @param requiredType
		 * @param otherTypes
		 * @return
		 */
		public static Component[] GetMergedTypes(Component requiredType, params Component[] otherTypes) {
			Component[] types = new Component[1+otherTypes.Length];
			types[0] = requiredType;
			for(int i = 0; otherTypes.Length > i; i++) {
				types[i+1] = otherTypes[i];
			}
			return types;
		}
	
	}
}