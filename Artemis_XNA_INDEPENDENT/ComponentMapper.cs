#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentMapper.cs" company="GAMADU.COM">
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
//   Fastest way to get Components from entities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis
{
    #region Using statements

    using global::System;
    using global::System.Diagnostics;

    using Artemis.Interface;
    using Artemis.Manager;

    #endregion Using statements

    /// <summary>Another way to get components from entities, prefer using the GetComponent method of the Entity object now. Note: this class is deprecated and will be removed in future releases!</summary>
    /// <typeparam name="T">The <see langword="Type"/> T.</typeparam>
    [Obsolete("This ComponetMapper and the GetComponent method of the Entity have the same performance cost now, prefer using the GetComponent<> method from Entity", false)]
    public sealed class ComponentMapper<T> where T : IComponent
    {
        /// <summary>The entity manager.</summary>
        private readonly EntityManager entityManager;

        /// <summary>The component type.</summary>
        private readonly ComponentType componentType;

        /// <summary>Initializes a new instance of the <see cref="ComponentMapper{T}"/> class.</summary>
        /// <param name="entityWorld">The entity world.</param>
        public ComponentMapper(EntityWorld entityWorld)
        {
            Debug.Assert(entityWorld != null, "Entity world must not be null.");

            this.entityManager = entityWorld.EntityManager;
            this.componentType = ComponentTypeManager.GetTypeFor<T>();
        }

        /// <summary>Gets the component mapper for.</summary>
        /// <typeparam name="TK">The <see langword="Type" /> TK.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="entityWorld">The entity world.</param>
        /// <returns>The ComponentMapper.</returns>
        public static ComponentMapper<TK> GetComponentMapperFor<TK>(TK type, EntityWorld entityWorld) where TK : IComponent
        {
            return new ComponentMapper<TK>(entityWorld);
        }

        /// <summary>Gets the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The specified component.</returns>
        public T Get(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            return (T)this.entityManager.GetComponent(entity, this.componentType);
        }
    }
}