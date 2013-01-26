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
    public class MultiThreadComponentPool<T> : Artemis.IComponentPool<T> where T : ComponentPoolable
    {
        private object sync = new object();
        // the amount to enlarge the items array if New is called and there are no free items
        public int ResizeAmount
        {
            get;
            internal set;
        }

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

        private Type innerType;

        /// <summary>
        /// Returns a valid object at the given index. The index must fall in the range of [0, ValidCount].
        /// </summary>
        /// <param name="index">The index of the valid object to get</param>
        /// <returns>A valid object found at the index</returns>
        public T this[int index]
        {
            get
            {
                lock (sync)
                {
                    index += InvalidCount;

                    if (index < InvalidCount || index >= items.Length)
                        throw new IndexOutOfRangeException("The index must be less than or equal to ValidCount");

                    return items[index];
                }
            }
        }

        /// <summary>
        /// Creates a new pool with a specific starting size.
        /// </summary>
        /// <param name="initialSize">The initial size of the pool.</param>
        /// <param name="resizePool">The resize pool size.</param>
        /// <param name="resizes">Whether or not the pool is allowed to increase its size as needed.</param>
        /// <param name="allocateFunc">A function used to allocate an instance for the pool.</param>
        /// <param name="innerType">Type ComponentPoolable.</param>
        /// <exception cref="ArgumentOutOfRangeException">initialSize;initialSize must be at least 1.</exception>
        /// <exception cref="ArgumentNullException">allocateFunc</exception>
        internal MultiThreadComponentPool(int initialSize, int resizePool, bool resizes, Func<Type, T> allocateFunc, Type innerType)
        {
            // validate some parameters
            if (initialSize < 1)
                throw new ArgumentOutOfRangeException("initialSize", "initialSize must be at least 1.");

            if (resizePool < 1)
                throw new ArgumentOutOfRangeException("resizePool", "resizePool must be at least 1.");           

            if (allocateFunc == null)
                throw new ArgumentNullException("allocateFunc");

            if (innerType == null)
                throw new ArgumentNullException("innerType");

            this.innerType = innerType;
            canResize = resizes;
            this.ResizeAmount = resizePool;

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
            lock(sync)
                invalidObjects.Add(obj);
        }

        /// <summary>
        /// Cleans up the pool by checking each valid object to ensure it is still actually valid.
        /// Must be called regularly to free returned Objects
        /// </summary>
        public void CleanUp()
        {

            lock (sync)
            {

                for (int i = 0; i < invalidObjects.Count; i++)
                {
                    T obj = invalidObjects[i];

                    // otherwise if we're not at the start of the invalid objects, we have to move
                    // the object to the invalid object section of the array
                    if (obj.poolId != InvalidCount)
                    {
                        items[obj.poolId] = items[InvalidCount];
                        items[InvalidCount].poolId = obj.poolId;
                        items[InvalidCount] = obj;
                        obj.poolId = -1;
                    }

                    // clean the object if desired
                    obj.Cleanup();
                    InvalidCount++;
                }

                invalidObjects.Clear();
            }
        }

        /// <summary>
        /// Returns a new object from the Pool.
        /// </summary>
        /// <returns>The next object in the pool if available, null if all instances are valid.</returns>
        public T New()
        {
            lock (sync)
            {
                // if we're out of invalid instances...
                if (InvalidCount == 0)
                {
                    // if we can't resize, then we can't give the user back any instance
                    if (!canResize)
                        throw new Exception("Limit Exceeded " + this.items.Length + " , and the pool was set to not resize");

                    // create a new array with some more slots and copy over the existing items
                    T[] newItems = new T[items.Length + ResizeAmount];

                    for (int i = items.Length - 1; i >= 0; i--)
                    {
                        if (i >= InvalidCount)
                        {
                            items[i].poolId = i + ResizeAmount;
                        }
                        newItems[i + ResizeAmount] = items[i];

                    }
                    items = newItems;

                    // move the invalid count based on our resize amount
                    InvalidCount += ResizeAmount;
                }

                // decrement the counter
                InvalidCount--;

                // get the next item in the list
                T obj = items[InvalidCount];

                // if the item is null, we need to allocate a new instance
                if (obj == null)
                {
                    obj = allocate(innerType);

                    if (obj == null)
                        throw new InvalidOperationException("The pool's allocate method returned a null object reference.");

                    items[InvalidCount] = obj;
                }

                obj.poolId = InvalidCount;
                // initialize the object if a delegate was provided
                obj.Initialize();

                return obj;
            }
        }
    }
}
