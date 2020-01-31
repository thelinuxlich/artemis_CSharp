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

    using Artemis.Interface;

    #endregion Using statements

    /// <summary><para>A collection that maintains a set of class instances</para>
    ///   <para>to allow for recycling instances and</para>
    ///   <para>minimizing the effects of garbage collection.</para></summary>
    /// <typeparam name="T">The type of object to store in the Pool. Pools can only hold class types.</typeparam>
    public class ComponentPoolMultiThread<T> : IComponentPool<T>
        where T : ComponentPoolable
    {
        /// <summary>The pool.</summary>
        private readonly ComponentPool<T> pool;
        
        /// <summary>The sync.</summary>
        private readonly object sync;

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
            this.pool = new ComponentPool<T>(initialSize, resizePool, resizes, allocateFunc, innerType);
            this.sync = new object();
        }

        /// <summary>Gets the number of invalid objects in the pool.</summary>
        /// <value>The invalid count.</value>
        public int InvalidCount
        {
            get
            {
                return this.pool.InvalidCount;
            }
        }

        /// <summary>Gets the resize amount.</summary>
        /// <value>The resize amount.</value>
        public int ResizeAmount
        {
            get
            {
                return this.pool.ResizeAmount;
            }
        }

        /// <summary>Gets the number of valid objects in the pool.</summary>
        /// <value>The valid count.</value>
        public int ValidCount
        {
            get
            {
                return this.pool.ValidCount;
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
                    return this.pool[index];
                }
            }
        }

        /// <summary>Cleans up the pool by checking each valid object to ensure it is still actually valid.
        /// Must be called regularly to free returned Objects</summary>
        public void CleanUp()
        {
            lock (this.sync)
            {
                this.pool.CleanUp();
            }
        }

        /// <summary>Returns a new object from the Pool.</summary>
        /// <returns>The next object in the pool if available, null if all instances are valid.</returns>
        /// <exception cref="Exception">Limit Exceeded components.Length, and the pool was set to not resize</exception>
        /// <exception cref="InvalidOperationException">The pool's allocate method returned a null object reference.</exception>
        public T New()
        {
            lock (this.sync)
            {
                return this.pool.New();
            }
        }

        /// <summary>Return an object to the pool</summary>
        /// <param name="component">The component.</param>
        public void ReturnObject(T component)
        {
            lock (this.sync)
            {
                this.pool.ReturnObject(component);
            }
        }
    }
}