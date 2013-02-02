namespace Artemis.Utils
{
    #region Using statements

    using global::System.Collections;
    using global::System.Collections.Generic;

    #endregion Using statements

    /// <summary>Class BagEnumerator.</summary>
    /// <typeparam name="T">The <see langword="Type"/> T.</typeparam>
    internal class BagEnumerator<T> : IEnumerator<T>
    {
        /// <summary>The bag.</summary>
        private volatile Bag<T> bag;

        /// <summary>The index.</summary>
        private volatile int index;

        /// <summary>Initializes a new instance of the <see cref="BagEnumerator{T}"/> class.</summary>
        /// <param name="bag">The bag.</param>
        public BagEnumerator(Bag<T> bag)
        {
            this.bag = bag;
            this.Reset();
        }

        /// <summary>Gets the current element in the collection.</summary>
        /// <value>The current element.</value>
        /// <returns>The current element in the collection.</returns>
        T IEnumerator<T>.Current
        {
            get
            {
                return this.bag.Get(this.index);
            }
        }

        /// <summary>Gets the current element in the collection.</summary>
        /// <value>The current.</value>
        /// <returns>The current element in the collection.</returns>
        object IEnumerator.Current
        {
            get
            {
                return this.bag.Get(this.index);
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this.bag = null;
        }

        /// <summary>Advances the enumerator to the next element of the collection.</summary>
        /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
            return ++this.index < this.bag.Size;
        }

        /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
        public void Reset()
        {
            this.index = -1;
        }
    }
}