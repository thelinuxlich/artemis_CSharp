using System;
using System.Collections.Generic;
#if !XBOX && !WINDOWS_PHONE
using System.Numerics;
#endif

#if XBOX || WINDOWS_PHONE
using BigInteger = System.Int32;
#endif

namespace Artemis
{
    /// <summary>
    /// Base of all Entity Systems
    /// Provide basic funcionalities
    /// </summary>
	public abstract class EntitySystem {

        protected static BlackBoard blackBoard = new BlackBoard();

        public static BlackBoard BlackBoard
        {
            get
            {
                return blackBoard;
            }
        }

		private BigInteger systemBit = 0;
			
		protected bool enabled = true;
	
		protected EntityWorld world;

        protected Aspect aspect = null; 
	
		private Dictionary<int,Entity> actives = new Dictionary<int, Entity>();
		
		public EntitySystem() {
		}
	
		public EntitySystem(params Type[] types) {
            aspect = Aspect.All(types);
		}

        public EntitySystem(Aspect aspect)
        {
            System.Diagnostics.Debug.Assert(aspect != null);
            this.aspect = aspect;
		}
        
		
		internal BigInteger SystemBit {
			set { systemBit = value; }
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
        public virtual void OnRemoved(Entity e) { }
	
		public virtual void OnEnabled(Entity e) { }
		
		public virtual void OnDisabled(Entity e) { }
		
		public virtual void OnChange(Entity e) {
            System.Diagnostics.Debug.Assert(e != null);
			bool contains = (systemBit & e.SystemBits) == systemBit;
			//bool interest = (typeFlags & e.TypeBits) == typeFlags;
            bool interest = aspect.Interest(e);
	
			if (interest && !contains ) {
				Add(e);
			} else if (!interest && contains ) {
				Remove(e);
			} else if (interest && contains && e.Enabled == true) {
				Enable(e);
			} else if (interest && contains && e.Enabled == false ) {
				Disable(e);
			}
		}

        protected bool Interests(Entity e)
        {
            return aspect.Interest(e);
        }
		
		protected void Add(Entity e) {
            System.Diagnostics.Debug.Assert(e != null);
			e.AddSystemBit(systemBit);
			if (e.Enabled == true) {
				Enable(e);
			}
			Added(e);
		}
	
		protected void Remove(Entity e) {
            System.Diagnostics.Debug.Assert(e != null);
			e.RemoveSystemBit(systemBit);
			if (e.Enabled == true) {
				Disable(e);
			}
			OnRemoved(e);
		}
		
		private void Enable(Entity e) {
            System.Diagnostics.Debug.Assert(e != null);
			if (actives.ContainsKey(e.Id)) {
				return;
			}
			actives.Add(e.Id,e);
			OnEnabled(e);
		}
		
		private void Disable(Entity e) {
            System.Diagnostics.Debug.Assert(e != null);

			if (!actives.ContainsKey(e.Id)) {
				return;
			}
			actives.Remove(e.Id);
			OnDisabled(e);
		}
	
		public EntityWorld World {
			internal set { world = value; }
            get { return world; }
             
		}
		
		public void Toggle() {            
			enabled = !enabled;
		}
		
		public bool Enabled {
            get
            {
                return enabled;
            }
            set
            {
                this.enabled = value;
            }			
		}
		
		
		/**
		 * Merge together a required type and a array of other types. Used in derived systems.
		 * @param requiredType
		 * @param otherTypes
		 * @return
		 */
		public static Type[] GetMergedTypes(Type requiredType, params Type[] otherTypes) {
            System.Diagnostics.Debug.Assert(requiredType != null);
			Type[] types = new Type[1+otherTypes.Length];
			types[0] = requiredType;
			for(int i = 0,j = otherTypes.Length; j > i; i++) {
				types[i+1] = otherTypes[i];
			}
			return types;
		}
	
	}
}