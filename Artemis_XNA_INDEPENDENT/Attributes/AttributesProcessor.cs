#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributesProcessor.cs" company="GAMADU.COM">
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
//   Class AttributesProcessor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Attributes
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Reflection;

    #endregion Using statements

    /// <summary>Class AttributesProcessor.</summary>
    public class AttributesProcessor
    {
        /// <summary>The supported attributes.</summary>
        public static readonly List<Type> SupportedAttributes = new List<Type>
                                                                    {
                                                                        typeof(ArtemisEntitySystem),
                                                                        typeof(ArtemisEntityTemplate),
                                                                        typeof(ArtemisComponentPool),
                                                                        typeof(ArtemisComponentCreate)
                                                                    };

        /// <summary>Processes the specified supported attributes.</summary>
        /// <param name="supportedAttributes">The supported attributes.</param>
        /// <returns>The Dictionary{TypeList{Attribute}}.</returns>
        public static IDictionary<Type, List<Attribute>> Process(List<Type> supportedAttributes)
        {
            IDictionary<Type, List<Attribute>> attributeTypes = new Dictionary<Type, List<Attribute>>();

#if FULLDOTNET
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly item in loadedAssemblies)
            {
                Type[] types = item.GetTypes();
                foreach (Type type in types)
                {
                    object[] attributes = type.GetCustomAttributes(false);
                    foreach (object attribute in attributes)
                    {
                        if (supportedAttributes.Contains(attribute.GetType()))
                        {
                            if (!attributeTypes.ContainsKey(type))
                            {
                                attributeTypes[type] = new List<Attribute>();
                            }

                            attributeTypes[type].Add((Attribute)attribute);
                        }
                    }
                }
            }

            return attributeTypes;
#else

            List<Assembly> loadedAssemblies = new List<Assembly>
                                                  {
                                                      Assembly.GetCallingAssembly(),
                                                      Assembly.GetExecutingAssembly(),
                                                  };

            foreach (Assembly item in loadedAssemblies)
            {
                Type[] types = item.GetTypes();
                foreach (Type type in types)
                {
                    var attributes = type.GetCustomAttributes(false);
                    foreach (object attribute in attributes)
                    {
                        if (supportedAttributes.Contains(attribute.GetType()))
                        {
                            if (!attributeTypes.ContainsKey(type))
                            {
                                attributeTypes[type] = new List<Attribute>();
                            }

                            attributeTypes[type].Add((Attribute)attribute);
                        }
                    }
                }
            }

            return attributeTypes;

#endif
        }
    }
}