#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagSystem.cs" company="GAMADU.COM">
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
//   Tag System does not fire ANY Events of the EntitySystem.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.System
{
    #region Using statements

    using global::System.Collections.Generic;
    using global::System.Diagnostics;

    #endregion Using statements

    /// <summary>Tag System does not fire ANY Events of the EntitySystem.</summary>
    public abstract class TagSystem : ProcessingSystem
    {
        private readonly string tag;

        /// <summary>Initializes a new instance of the <see cref="TagSystem"/> class.</summary>
        /// <param name="tag">The tag.</param>
        protected TagSystem(string tag)
        {
            Debug.Assert(!string.IsNullOrEmpty(tag), "Tag must not be null.");

            this.tag = tag;
        }

        /// <summary>Gets the tag.</summary>
        public string Tag
        {
            get { return this.tag; }
        }

        /// <summary>Called when [change].</summary>
        /// <param name="entity">The entity.</param>
        public override void OnChange(Entity entity)
        {
        }

        public override void ProcessSystem()
        {
            Entity entity = this.EntityWorld.TagManager.GetEntity(this.Tag);
            if (entity != null)
            {
                this.Process(entity);
            }
        }

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public abstract void Process(Entity entity);
    }
}