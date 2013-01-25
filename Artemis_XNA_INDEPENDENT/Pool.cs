using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis
{
    /// <summary>
    /// A collection that maintains a set of class instances to allow for recycling
    /// instances and minimizing the effects of garbage collection.
    /// </summary>
    /// <typeparam name="T">The type of object to store in the Pool. Pools can only hold class types.</typeparam>
    public class Pool<T> where T : ComponentPoolable
    {
        // the amount to enlarge the items array if New is called and there are no free items
        private const int resizeAmount = 20;

        // whether or not the pool is allowed to resize
        private bool canResize;

        // the actual items of the pool
        private T[] items;

        private List<T> invalidObjects = new List<T>();

        private Dictionary<int, T> itemDic = new Dictionary<int, T>();
                
        // used for allocating instances of the object
        private readonly Func<Type, T> allocate;

        /// <summary>
        /// Gets the number of valid objects in the pool.
        /// </summary>
        public int ValidCount { get { return items.Length - InvalidCount; } }

        /// <summary>
        /// Gets the number of invalid objects in the pool.
        /// </summary>
        public int InvalidCount { get; private set; }

        /// <summary>
        /// Returns a valid object at the given index. The index must fall in the range of [0, ValidCount].
        /// </summary>
        /// <param name="index">The index of the valid object to get</param>
        /// <returns>A valid object found at the index</returns>
        public T this[int index]
        {
            get
            {
                index += InvalidCount;

                if (index < InvalidCount || index >= items.Length)
                    throw new IndexOutOfRangeException("The index must be less than or equal to ValidCount");

                return items[index];
            }
        }

        /// <summary>
        /// Creates a new pool with a specific starting size.
        /// </summary>
        /// <param name="initialSize">The initial size of the pool.</param>
        /// <param name="resizes">Whether or not the pool is allowed to increase its size as needed.</param>
        /// <param name="validateFunc">A predicate used to determine if a given object is still valid.</param>
        /// <param name="allocateFunc">A function used to allocate an instance for the pool.</param>
        public Pool(int initialSize, bool resizes, Func<Type,T> allocateFunc)
        {
            // validate some parameters
            if (initialSize < 1)
                throw new ArgumentOutOfRangeException("initialSize", "initialSize must be at least 1.");           
            if (allocateFunc == null)
                throw new ArgumentNullException("allocateFunc");

            canResize = resizes;

            // create our items array
            items = new T[initialSize];
            InvalidCount = items.Length;

            // store our delegates           
            allocate = allocateFunc;
        }

        /// <summary>
        /// Return an object to the pool
        /// </summary>
        /// <param name="obj"></param>
        public void ReturnObject(T obj)
        {
            invalidObjects.Add(obj);
        }

        /// <summary>
        /// Cleans up the pool by checking each valid object to ensure it is still actually valid.
        /// Must be called regularly to free returned Objects
        /// </summary>
        public void CleanUp()
        {
            for (int i = 0; i < invalidObjects.Count; i++)
            {
                T obj = invalidObjects[i];
               
                // otherwise if we're not at the start of the invalid objects, we have to move
                // the object to the invalid object section of the array
                if (i != InvalidCount)
                {
                    items[i] = items[InvalidCount];
                    items[InvalidCount] = obj;
                }

                // clean the object if desired
                obj.Cleanup();
                InvalidCount++;
            }

            invalidObjects.Clear();
        }

        /// <summary>
        /// Returns a new object from the Pool.
        /// </summary>
        /// <returns>The next object in the pool if available, null if all instances are valid.</returns>
        public T New()
        {
            // if we're out of invalid instances...
            if (InvalidCount == 0)
            {
                // if we can't resize, then we can't give the user back any instance
                if (!canResize)
                    return default(T);

                //System.Diagnostics.Debug.WriteLine("Resizing pool. Old size: " + items.Length + ". New size: " + (items.Length + resizeAmount));

                // create a new array with some more slots and copy over the existing items
                T[] newItems = new T[items.Length + resizeAmount];
                for (int i = items.Length - 1; i >= 0; i--)
                    newItems[i + resizeAmount] = items[i];
                items = newItems;

                // move the invalid count based on our resize amount
                InvalidCount += resizeAmount;
            }

            // decrement the counter
            InvalidCount--;

            // get the next item in the list
            T obj = items[InvalidCount];

            // if the item is null, we need to allocate a new instance
            if (obj == null)
            {
                obj = allocate(typeof(T));

                if (obj == null)
                    throw new InvalidOperationException("The pool's allocate method returned a null object reference.");

                items[InvalidCount] = obj;
            }

            // initialize the object if a delegate was provided
            obj.Initialize();

            return obj;
        }
    }
}
