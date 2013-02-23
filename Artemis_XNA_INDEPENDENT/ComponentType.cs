#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentType.cs" company="GAMADU.COM">
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
//   Represents a Component Type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis
{
    #region Using statements

#if !XBOX && !WINDOWS_PHONE
    using global::System.Numerics;
#endif
#if XBOX || WINDOWS_PHONE
    using BigInteger = global::System.Int32;
#endif

    #endregion Using statements

    /// <summary>Represents a Component Type.</summary>
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