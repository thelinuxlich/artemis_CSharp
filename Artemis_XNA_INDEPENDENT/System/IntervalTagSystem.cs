#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntervalTagSystem.cs" company="GAMADU.COM">
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
//   Class IntervalTagSystem.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.System
{
    #region Using statement

    using global::System;

    using Artemis.Utils;

    #endregion

    /// <summary>Class IntervalTagSystem.</summary>
    public abstract class IntervalTagSystem : TagSystem
    {
        /// <summary>The timer.</summary>
        private readonly Timer timer;

        /// <summary>Initializes a new instance of the <see cref="IntervalTagSystem"/> class.</summary>
        /// <param name="timeSpan">The time span.</param>
        /// <param name="tag">The tag.</param>
        protected IntervalTagSystem(TimeSpan timeSpan, string tag)
            : base(tag)
        {
            this.timer = new Timer(timeSpan);
        }

        /// <summary>Checks the processing.</summary>
        /// <returns><see langword="true" /> if this instance is enabled, <see langword="false" /> otherwise</returns>
        protected override bool CheckProcessing()
        {
            if (this.timer.IsReached(this.EntityWorld.Delta))
            {
                return this.IsEnabled;
            }

            return false;
        }
    }
}