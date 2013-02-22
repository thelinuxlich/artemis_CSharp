#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentPoolMultiThread.cs" company="GAMADU.COM">
//     Copyright © 2013 GAMADU.COM. All rights reserved.
//
//     Redistribution and use in source and binary forms, with or without modification, are
//     permitted provided that the following conditions are met:
//
//        1. Redistributions of source code must retain the above copyright notice, this list of
//           conditions and the following disclaimer.
//
//        2. Redistributions in binary form must reproduce the above copyright notice, this list
//           of conditions and the following disclaimer in the documentation and/or other materials
//           provided with the distribution.
//
//     THIS SOFTWARE IS PROVIDED BY GAMADU.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
//     WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//     FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
//     CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//     CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//     SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//     ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//     ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//     The views and conclusions contained in the software and documentation are those of the
//     authors and should not be interpreted as representing official policies, either expressed
//     or implied, of GAMADU.COM.
// </copyright>
// <summary>
//   A collection that maintains a set of class instances to allow for recycling instances and minimizing the effects
//   of garbage collection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;

    using Artemis.Interface;

    #endregion Using statements

    /// <summary><para>A collection that maintains a set of class instances</para>
    ///   <para>to allow for recycling instances and</para>
    ///   <para>minimizing the effects of garbage collection.</para></summary>
    /// <typeparam name="T">The type of object to store in the Pool. Pools can only hold class types.</typeparam>
    public class ComponentPoolMultiThread<T> : IComponentPool<T>
        where T : ComponentPoolable
    {
        /// <summary>The allocate.</summary>
        private readonly Func<Type, T> allocate;

        /// <summary>The is resize allowed.</summary>
        private readonly bool isResizeAllowed;

        /// <summary>The inner type.</summary>
        private readonly Type innerType;

        /// <summary>The invalid objects.</summary>
        private readonly List<T> invalidObjects;

        /// <summary>The sync.</summary>
        private readonly object sync;

        /// <summary>The items.</summary>
        private T[] items;

        /// <summary>Initializes a new instance of the <see cref="ComponentPoolMultiThread{T}"/> class.</summary>
        /// <param name="initialSize">The initial size.</param>
        /// <param name="resizePool">The resize pool.</param>
        /// <param name="resizes">if set to <see langword="true" /> [resizes].</param>
        /// <param name="allocateFunc">The allocate <see langword="Func" />.</param>
        /// <param name="innerType">Type of the inner.</param>
        /// <exception cref="ArgumentOutOfRangeException">initialSize and resizePool must be at least 1.</exception>
        /// <exception cref="ArgumentNullException">allocateFunc or innerType is null.</exception>
        public ComponentPoolMultiThread(int initialSize, int resizePool, bool resizes, Func<Type, T> allocateFunc, Type innerType)
        {
            this.sync = new object();
            this.invalidObjects = new List<T>();

            // Validate some parameters.
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
                lock (this.sync)
                {
                    index += this.InvalidCount;

                    if (index < this.InvalidCount || index >= this.items.Length)
                    {
                        throw new IndexOutOfRangeException("The index must be less than or equal to ValidCount.");
                    }

                    return this.items[index];
                }
            }
        }

        /// <summary>Cleans up the pool by checking each valid object to ensure it is still actually valid.
        /// Must be called regularly to free returned Objects</summary>
        public void CleanUp()
        {
            lock (this.sync)
            {
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

                    // Clean the object if desired.
                    item.CleanUp();
                    ++this.InvalidCount;
                }

                this.invalidObjects.Clear();
            }
        }

        /// <summary>Returns a new object from the Pool.</summary>
        /// <returns>The next object in the pool if available, null if all instances are valid.</returns>
        /// <exception cref="Exception">Limit Exceeded items.Length, and the pool was set to not resize</exception>
        /// <exception cref="InvalidOperationException">The pool's allocate method returned a null object reference.</exception>
        public T New()
        {
            lock (this.sync)
            {
                // If we're out of invalid instances...
                if (this.InvalidCount == 0)
                {
                    // If we can't resize, then we can't give the user back any instance.
                    if (!this.isResizeAllowed)
                    {
                        throw new Exception("Limit Exceeded " + this.items.Length + ", and the pool was set to not resize.");
                    }

                    // Create a new array with some more slots and copy over the existing items
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

                    // Move the invalid count based on our resize amount.
                    this.InvalidCount += this.ResizeAmount;
                }

                // Decrement the counter.
                --this.InvalidCount;

                // Get the next item in the list.
                T result = this.items[this.InvalidCount];

                // If the item is null, we need to allocate a new instance.
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
        }

        /// <summary>Return an object to the pool</summary>
        /// <param name="item">The item.</param>
        public void ReturnObject(T item)
        {
            lock (this.sync)
            {
                this.invalidObjects.Add(item);
            }
        }
    }
}