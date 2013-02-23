#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bag.cs" company="GAMADU.COM">
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
//   Class Bag.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Utils
{
    #region Using statements

    using global::System;
    using global::System.Collections;
    using global::System.Collections.Generic;

    #endregion Using statements

    /// <summary>Class Bag.</summary>
    /// <typeparam name="T">The <see langword="Type"/> T.</typeparam>
    public class Bag<T> : IEnumerable<T>
    {
        /// <summary>The data.</summary>
        private T[] data;

        /// <summary>Initializes a new instance of the <see cref="Bag{T}"/> class.</summary>
        public Bag()
        {
            this.data = new T[16];
        }

        /// <summary>Initializes a new instance of the <see cref="Bag{T}"/> class.</summary>
        /// <param name="capacity">The capacity.</param>
        public Bag(int capacity)
        {
            this.data = new T[capacity];
        }

        /// <summary>Gets the capacity.</summary>
        /// <value>The capacity.</value>
        public int Capacity
        {
            get
            {
                return this.data.Length;
            }
        }

        /// <summary>Gets a value indicating whether this instance is empty.</summary>
        /// <value><see langword="true" /> if this instance is empty; otherwise, <see langword="false" />.</value>
        public bool IsEmpty
        {
            get
            {
                return this.Size == 0;
            }
        }

        /// <summary>Gets the size.</summary>
        /// <value>The size.</value>
        public int Size { get; private set; }

        /// <summary>Returns the element at the specified position in Bag.</summary>
        /// <param name="index">The index.</param>
        /// <returns>the element at the specified position in Bag</returns>
        public T this[int index]
        {
            get
            {
                return this.data[index];
            }

            set
            {
                if (index >= this.data.Length)
                {
                    T[] oldData = this.data;
                    this.data = new T[index * 2];
                    Array.Copy(oldData, 0, this.data, 0, oldData.Length);

                    this.Size = index + 1;
                }
                else if (index >= this.Size)
                {
                    this.Size = index + 1;
                }

                this.data[index] = value;
            }
        }

        /// <summary>Adds the specified item.</summary>
        /// <param name="item">The item.</param>
        public void Add(T item)
        {
            // is size greater than capacity increase capacity
            if (this.Size == this.data.Length)
            {
                this.Grow();
            }

            this.data[this.Size] = item;
            ++this.Size;
        }

        /// <summary>Adds all.</summary>
        /// <param name="items">The items.</param>
        public void AddAll(Bag<T> items)
        {
            for (int index = 0, j = items.Size; j > index; ++index)
            {
                if (this.Size == this.data.Length)
                {
                    this.Grow();
                }

                this.data[this.Size] = items.Get(index);
                ++this.Size;
            }
        }

        /// <summary>Clears this instance.</summary>
        public void Clear()
        {
            // null all elements so gc can clean up
            for (int index = 0; index < this.Size; ++index)
            {
                this.data[index] = default(T);
            }

            this.Size = 0;
        }

        /// <summary>Determines whether [contains] [the specified item].</summary>
        /// <param name="item">The item.</param>
        /// <returns><see langword="true" /> if [contains] [the specified item]; otherwise, <see langword="false" />.</returns>
        public bool Contains(T item)
        {
            for (int index = 0; this.Size > index; ++index)
            {
                if (item.Equals(this.data[index]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Gets the specified index.</summary>
        /// <param name="index">The index.</param>
        /// <returns>The specified element.</returns>
        public T Get(int index)
        {
            return this.data[index];
        }

        /// <summary>Removes the specified index.</summary>
        /// <param name="index">The index.</param>
        /// <returns>The removed element.</returns>
        public T Remove(int index)
        {
            // Make copy of element to remove so it can be returned.
            T result = this.data[index];
            --this.Size;
            
            // Overwrite item to remove with last element.
            this.data[index] = this.data[this.Size];

            // Null last element, so gc can do its work.
            this.data[this.Size] = default(T);
            return result;
        }

        /// <summary>Removes the specified item.</summary>
        /// <param name="item">The item.</param>
        /// <returns><see langword="true" /> if XXXX, <see langword="false" /> otherwise</returns>
        public bool Remove(T item)
        {
            for (int index = 0; index < this.Size; ++index)
            {
                if (item.Equals(this.data[index]))
                {
                    --this.Size;

                    // Overwrite item to remove with last element.
                    this.data[index] = this.data[this.Size];
                    this.data[this.Size] = default(T);

                    return true;
                }
            }

            return false;
        }

        /// <summary>Removes all.</summary>
        /// <param name="bag">The bag.</param>
        /// <returns><see langword="true" /> if XXXX, <see langword="false" /> otherwise</returns>
        public bool RemoveAll(Bag<T> bag)
        {
            bool modified = false;

            for (int index = bag.Size - 1; index >= 0; --index)
            {
                if (this.Remove(bag.Get(index)))
                {
                    modified = true;
                }
            }

            return modified;
        }

        /// <summary>Removes the last.</summary>
        /// <returns>The last element.</returns>
        public T RemoveLast()
        {
            if (this.Size > 0)
            {
                --this.Size;
                T result = this.data[this.Size];

                // default(E) if class = null.
                this.data[this.Size] = default(T);
                return result;
            }

            return default(T);
        }

        /// <summary>Sets the specified index.</summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Set(int index, T item)
        {
            if (index >= this.data.Length)
            {
                T[] oldData = this.data;
                this.data = new T[index * 2];
                Array.Copy(oldData, 0, this.data, 0, oldData.Length);

                this.Size = index + 1;
            }
            else if (index >= this.Size)
            {
                this.Size = index + 1;
            }

            this.data[index] = item;
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1" /> object that can be used to iterate through the collection.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new BagEnumerator<T>(this);
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new BagEnumerator<T>(this);
        }

        /// <summary>Grows this instance.</summary>
        private void Grow()
        {
            this.Grow(((this.data.Length * 3) / 2) + 1);
        }

        /// <summary>Grows the specified new capacity.</summary>
        /// <param name="newCapacity">The new capacity.</param>
        private void Grow(int newCapacity)
        {
            T[] oldData = this.data;
            this.data = new T[newCapacity];
            Array.Copy(oldData, 0, this.data, 0, oldData.Length);
        }
    }
}