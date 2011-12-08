using System;
using System.Collections.Generic;
using System.Numerics;
namespace Artemis
{
	public abstract class EntitySystem {
		private BigInteger systemBit = 0;
	
		private BigInteger typeFlags = 0;
		
		protected bool enabled = true;
	
		protected EntityWorld world;
	
		private Dictionary<int,Entity> actives = new Dictionary<int, Entity>();
		
		public EntitySystem() {
		}
	
		public EntitySystem(params Type[] types) {
			for (int i = 0, j = types.Length; i < j; i++) {
                Type type = types[i];
				ComponentType ct = ComponentTypeManager.GetTypeFor(type);
				typeFlags |= ct.GetBit();
			}
		}
		
		public void SetSystemBit(BigInteger bit) {
			this.systemBit = bit;
		}
		
		/**
		 * Called before processing of entities begins. 
		 */
        protected virtual void Begin() { }
	
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
        protected virtual void End() { }
		
		/**
		 * Any implementing entity system must implement this method and the logic
		 * to process the given entities of the system.
		 * 
		 * @param entities the entities this system contains.
		 */
        protected virtual void ProcessEntities(Dictionary<int, Entity> entities) { }
		
		/**
		 * 
		 * @return true if the system should be processed, false if not.
		 */
        protected virtual bool CheckProcessing() { return enabled; }
	
		/**
		 * Override to implement code that gets executed when systems are initialized.
		 */
        public virtual void Initialize() { }
	
		/**
		 * Called if the system has received a entity it is interested in, e.g. created or a component was added to it.
		 * @param e the entity that was added to this system.
		 */
        public virtual void Added(Entity e) { }
		/**
		 * Called if a entity was removed from this system, e.g. deleted or had one of it's components removed.
		 * @param e the entity that was removed from this system.
		 */
        public virtual void Removed(Entity e) { }
	
		public void Change(Entity e) {
			bool contains = (systemBit & e.GetSystemBits()) == systemBit;
			bool interest = (typeFlags & e.GetTypeBits()) == typeFlags;
	
			if (interest && !contains && typeFlags > 0) {
				actives.Add(e.GetId(),e);
				e.AddSystemBit(systemBit);
				Added(e);
			} else if (!interest && contains && typeFlags > 0) {
				Remove(e);
			}
		}
	
		private void Remove(Entity e) {
			actives.Remove(e.GetId());
			e.RemoveSystemBit(systemBit);
			Removed(e);
		}
	
		public void SetWorld(EntityWorld world) {
			this.world = world;
		}
		
		public void Toggle() {
			enabled = !enabled;
		}
		
		public void Enable() {
			enabled = true;
		}
		
		public void Disable() {
			enabled = false;
		}
		
		/**
		 * Merge together a required type and a array of other types. Used in derived systems.
		 * @param requiredType
		 * @param otherTypes
		 * @return
		 */
		public static Type[] GetMergedTypes(Type requiredType, params Type[] otherTypes) {
			Type[] types = new Type[1+otherTypes.Length];
			types[0] = requiredType;
			for(int i = 0,j = otherTypes.Length; j > i; i++) {
				types[i+1] = otherTypes[i];
			}
			return types;
		}
	
	}
}