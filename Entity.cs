using System;
namespace Artemis
{
	public sealed class Entity {
		private int id;
		private long uniqueId;
		private long typeBits;
		private long systemBits;
		
		private World world;
		private EntityManager entityManager;
		
		protected Entity(World world, int id) {
			this.world = world;
			this.entityManager = world.getEntityManager();
			this.id = id;
		}
	
		/**
		 * The internal id for this entity within the framework. No other entity will have the same ID, but
		 * ID's are however reused so another entity may acquire this ID if the previous entity was deleted.
		 * 
		 * @return id of the entity.
		 */
		public int GetId() {
			return id;
		}
		
		protected void SetUniqueId(long uniqueId) {
			this.uniqueId = uniqueId;
		}
		
		/**
		 * Get the unique ID of this entity. Because entity instances are reused internally use this to identify between different instances.
		 * @return the unique id of this entity.
		 */
		public long GetUniqueId() {
			return uniqueId;
		}
		
		protected long GetTypeBits() {
			return typeBits;
		}
		
		protected void AddTypeBit(long bit) {
			typeBits |= bit;
		}
		
		protected void RemoveTypeBit(long bit) {
			typeBits &= ~bit;
		}
		
		protected long GetSystemBits() {
			return systemBits;
		}
		
		protected void AddSystemBit(long bit) {
			systemBits |= bit;
		}
		
		protected void RemoveSystemBit(long bit) {
			systemBits &= ~bit;
		}
		
		protected void SetSystemBits(long systemBits) {
			this.systemBits = systemBits;
		}
		
		protected void SetTypeBits(long typeBits) {
			this.typeBits = typeBits;
		}
		
		protected void Reset() {
			systemBits = 0;
			typeBits = 0;
		}
		
		public String ToString() {
			return "Entity["+id+"]";
		}
		
		/**
		 * Add a component to this entity.
		 * @param component to add to this entity
		 */
		public void AddComponent(Component component){
			entityManager.AddComponent(this, component);
		}
		
		/**
		 * Removes the component from this entity.
		 * @param component to remove from this entity.
		 */
		public void RemoveComponent(Component component){
			entityManager.RemoveComponent(this, component);
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
		public boolean IsActive(){
			return entityManager.IsActive(id);
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
			return GetComponent(ComponentTypeManager.GetTypeFor<T>());
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
			world.RefreshEntity(this);
		}
		
		/**
		 * Delete this entity from the world.
		 */
		public void Delete() {
			world.DeleteEntity(this);
		}
	
		/**
		 * Set the group of the entity. Same as World.setGroup().
		 * @param group of the entity.
		 */
		public void SetGroup(String group) {
			world.GetGroupManager().set(group, this);
		}
		
		/**
		 * Assign a tag to this entity. Same as World.setTag().
		 * @param tag of the entity.
		 */
		public void SetTag(String tag) {
			world.GetTagManager().Register(tag, this);
		}
		
	}
}

