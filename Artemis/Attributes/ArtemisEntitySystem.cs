#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArtemisEntitySystem.cs" company="GAMADU.COM">
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
//   Class ArtemisEntitySystem.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Attributes
{
    #region Using statements

    using global::System;

    using Artemis.Manager;

    #endregion Using statements

    /// <summary>Class ArtemisEntitySystem.</summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ArtemisEntitySystem : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ArtemisEntitySystem"/> class.</summary>
        public ArtemisEntitySystem()
        {
            this.GameLoopType = GameLoopType.Update;
            this.Layer = 0;
            this.ExecutionType = ExecutionType.Synchronous;
        }

        /// <summary>Gets or sets the type of the game loop.</summary>
        /// <value>The type of the game loop.</value>
        public GameLoopType GameLoopType { get; set; }

        /// <summary>Gets or sets the layer.</summary>
        /// <value>The layer.</value>
        public int Layer { get; set; }

        /// <summary>Gets or sets the type of the execution.</summary>
        /// <value>The type of the execution.</value>
        public ExecutionType ExecutionType { get; set; }
    }
}