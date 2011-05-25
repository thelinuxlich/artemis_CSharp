using System;
namespace Artemis
{
	public class Bag<E> where E : class 
    {
		private E[] data;
		private int size = 0;
	
		/**
		 * Constructs an empty Bag with an initial capacity of 64.
		 * 
		 */
		public Bag() {
			data = new E[16];
		}
	
		/**
		 * Constructs an empty Bag with the specified initial capacity.
		 * 
		 * @param capacity
		 *            the initial capacity of Bag
		 */
		public Bag(int capacity) {
			data = new E[capacity];
		}
	
		/**
		 * Removes the element at the specified position in this Bag. does this by
		 * overwriting it was last element then removing last element
		 * 
		 * @param index
		 *            the index of element to be removed
		 * @return element that was removed from the Bag
		 */
		public E Remove(int index) {
			E o = data[index]; // make copy of element to remove so it can be
			// returned
			data[index] = data[--size]; // overwrite item to remove with last
			// element
			data[size] = null; // null last element, so gc can do its work
			return o;
		}
		
		
		/**
		 * Remove and return the last object in the bag.
		 * 
		 * @return the last object in the bag, null if empty.
		 */
		public E RemoveLast() {
			if(size > 0) {
				E o = data[--size];
				data[size] = null;
				return o;
			}
			
			return default(E);
		}
	
		/**
		 * Removes the first occurrence of the specified element from this Bag, if
		 * it is present. If the Bag does not contain the element, it is unchanged.
		 * does this by overwriting it was last element then removing last element
		 * 
		 * @param o
		 *            element to be removed from this list, if present
		 * @return <tt>true</tt> if this list contained the specified element
		 */
		public bool Remove(E o) {
			for (int i = 0; i < size; i++) {
				Object o1 = data[i];
	
				if (o.Equals(o1)) {
					data[i] = data[--size]; // overwrite item to remove with last
					// element
					data[size] = null; // null last element, so gc can do its work
					return true;
				}
			}
	
			return false;
		}
		
		/**
		 * Check if bag contains this element.
		 * 
		 * @param o
		 * @return
		 */
		public bool Contains(E o) {
			for(int i = 0; size > i; i++) {
				if(o.Equals(data[i])) {
					return true;
				}
			}
			return false;
		}
	
		/**
		 * Removes from this Bag all of its elements that are contained in the
		 * specified Bag.
		 * 
		 * @param bag
		 *            Bag containing elements to be removed from this Bag
		 * @return {@code true} if this Bag changed as a result of the call
		 */
		public bool RemoveAll(Bag<E> bag) {
			bool modified = false;
	
			for (int i = 0; i < bag.Size(); i++) {
				Object o1 = bag.Get(i);
	
				for (int j = 0; j < size; j++) {
					Object o2 = data[j];
	
					if (o1 == o2) {
						Remove(j);
						j--;
						modified = true;
						break;
					}
				}
			}
	
			return modified;
		}
	
		/**
		 * Returns the element at the specified position in Bag.
		 * 
		 * @param index
		 *            index of the element to return
		 * @return the element at the specified position in bag
		 */
		public E Get(int index) {
			return data[index];
		}
	
		/**
		 * Returns the number of elements in this bag.
		 * 
		 * @return the number of elements in this bag
		 */
		public int Size() {
			return size;
		}
		
		/**
		 * Returns the number of elements the bag can hold without growing.
		 * 
		 * @return the number of elements the bag can hold without growing.
		 */
		public int GetCapacity() {
			return data.Length;
		}
	
		/**
		 * Returns true if this list contains no elements.
		 * 
		 * @return true if this list contains no elements
		 */
		public bool IsEmpty() {
			return size == 0;
		}
	
		/**
		 * Adds the specified element to the end of this bag. if needed also
		 * increases the capacity of the bag.
		 * 
		 * @param o
		 *            element to be added to this list
		 */
		public void Add(E o) {
			// is size greater than capacity increase capacity
			if (size == data.Length) {
				Grow();
			}
	
			data[size++] = o;
		}
	
		/**
		 * Set element at specified index in the bag.
		 * 
		 * @param index position of element
		 * @param o the element
		 */
		public void Set(int index, E o) {
			if(index >= data.Length) {
				Grow(index*2);
				size = index+1;
			} else if(index >= size) {
				size = index+1;
			}
			data[index] = o;
		}
	
		private void Grow() {
			int newCapacity = (data.Length * 3) / 2 + 1;
			Grow(newCapacity);
		}
		
		private void Grow(int newCapacity) {
			E[] oldData = data;
			data = new E[newCapacity];
			Array.Copy(oldData, 0, data, 0, oldData.Length);
		}
	
		/**
		 * Removes all of the elements from this bag. The bag will be empty after
		 * this call returns.
		 */
		public void Clear() {
			// null all elements so gc can clean up
			for (int i = 0; i < size; i++) {
				data[i] = null;
			}
	
			size = 0;
		}
	
		/**
		 * Add all items into this bag. 
		 * @param added
		 */
		public void AddAll(Bag<E> items) {
			for(int i = 0; items.Size() > i; i++) {
				Add(items.Get(i));
			}
		}
		
	}
}

