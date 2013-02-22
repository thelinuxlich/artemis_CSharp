namespace Artemis
{
    #region Using statements

    using Artemis.Interface;

    using global::System;
    using global::System.Collections.Generic;

    #endregion Using statements

    /// <summary>
    /// <para>A collection that maintains a set of class instances</para>
    /// <para>to allow for recycling instances and</para>
    /// <para>minimizing the effects of garbage collection.</para>
    /// </summary>
    /// <typeparam name="T">The type of object to store in the Pool. Pools can only hold class types.</typeparam>
    public class ComponentPool<T> : IComponentPool<T>
        where T : ComponentPoolable
    {
        /// <summary>The allocate.</summary>
        private readonly Func<Type, T> allocate;

        /// <summary>The is resize allowed indicates whether or not the pool is allowed to resize.</summary>
        private readonly bool isResizeAllowed;

        /// <summary>The inner type.</summary>
        private readonly Type innerType;

        /// <summary>The invalid objects.</summary>
        private readonly List<T> invalidObjects;

        /// <summary>The actual items of the pool.</summary>
        private T[] items;

        /// <summary>Creates a new pool with a specific starting size.</summary>
        /// <param name="initialSize">The initial size of the pool.</param>
        /// <param name="resizePool">The resize pool size.</param>
        /// <param name="resizes">Whether or not the pool is allowed to increase its size as needed.</param>
        /// <param name="allocateFunc">A function used to allocate an instance for the pool.</param>
        /// <param name="innerType">Type ComponentPoolable.</param>
        /// <exception cref="ArgumentOutOfRangeException">InitialSize must be at least 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">ResizePool must be at least 1.</exception>
        /// <exception cref="ArgumentNullException">AllocateFunc must not be null.</exception>
        /// <exception cref="ArgumentNullException">InnerType must not be null.</exception>
        public ComponentPool(int initialSize, int resizePool, bool resizes, Func<Type, T> allocateFunc, Type innerType)
        {
            this.invalidObjects = new List<T>();

            // validate some parameters
            if (initialSize < 1)
            {
                throw new ArgumentOutOfRangeException("initialSize", "initialSize must be at least 1.");
            }

            if (resizePool < 1)
            {
                throw new ArgumentOutOfRangeException("resizePool", "resizePool must be at least 1.");
            }

            if (allocateFunc == null)
            {
                throw new ArgumentNullException("allocateFunc");
            }

            if (innerType == null)
            {
                throw new ArgumentNullException("innerType");
            }

            this.innerType = innerType;
            this.isResizeAllowed = resizes;
            this.ResizeAmount = resizePool;

            // Create our items array.
            this.items = new T[initialSize];
            this.InvalidCount = this.items.Length;

            // Store our delegates.         
            this.allocate = allocateFunc;
        }

        /// <summary>Gets the number of invalid objects in the pool.</summary>
        /// <value>The invalid count.</value>
        public int InvalidCount { get; private set; }

        /// <summary>Gets the resize amount.</summary>
        /// <value>The resize amount.</value>
        public int ResizeAmount { get; internal set; }

        /// <summary>Gets the number of valid objects in the pool.</summary>
        /// <value>The valid count.</value>
        public int ValidCount
        {
            get
            {
                return this.items.Length - this.InvalidCount;
            }
        }

        /// <summary>Returns a valid object at the given index. The index must fall in the range of [0, ValidCount].</summary>
        /// <param name="index">The index.</param>
        /// <returns>A valid object found at the index</returns>
        /// <exception cref="IndexOutOfRangeException">The index must be less than or equal to ValidCount.</exception>
        public T this[int index]
        {
            get
            {
                index += this.InvalidCount;

                if (index < this.InvalidCount || index >= this.items.Length)
                {
                    throw new IndexOutOfRangeException("The index must be less than or equal to ValidCount");
                }

                return this.items[index];
            }
        }

        /// <summary>
        /// <para>Cleans up the pool by checking each valid object</para>
        /// <para>to ensure it is still actually valid.</para>
        /// <para>Must be called regularly to free returned Objects.</para>
        /// </summary>
        public void CleanUp()
        {
            ///////////30////////////50
            foreach (T item in this.invalidObjects)
            {
                // otherwise if we're not at the start of the invalid objects, we have to move
                // the object to the invalid object section of the array
                if (item.PoolId != this.InvalidCount)
                {
                    this.items[item.PoolId] = this.items[this.InvalidCount];
                    this.items[this.InvalidCount].PoolId = item.PoolId;
                    this.items[this.InvalidCount] = item;
                    item.PoolId = -1;
                }

                // clean the object if desired
                item.CleanUp();
                ++this.InvalidCount;
            }

            this.invalidObjects.Clear();
        }

        /// <summary>Returns a new object from the Pool.</summary>
        /// <returns>The next object in the pool if available, null if all instances are valid.</returns>
        /// <exception cref="Exception">Limit Exceeded items.Length and the pool was set to not resize.</exception>
        /// <exception cref="InvalidOperationException">The pool's allocate method returned a null object reference.</exception>
        public T New()
        {
            // If we're out of invalid instances...
            if (this.InvalidCount == 0)
            {
                // If we can't resize, then we can not give the user back any instance.
                if (!this.isResizeAllowed)
                {
                    throw new Exception("Limit Exceeded " + this.items.Length + ", and the pool was set to not resize.");
                }

                // Create a new array with some more slots and copy over the existing items.
                T[] newItems = new T[this.items.Length + this.ResizeAmount];

                for (int index = this.items.Length - 1; index >= 0; --index)
                {
                    if (index >= this.InvalidCount)
                    {
                        this.items[index].PoolId = index + this.ResizeAmount;
                    }
                    newItems[index + this.ResizeAmount] = this.items[index];
                }
                this.items = newItems;

                // move the invalid count based on our resize amount
                this.InvalidCount += this.ResizeAmount;
            }

            // decrement the counter
            --this.InvalidCount;

            // get the next item in the list
            T result = this.items[this.InvalidCount];

            // if the item is null, we need to allocate a new instance
            if (result == null)
            {
                result = this.allocate(this.innerType);

                if (result == null)
                {
                    throw new InvalidOperationException("The pool's allocate method returned a null object reference.");
                }

                this.items[this.InvalidCount] = result;
            }

            result.PoolId = this.InvalidCount;

            // Initialize the object if a delegate was provided.
            result.Initialize();

            return result;
        }

        /// <summary>Returns the object.</summary>
        /// <param name="item">The item.</param>
        public void ReturnObject(T item)
        {
            this.invalidObjects.Add(item);
        }
    }
}