#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentTypeManager.cs" company="GAMADU.COM">
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
//   Class ComponentTypeManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Manager
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics;
    using global::System.Linq;
    using global::System.Reflection;
#if !XBOX && !WINDOWS_PHONE  && !PORTABLE && !UNITY5
    using global::System.Numerics;
#endif
#if XBOX || WINDOWS_PHONE || PORTABLE || FORCEINT32
    using BigInteger = global::System.Int32;
#endif
#if METRO
    using Artemis.Attributes;
#endif

    using Artemis.Interface;

    #endregion Using statements

    /// <summary>Class ComponentTypeManager.</summary>
    public static class ComponentTypeManager
    {
        /// <summary>The component types.</summary>
        private static readonly Dictionary<Type, ComponentType> ComponentTypes = new Dictionary<Type, ComponentType>();

        /// <summary>Gets the bit.</summary>
        /// <typeparam name="T">The <see langword="Type"/> T.</typeparam>
        /// <returns>The bit flag register.</returns>
        public static BigInteger GetBit<T>() where T : IComponent
        {
            return GetTypeFor<T>().Bit;
        }

        /// <summary>Gets the id.</summary>
        /// <typeparam name="T">The <see langword="Type"/> T.</typeparam>
        /// <returns>The id.</returns>
        public static int GetId<T>() where T : IComponent
        {
            return GetTypeFor<T>().Id;
        }

        /// <summary>Get the component type for the given component instance.</summary>
        /// <typeparam name="T">Component for which you want the component type.</typeparam>
        /// <returns>Component Type.</returns>
        public static ComponentType GetTypeFor<T>() where T : IComponent
        {
            return GetTypeFor(typeof(T));
        }

        /// <summary><para>Ensure the given component type [tag] is an "official"</para>
        ///   <para>component type for your solution. If it does not already</para>
        ///   <para>exist, add it to the bag of available component types.</para>
        ///   <para>This is a way you can easily add "official" component</para>
        ///   <para>types to your solution.</para></summary>
        /// <param name="component">The component type label you want to ensure is an "official" component type</param>
        /// <returns>The specified ComponentType.</returns>
        public static ComponentType GetTypeFor(Type component)
        {
            Debug.Assert(component != null, "Component must not be null.");

            ComponentType result;
            if (!ComponentTypes.TryGetValue(component, out result))
            {
                result = new ComponentType();
                ComponentTypes.Add(component, result);
            }

            return result;
        }

        /// <summary><para>Scans assemblies for types implementing <see cref="IComponent"/> interface</para>
        /// <para>and creates a corresponding Artemis <see cref="ComponentType"/> for each type found.</para>
        /// </summary>
        /// <param name="assembliesToScan">The assemblies to scan.</param>
        public static void Initialize(params Assembly[] assembliesToScan)
        {
#if FULLDOTNET || METRO || UNITY5
            if (assembliesToScan.Length == 0)
            {
                assembliesToScan = AppDomain.CurrentDomain.GetAssemblies().ToArray();
            }
#endif

            foreach (Assembly assembly in assembliesToScan)
            {
#if METRO
                IEnumerable<Type> types = assembly.ExportedTypes;
#else
                IEnumerable<Type> types = assembly.GetTypes();
#endif

                Initialize(types, ignoreInvalidTypes: true);
            }
        }

        /// <summary><para>Scans the types for types implementing <see cref="IComponent"/> interface</para>
        /// <para>and creates a corresponding Artemis <see cref="ComponentType"/> for each type found.</para>
        /// </summary>
        /// <param name="types">Types to scan</param>
        /// <param name="ignoreInvalidTypes">If set to <see langword="true" />, will not throw Exception</param>
        public static void Initialize(IEnumerable<Type> types, bool ignoreInvalidTypes = false)
        {
            foreach (Type type in types)
            {
#if METRO
                if (typeof(IComponent).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
                {
                    if (type.GetTypeInfo().IsInterface)
                        continue;
#else
                if (typeof(IComponent).IsAssignableFrom(type))
                {
                    if (type.IsInterface)
                        continue;
#endif

                    if (type == typeof(ComponentPoolable))
                        continue;

                    GetTypeFor(type);
                }
                else if (!ignoreInvalidTypes)
                {
                    throw new ArgumentException(String.Format("Type {0} does not implement {1} interface", type, typeof(IComponent)));
                }
            }
        }

        /// <summary>Creates an enumerable from a <c>BigIntger</c> which holds type bits.</summary>
        /// <param name="bits">The BigInteger which holds the type bits.</param>
        /// <returns>An Enumerable of each type the bits has.</returns>
        internal static IEnumerable<Type> GetTypesFromBits(BigInteger bits)
        {
            foreach (KeyValuePair<Type, ComponentType> keyValuePair in ComponentTypes)
            {
                if ((keyValuePair.Value.Bit & bits) != 0)
                {
                    yield return keyValuePair.Key;
                }
            }            
        }

        /// <summary>Sets the type for specified ComponentType T.</summary>
        /// <typeparam name="T">The <see langword="Type" /> of T.</typeparam>
        /// <param name="type">The type.</param>
        internal static void SetTypeFor<T>(ComponentType type)
        {
            ComponentTypes.Add(typeof(T), type);
        }
    }
}