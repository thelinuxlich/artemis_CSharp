#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityProcessingSystem.cs" company="GAMADU.COM">
//     Copyright � 2013 GAMADU.COM. All rights reserved.
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
//   Class EntityComponentProcessingSystem.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.System
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;

    #endregion Using statements

    /// <summary>Class EntityProcessingSystem.
    /// Special type of System that has NO entity associated (called once each frame)
    /// Extend it and override the ProcessSystem function
    /// </summary>
    public abstract class EntityProcessingSystem : EntitySystem
    {
        /// <summary>Initializes a new instance of the <see cref="EntityProcessingSystem"/> class.</summary>
        protected EntityProcessingSystem()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EntityProcessingSystem" /> class.</summary>
        /// <param name="aspect">The aspect.</param>
        protected EntityProcessingSystem(Aspect aspect)
            : base(aspect)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EntityProcessingSystem"/> class.</summary>
        /// <param name="requiredType">Type of the required.</param>
        /// <param name="otherTypes">The other types.</param>
        protected EntityProcessingSystem(Type requiredType, params Type[] otherTypes)
            : base(EntitySystem.GetMergedTypes(requiredType, otherTypes))
        {
        }

        /// <summary>Processes the System. Users might extend this method.</summary>
        public virtual void ProcessSystem()
        {
        }

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public virtual void Process(Entity entity)
        {
        }

        /// <summary>Begins this instance processing.</summary>
        protected override void Begin()
        {
            base.Begin();
            this.ProcessSystem();
        }

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity entity in entities.Values)
            {
                this.Process(entity);
            }
        }
    }
}