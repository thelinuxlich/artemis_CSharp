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
    using Artemis.Manager;

    #endregion Using statements

    /// <summary>Represents a Component Type.</summary>
    public sealed class ComponentType 
    {
        private static BigInteger bit = 1;
        internal static BigInteger nextBit
        {
            get { BigInteger value = bit; bit <<= 1; return value; }
        }
        private static int id = 0;
        internal static int nextId
        {
            get { return id++; }
        }

        internal ComponentType()
        {
            Id = nextId;
            Bit = nextBit;
        }

        /// <summary>
        /// The bitindex that represents this type of component.
        /// </summary>
        public int Id
        {
            get;
            private set;
        }
        /// <summary>
        /// The bit that represents this type of component.
        /// </summary>
        public BigInteger Bit
        {
            get;
            private set;
        }
    }


    internal static class ComponentType<T>
        where T : Artemis.Interface.IComponent
    {
        static ComponentType()
        {
            CType = ComponentTypeManager.GetTypeFor<T>();
            if (CType == null)
            {
                CType = new ComponentType();
                ComponentTypeManager.SetTypeFor<T>(CType);
            }
        }
        public static readonly ComponentType CType;
    }
}