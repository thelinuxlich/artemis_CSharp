﻿#region File description

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
    using Utils;

    #endregion Using statements


#if METRO

    sealed class AppDomain
    {
        public static AppDomain CurrentDomain { get; private set; }

        static AppDomain()
        {
            CurrentDomain = new AppDomain();
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            return GetAssemblyListAsync().Result;
        }

        private async global::System.Threading.Tasks.Task<IEnumerable<Assembly>> GetAssemblyListAsync()
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            List<Assembly> assemblies = new List<Assembly>();
            foreach (Windows.Storage.StorageFile file in await folder.GetFilesAsync())
            {
                if (file.FileType == ".dll" || file.FileType == ".exe")
                {
                    AssemblyName name = new AssemblyName() { Name = global::System.IO.Path.GetFileNameWithoutExtension(file.Name) };
                    Assembly asm = Assembly.Load(name);
                    assemblies.Add(asm);
                }
            }

            return assemblies;
        }
    }

#endif

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

#if FULLDOTNET || METRO
        /// <summary>Processes the specified supported attributes.</summary>
        /// <param name="supportedAttributes">The supported attributes.</param>
        /// <returns>
        /// The Dictionary{TypeList{Attribute}}.
        /// </returns>
        public static IDictionary<Type, List<Attribute>> Process(List<Type> supportedAttributes)
        {
            return Process(supportedAttributes, AppDomain.CurrentDomain.GetAssemblies());
        }
#endif

        /// <summary>Processes the specified supported attributes.</summary>
        /// <param name="supportedAttributes">The supported attributes.</param>
        /// <param name="assembliesToScan">The assemblies to scan.</param>
        /// <returns>
        /// The Dictionary{TypeList{Attribute}}.
        /// </returns>
        public static IDictionary<Type, List<Attribute>> Process(List<Type> supportedAttributes, IEnumerable<Assembly> assembliesToScan = null)
        {
            IDictionary<Type, List<Attribute>> attributeTypes = new Dictionary<Type, List<Attribute>>();

            foreach (Assembly item in assembliesToScan)
            {
#if METRO      
                IEnumerable<Type> types = item.ExportedTypes;
#else
                IEnumerable<Type> types = item.GetTypes();
#endif          

                foreach (Type type in types)
                {
                    var attributes = type.GetTypeInfo().GetCustomAttributes(false);
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
        }
    }
}