using System;
using System.Numerics;
namespace Artemis
{
	public sealed class Entity {
		private int id;
		private long uniqueId;
		private BigInteger typeBits = 0;
		private BigInteger systemBits = 0;
		
		private EntityWorld world;
		private EntityManager entityManager;
        private bool enabled = true;
		private bool refreshingState = false;
		private bool deletingState = false;
		
		public Entity(EntityWorld world, int id) {
			this.world = world;
			this.entityManager = world.EntityManager;
			this.id = id;
		}
		
		/**
		 * The internal id for this entity within the framework. No other entity will have the same ID, but
		 * ID's are however reused so another entity may acquire this ID if the previous entity was deleted.
		 * 
		 * @return id of the entity.
		 */
		public int Id {
			get { return id;}
		}
		
		public long UniqueId {
			get { return uniqueId; }
			set { uniqueId = value;}
		}
		
		public BigInteger TypeBits {
			get { return typeBits; }
			set { typeBits = value; }
		}
		
		public void AddTypeBit(BigInteger bit) {
			typeBits |= bit;
		}
		
		public void RemoveTypeBit(BigInteger bit) {
			typeBits &= ~bit;
		}
		
		public bool RefreshingState {
			get { return refreshingState; }
			set { refreshingState = value; }
		}
		
		public bool DeletingState {
			get { return deletingState; }
			set { deletingState = value; }
		}
		
		public BigInteger SystemBits {
			get { return systemBits;}
			set { systemBits = value; }
		}
		
		public void AddSystemBit(BigInteger bit) {
			systemBits |= bit;
		}
		
		public void RemoveSystemBit(BigInteger bit) {
			systemBits &= ~bit;
		}
		
		public void Reset() {
			systemBits = 0;
			typeBits = 0;
			Enable();
		}
		
		public override String ToString() {
			return "Entity["+id+"]";
		}

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }

		/**
		 * Add a component to this entity.
		 * @param component to add to this entity
		 */
		public void AddComponent(Component component){
			entityManager.AddComponent(this, component);
		}
		
		public void AddComponent<T>(Component component) where T : Component {
			entityManager.AddComponent<T>(this, component);
		}
		
		/**
		 * Removes the component from this entity.
		 * @param component to remove from this entity.
		 */
		public void RemoveComponent<T>(Component component) where T : Component{
			entityManager.RemoveComponent<T>(this, component);
		}
		
		/**
		 * Faster removal of components from a entity.
		 * @param component to remove from this entity.
		 */
		public void RemoveComponent(ComponentType type){
			entityManager.RemoveComponent(this, type);
		}
		
		/**
		 * Checks if the entity has been deleted from somewhere.
		 * @return if it's active.
		 */
		public bool IsActive(){
			return entityManager.IsActive(id);
		}

        public bool Enabled
        {
            get {return enabled;}
        }

		/**
		 * This is the preferred method to use when retrieving a component from a entity. It will provide good performance.
		 * 
		 * @param type in order to retrieve the component fast you must provide a ComponentType instance for the expected component.
		 * @return
		 */
		public Component GetComponent(ComponentType type) {
			return entityManager.GetComponent(this, type);
		}
		
		/**
		 * Slower retrieval of components from this entity. Minimize usage of this, but is fine to use e.g. when creating new entities
		 * and setting data in components.
		 * @param <T> the expected return component type.
		 * @param type the expected return component type.
		 * @return component that matches, or null if none is found.
		 */
		public T GetComponent<T>() where T : Component {
			return (T)GetComponent(ComponentTypeManager.GetTypeFor<T>());
		}
		
		public bool HasComponent<T>() where T : Component {
			return (T)GetComponent(ComponentTypeManager.GetTypeFor<T>()) != null;
		}
		
		/**
		 * Get all components belonging to this entity.
		 * WARNING. Use only for debugging purposes, it is dead slow.
		 * WARNING. The returned bag is only valid until this method is called again, then it is overwritten.
		 * @return all components of this entity.
		 */
		public Bag<Component> GetComponents() {
			return entityManager.GetComponents(this);
		}
		
		/**
		 * Refresh all changes to components for this entity. After adding or removing components, you must call
		 * this method. It will update all relevant systems.
		 * It is typical to call this after adding components to a newly created entity.
		 */
		public void Refresh() {
			if(refreshingState == true) {
				return;
			}
			world.RefreshEntity(this);
			refreshingState = true;
		}
		
		/**
		 * Delete this entity from the world.
		 */
		public void Delete() {
			if(deletingState == true) {
				return;
			}
			world.DeleteEntity(this);
			deletingState = true;
		}
	
		/**
		 * Set the group of the entity. Same as World.setGroup().
		 * @param group of the entity.
		 */
		public void SetGroup(String group) {
			world.GroupManager.Set(group, this);
		}
		
		/**
		 * Assign a tag to this entity. Same as World.setTag().
		 * @param tag of the entity.
		 */
		public void SetTag(String tag) {
			world.TagManager.Register(tag, this);
		}
		
		public String GetTag() {
			return world.TagManager.GetTagOfEntity(this);
		}
	}
}

