#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelayedEntitySystem.cs" company="GAMADU.COM">
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
//   Class DelayedEntitySystem.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.System
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;

    #endregion Using statements

    /// <summary>Class DelayedEntitySystem.</summary>
    public abstract class DelayedEntitySystem : EntitySystem
    {
        /// <summary>The accumulated delta.</summary>
        private float accumulatedDelta;

        /// <summary>The is running.</summary>
        private bool isRunning;

        /// <summary>Initializes a new instance of the <see cref="DelayedEntitySystem" /> class.</summary>
        /// <param name="types">The types.</param>
        protected DelayedEntitySystem(params Type[] types)
            : base(types)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DelayedEntitySystem" /> class.</summary>
        /// <param name="aspect">The aspect.</param>
        protected DelayedEntitySystem(Aspect aspect)
            : base(aspect)
        {
        }

        /// <summary>Gets the initial time delay.</summary>
        /// <value>The initial time delay.</value>
        public float InitialTimeDelay { get; private set; }

        /// <summary>Gets the remaining time until processing.</summary>
        /// <returns>The remaining time in milliseconds.</returns>
        public float GetRemainingTimeUntilProcessing()
        {
            if (this.isRunning)
            {
                return this.InitialTimeDelay - this.accumulatedDelta;
            }

            return 0.0f;
        }

        /// <summary>Determines whether this instance is running.</summary>
        /// <returns><see langword="true" /> if this instance is running; otherwise, <see langword="false" />.</returns>
        public bool IsRunning()
        {
            return this.isRunning;
        }

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        /// <param name="accumulatedDelta">The accumulated delta.</param>
        public abstract void ProcessEntities(IDictionary<int, Entity> entities, float accumulatedDelta);

        /// <summary>Starts the delayed run.</summary>
        /// <param name="milliseconds">The delay in milliseconds.</param>
        public void StartDelayedRun(float milliseconds)
        {
            this.InitialTimeDelay = milliseconds;
            this.accumulatedDelta = 0.0f;
            this.isRunning = true;
        }

        /// <summary>
        /// <para>Stops this instance.</para>
        /// <para>Aborts running the system in the future and stops it.</para>
        /// <para>Call delayedRun() to start it again.</para>
        /// </summary>
        public void Stop()
        {
            this.isRunning = false;
            this.accumulatedDelta = 0.0f;
        }

        /// <summary>Checks the processing.</summary>
        /// <returns><see langword="true" /> if this instance is enabled, <see langword="false" /> otherwise</returns>
        protected override bool CheckProcessing()
        {
            if (this.isRunning)
            {
                this.accumulatedDelta += this.EntityWorld.ElapsedTime;

                if (this.accumulatedDelta >= this.InitialTimeDelay)
                {
                    return this.IsEnabled;
                }
            }

            return false;
        }

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            this.ProcessEntities(entities, this.accumulatedDelta);
            this.Stop();
        }
    }
}