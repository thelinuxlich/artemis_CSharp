namespace Artemis.Manager
{
    #region Using statements

    using Artemis.System;

    using global::System.Collections.Generic;
#if !XBOX && !WINDOWS_PHONE
    using global::System.Numerics;
#endif
#if XBOX || WINDOWS_PHONE
    using BigInteger = System.Int32;
#endif

    #endregion Using statements

    /// <summary>Class SystemBitManager.</summary>
    internal static class SystemBitManager
    {
        /// <summary>The system bits.</summary>
        private static readonly Dictionary<EntitySystem, BigInteger> SystemBits = new Dictionary<EntitySystem, BigInteger>();

        /// <summary>The position.</summary>
        private static int position;

        /// <summary>Gets the bit-register for the specified entity system.</summary>
        /// <param name="entitySystem">The entity system.</param>
        /// <returns>BigInteger.</returns>
        public static BigInteger GetBitFor(EntitySystem entitySystem)
        {
            BigInteger bit;
            if (SystemBits.TryGetValue(entitySystem, out bit) == false)
            {
#if WINDOWS_PHONE || XBOX
                bit = 1 << position;
#else
                bit = 1L << position;
#endif
                ++position;
                SystemBits.Add(entitySystem, bit);
            }

            return bit;
        }
    }
}