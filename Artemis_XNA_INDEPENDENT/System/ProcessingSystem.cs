#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessingSystem.cs" company="GAMADU.COM">
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
//   Class EntityProcessingSystem.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.System
{
    /// <summary><para>The processing system class.</para>
    /// <para>Special type of System that has NO entity associated (called once each frame).</para>
    /// <para>Extend it and override the ProcessSystem function.</para></summary>
    public abstract class ProcessingSystem : EntitySystem
    {
        /// <summary>Called when [change].</summary>
        /// <param name="entity">The entity.</param>
        public override void OnChange(Entity entity)
        {
            // By overriding base.OnChange we disable notification of Entities' changes
        }

        /// <summary>Processes this instance. [Internal]</summary>
        public override void Process()
        {
            if (this.CheckProcessing())
            {
                this.Begin();
                this.ProcessSystem();
                this.End();
            }
        }

        /// <summary>Processes the System. Users must extend this method. Called once per frame.</summary>
        public abstract void ProcessSystem();
    }
}