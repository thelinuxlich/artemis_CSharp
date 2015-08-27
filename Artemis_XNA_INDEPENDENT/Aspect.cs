#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Aspect.cs" company="GAMADU.COM">
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
//   Specify a Filter class to filter what Entities (with what Components) a EntitySystem will Process.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis
{
    #region Using statements

    using global::System;
    using global::System.Diagnostics;
    using global::System.Linq;
#if !XBOX && !WINDOWS_PHONE && !PORTABLE && !UNITY5
    using global::System.Numerics;
#endif

#if UNITY5
    using MathNet.Numerics;
#endif
    using global::System.Text;

    using Artemis.Manager;

#if XBOX || WINDOWS_PHONE || PORTABLE || FORCEINT32
    using BigInteger = global::System.Int32;
#endif

    #endregion Using statements

    /// <summary>Specify a Filter class to filter what Entities (with what Components) a EntitySystem will Process.</summary>
    public class Aspect
    {
        /// <summary>Initializes a new instance of the <see cref="Aspect"/> class.</summary>
        protected Aspect()
        {
            this.OneTypesMap = 0;
            this.ExcludeTypesMap = 0;
            this.ContainsTypesMap = 0;
        }

        /// <summary>Gets or sets the contains types map.</summary>
        /// <value>The contains types map.</value>
        protected BigInteger ContainsTypesMap { get; set; }

        /// <summary>Gets or sets the exclude types map.</summary>
        /// <value>The exclude types map.</value>
        protected BigInteger ExcludeTypesMap { get; set; }

        /// <summary>Gets or sets the one types map.</summary>
        /// <value>The one types map.</value>
        protected BigInteger OneTypesMap { get; set; }

        /// <summary>All the specified types.</summary>
        /// <param name="types">The types.</param>
        /// <returns>The specified Aspect.</returns>
        public static Aspect All(params Type[] types)
        {
            return new Aspect().GetAll(types);
        }

        /// <summary>Returns an Empty Aspect (does not filter anything - i.e. rejects everything).</summary>
        /// <returns>The Aspect.</returns>
        public static Aspect Empty()
        {
            return new Aspect();
        }
        
        /// <summary>Excludes the specified types.</summary>
        /// <param name="types">The types.</param>
        /// <returns>The specified Aspect.</returns>
        public static Aspect Exclude(params Type[] types)
        {
            return new Aspect().GetExclude(types);
        }

        /// <summary>Ones the specified types.</summary>
        /// <param name="types">The types.</param>
        /// <returns>The specified Aspect.</returns>
        public static Aspect One(params Type[] types)
        {
            return new Aspect().GetOne(types);
        }

        /// <summary>Called by the EntitySystem to determine if the system is interested in the passed Entity</summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        public virtual bool Interests(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            
            if (!(this.ContainsTypesMap > 0 || this.ExcludeTypesMap > 0 || this.OneTypesMap > 0))
            {
                return false;
            }

            ////Little help
            ////10010 & 10000 = 10000
            ////10010 | 10000 = 10010
            ////10010 | 01000 = 11010

            ////1001 & 0000 = 0000 OK
            ////1001 & 0100 = 0000 NOK           
            ////0011 & 1001 = 0001 Ok

            return ((this.OneTypesMap      & entity.TypeBits) != 0                     || this.OneTypesMap      == 0) &&
                   ((this.ContainsTypesMap & entity.TypeBits) == this.ContainsTypesMap || this.ContainsTypesMap == 0) &&
                   ((this.ExcludeTypesMap  & entity.TypeBits) == 0                     || this.ExcludeTypesMap  == 0);
        }

        /// <summary>Gets all.</summary>
        /// <param name="types">The types.</param>
        /// <returns>The specified Aspect.</returns>
        public Aspect GetAll(params Type[] types)
        {
            Debug.Assert(types != null, "Types must not be null.");

            foreach (ComponentType componentType in types.Select(ComponentTypeManager.GetTypeFor))
            {
                this.ContainsTypesMap |= componentType.Bit;
            }

            return this;
        }

        /// <summary>Gets the exclude.</summary>
        /// <param name="types">The types.</param>
        /// <returns>The specified Aspect.</returns>
        public Aspect GetExclude(params Type[] types)
        {
            Debug.Assert(types != null, "Types must not be null.");

            foreach (ComponentType componentType in types.Select(ComponentTypeManager.GetTypeFor))
            {
                this.ExcludeTypesMap |= componentType.Bit;
            }

            return this;
        }

        /// <summary>Gets the one.</summary>
        /// <param name="types">The types.</param>
        /// <returns>The specified Aspect.</returns>
        public Aspect GetOne(params Type[] types)
        {
            Debug.Assert(types != null, "Types must not be null.");

            foreach (ComponentType componentType in types.Select(ComponentTypeManager.GetTypeFor))
            {
                this.OneTypesMap |= componentType.Bit;
            }

            return this;
        }

        /// <summary>Creates a string that displays all the type names of the components that interests this Aspect.</summary>
        /// <returns>A string displaying all the type names that interests this Aspect.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(1024);

            builder.AppendLine("Aspect :");
            AppendTypes(builder, " Requires the components : ", this.ContainsTypesMap);
            AppendTypes(builder, " Has none of the components : ", this.ExcludeTypesMap);
            AppendTypes(builder, " Has atleast one of the components : ", this.OneTypesMap);

            return builder.ToString();
        }

        /// <summary>Appends the types.</summary>
        /// <param name="builder">The builder.</param>
        /// <param name="headerMessage">The header message.</param>
        /// <param name="typeBits">The type bits.</param>
        private static void AppendTypes(StringBuilder builder, string headerMessage, BigInteger typeBits)
        {
            if (typeBits != 0)
            {
                builder.AppendLine(headerMessage);
                foreach (Type type in ComponentTypeManager.GetTypesFromBits(typeBits))
                {
                    builder.Append(", ");
                    builder.AppendLine(type.Name);
                }
            }
        }
    }
}
