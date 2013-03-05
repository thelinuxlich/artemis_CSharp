#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemBitManager.cs" company="GAMADU.COM">
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
//   Class SystemBitManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Manager
{
    #region Using statements

    using global::System.Collections.Generic;
    using Artemis.System;

#if !XBOX && !WINDOWS_PHONE  && !PORTABLE
    using global::System.Numerics;
#endif
#if XBOX || WINDOWS_PHONE || PORTABLE
    using BigInteger = global::System.Int32;
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
        /// <returns>The bit flag register for the specified system.</returns>
        public static BigInteger GetBitFor(EntitySystem entitySystem)
        {
            BigInteger bit;
            if (SystemBits.TryGetValue(entitySystem, out bit) == false)
            {
#if WINDOWS_PHONE || XBOX || PORTABLE
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