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
    /// <summary>Class IntervalTagSystem.</summary>
    public abstract class IntervalTagSystem : TagSystem
    {
        /// <summary>The interval.</summary>
        private readonly float interval;

        /// <summary>The accumulated delta.</summary>
        private float accumulatedDelta;

        /// <summary>Initializes a new instance of the <see cref="IntervalTagSystem"/> class.</summary>
        /// <param name="interval">The interval.</param>
        /// <param name="tag">The tag.</param>
        protected IntervalTagSystem(float interval, string tag)
            : base(tag)
        {
            this.interval = interval;
        }

        /// <summary>Checks the processing.</summary>
        /// <returns><see langword="true" /> if this instance is enabled, <see langword="false" /> otherwise</returns>
        protected override bool CheckProcessing()
        {
            this.accumulatedDelta += this.EntityWorld.ElapsedTime;
            if (this.accumulatedDelta >= this.interval)
            {
                this.accumulatedDelta -= this.interval;
                return this.IsEnabled;
            }

            return false;
        }
    }
}