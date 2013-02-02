namespace Artemis
{
    #region Using statements

#if !XBOX && !WINDOWS_PHONE
    using global::System.Numerics;
#endif
#if XBOX || WINDOWS_PHONE
    using BigInteger = System.Int32;
#endif

    #endregion Using statements

    /// <summary>Represents a Component Type</summary>
    public sealed class ComponentType
    {
        /// <summary>The next bit.</summary>
        private static BigInteger nextBit = 1;

        /// <summary>The next id.</summary>
        private static int nextId;

        /// <summary>Initializes a new instance of the <see cref="ComponentType"/> class.</summary>
        public ComponentType()
        {
            this.Bit = nextBit;
            nextBit <<= 1;
            this.Id = nextId;
            ++nextId;
        }

        /// <summary>Gets the unique bit representation of a type.</summary>
        /// <value>The bit.</value>
        public BigInteger Bit { get; private set; }

        /// <summary>Gets the unique integer representing a type.</summary>
        /// <value>The id.</value>
        public int Id { get; private set; }
    }
}