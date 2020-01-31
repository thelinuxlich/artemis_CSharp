#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArtemisComponentPool.cs" company="GAMADU.COM">
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
//   Class ArtemisComponentPool.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Attributes
{
    #region Using statements

    using global::System;

    #endregion Using statements

    /// <summary>Class ArtemisComponentPool.</summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ArtemisComponentPool : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ArtemisComponentPool"/> class.</summary>
        public ArtemisComponentPool()
        {
            this.InitialSize = 10;
            this.ResizeSize = 10;
            this.IsResizable = true;
            this.IsSupportMultiThread = false;
        }

        /// <summary>Gets or sets the initial size of the Pool. Default is 10.</summary>
        /// <value>The initial size.</value>
        public int InitialSize { get; set; }

        /// <summary>Gets or sets the size of the pool resize. Default is 10.</summary>
        /// <value>The size of the resize.</value>
        public int ResizeSize { get; set; }

        /// <summary>Gets or sets a value indicating whether the pool is resizable.</summary>
        /// <value><see langword="true" /> if the pool is resizable; otherwise, <see langword="false" />.</value>
        public bool IsResizable { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance should support multi thread environment. Default is <see langword="false" />.</summary>
        /// <value><see langword="true" /> if this instance should support multi thread environment; otherwise, <see langword="false" />.</value>
        public bool IsSupportMultiThread { get; set; }
    }
}