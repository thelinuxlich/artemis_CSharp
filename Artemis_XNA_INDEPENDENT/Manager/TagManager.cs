#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagManager.cs" company="GAMADU.COM">
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
//   Class TagManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Manager
{
    #region Using statements

    using global::System.Collections.Generic;
    using global::System.Diagnostics;
    using global::System.Linq;

    #endregion Using statements

    /// <summary>Class TagManager.</summary>
    public sealed class TagManager
    {
        /// <summary>The entity by tag.</summary>
        private readonly Dictionary<string, Entity> entityByTag;

        /// <summary>Initializes a new instance of the <see cref="TagManager" /> class.</summary>
        internal TagManager()
        {
            this.entityByTag = new Dictionary<string, Entity>();
        }

        /// <summary>Gets the entity.</summary>
        /// <param name="tag">The tag.</param>
        /// <returns>The specified entity.</returns>
        public Entity GetEntity(string tag)
        {
            Debug.Assert(!string.IsNullOrEmpty(tag), "Tag must not be null or empty.");

            Entity entity;
            this.entityByTag.TryGetValue(tag, out entity);
            if (entity != null && entity.IsActive)
            {
                return entity;
            }

            this.Unregister(tag);
            return null;
        }

        /// <summary>Gets the tag of entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The tag of the specified entity.</returns>
        public string GetTagOfEntity(Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");

            string tag = null;
            foreach (KeyValuePair<string, Entity> pair in this.entityByTag.Where(pair => pair.Value.Equals(entity)))
            {
                tag = pair.Key;
                break;
            }

            return tag;
        }

        /// <summary>Determines whether the specified tag is registered.</summary>
        /// <param name="tag">The tag.</param>
        /// <returns><see langword="true" /> if the specified tag is registered; otherwise, <see langword="false" />.</returns>
        public bool IsRegistered(string tag)
        {
            Debug.Assert(!string.IsNullOrEmpty(tag), "Tag must not be null or empty.");

            return this.entityByTag.ContainsKey(tag);
        }

        /// <summary>Registers the specified tag.</summary>
        /// <param name="tag">The tag.</param>
        /// <param name="entity">The entity.</param>
        internal void Register(string tag, Entity entity)
        {
            Debug.Assert(entity != null, "Entity must not be null.");
            Debug.Assert(!string.IsNullOrEmpty(tag), "Tag must not be null or empty.");

            this.entityByTag.Add(tag, entity);
        }

        /// <summary>Unregisters the specified tag.</summary>
        /// <param name="tag">The tag.</param>
        internal void Unregister(string tag)
        {
            Debug.Assert(!string.IsNullOrEmpty(tag), "Tag must not be null or empty.");

            this.entityByTag.Remove(tag);
        }

        /// <summary>Unregisters the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        internal void Unregister(Entity entity)
        {
            string tag = this.GetTagOfEntity(entity);
            if (!string.IsNullOrEmpty(tag))
            {
                this.entityByTag.Remove(tag);
            }
        }
    }
}