#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Timer.cs" company="GAMADU.COM">
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
//   The class Timer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Utils
{
    #region Using statements

    using global::System;

    #endregion

    /// <summary>The class Timer.</summary>
    public class Timer
    {
        /// <summary>The delay ticks.</summary>
        private readonly long delayTicks;

        /// <summary>Initializes a new instance of the <see cref="Timer" /> class.</summary>
        /// <param name="delay">The delay.</param>
        public Timer(TimeSpan delay)
        {
            this.delayTicks = delay.Ticks;
            this.Reset();
        }

        /// <summary>Gets the accumulated ticks.</summary>
        /// <value>The accumulated ticks.</value>
        public long AccumulatedTicks { get; private set; }

        /// <summary>Determines whether the specified delta is reached.</summary>
        /// <param name="deltaTicks">The delta in ticks.</param>
        /// <returns><see langword="true" /> if the specified delta is reached; otherwise, <see langword="false" />.</returns>
        public bool IsReached(long deltaTicks)
        {
            this.AccumulatedTicks += deltaTicks;
            if (this.AccumulatedTicks >= this.delayTicks)
            {
                this.AccumulatedTicks -= this.delayTicks;
                return true;
            }

            return false;
        }

        /// <summary>Resets this instance.</summary>
        public void Reset()
        {
            this.AccumulatedTicks = 0;
        }
    }
}
