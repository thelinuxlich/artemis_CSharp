#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentPool.cs" company="GAMADU.COM">
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

        /// <summary>The invalid components.</summary>
        private readonly List<T> invalidComponents;

        /// <summary>The actual components of the pool.</summary>
        private T[] components;

        /// <summary>Initializes a new instance of the <see cref="ComponentPool{T}"/> class.</summary>
        /// <param name="initialSize">The initial size of the pool.</param>
        /// <param name="resizePool">The resize pool size.</param>
        /// <param name="resizes">Whether or not the pool is allowed to increase its size as needed.</param>
        /// <param name="allocateFunc">A function used to allocate an instance for the pool.</param>
        /// <param name="innerType">Type ComponentPool-able.</param>
        /// <exception cref="ArgumentOutOfRangeException">InitialSize must be at least 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">ResizePool must be at least 1.</exception>
        /// <exception cref="ArgumentNullException">AllocateFunc must not be null.</exception>
        /// <exception cref="ArgumentNullException">InnerType must not be null.</exception>
        public ComponentPool(int initialSize, int resizePool, bool resizes, Func<Type, T> allocateFunc, Type innerType)
        {
            this.invalidComponents = new List<T>();

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

            // Create our components array.
            this.components = new T[initialSize];
            this.InvalidCount = this.components.Length;

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
                return this.components.Length - this.InvalidCount;
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

                if (index < this.InvalidCount || index >= this.components.Length)
                {
                    throw new ArgumentOutOfRangeException("index", "The index must be less than or equal to ValidCount.");
                }

                return this.components[index];
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
            foreach (T component in this.invalidComponents)
            {
                // otherwise if we're not at the start of the invalid objects, we have to move
                // the object to the invalid object section of the array
                if (component.PoolId != this.InvalidCount)
                {
                    this.components[component.PoolId] = this.components[this.InvalidCount];
                    this.components[this.InvalidCount].PoolId = component.PoolId;
                    this.components[this.InvalidCount] = component;
                    component.PoolId = -1;
                }

                // clean the object if desired
                component.CleanUp();
                ++this.InvalidCount;
            }

            this.invalidComponents.Clear();
        }

        /// <summary>Returns a new object from the Pool.</summary>
        /// <returns>The next object in the pool if available, null if all instances are valid.</returns>
        /// <exception cref="Exception">Limit Exceeded components.Length and the pool was set to not resize.</exception>
        /// <exception cref="InvalidOperationException">The pool's allocate method returned a null object reference.</exception>
        public T New()
        {
            // If we're out of invalid instances...
            if (this.InvalidCount == 0)
            {
                // If we can't resize, then we can not give the user back any instance.
                if (!this.isResizeAllowed)
                {
                    throw new Exception("Limit Exceeded " + this.components.Length + ", and the pool was set to not resize.");
                }

                // Create a new array with some more slots and copy over the existing components.
                T[] newComponents = new T[this.components.Length + this.ResizeAmount];

                for (int index = this.components.Length - 1; index >= 0; --index)
                {
                    if (index >= this.InvalidCount)
                    {
                        this.components[index].PoolId = index + this.ResizeAmount;
                    }

                    newComponents[index + this.ResizeAmount] = this.components[index];
                }

                this.components = newComponents;

                // move the invalid count based on our resize amount
                this.InvalidCount += this.ResizeAmount;
            }

            // decrement the counter
            --this.InvalidCount;

            // get the next component in the list
            T result = this.components[this.InvalidCount];

            // if the component is null, we need to allocate a new instance
            if (result == null)
            {
                result = this.allocate(this.innerType);

                if (result == null)
                {
                    throw new InvalidOperationException("The pool's allocate method returned a null object reference.");
                }

                this.components[this.InvalidCount] = result;
            }

            result.PoolId = this.InvalidCount;

            // Initialize the object if a delegate was provided.
            result.Initialize();

            return result;
        }

        /// <summary>Returns the object.</summary>
        /// <param name="component">The component.</param>
        public void ReturnObject(T component)
        {
            this.invalidComponents.Add(component);
        }
    }
}