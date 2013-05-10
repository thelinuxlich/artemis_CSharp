#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TriggerMultiCondition.cs" company="GAMADU.COM">
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
//   Class TriggerMultiCondition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Blackboard
{
    #region Using statements

    using global::System;

    #endregion Using statements

    /// <summary>Class TriggerMultiCondition.</summary>
    public class TriggerMultiCondition : Trigger
    {
        /// <summary>The condition.</summary>
        private readonly Func<BlackBoard, TriggerStateType, bool> condition;

        /// <summary>The on fire.</summary>
        private readonly Action<TriggerStateType> onFire;

        /// <summary>Initializes a new instance of the <see cref="TriggerMultiCondition"/> class.</summary>
        /// <param name="condition">The condition.</param>
        /// <param name="onFire">The on fire.</param>
        /// <param name="names">The names.</param>
        public TriggerMultiCondition(Func<BlackBoard, TriggerStateType, bool> condition, Action<TriggerStateType> onFire = null, params string[] names)
        {
            this.WorldPropertiesMonitored.AddRange(names);
            this.condition = condition;
            this.onFire = onFire;
        }

        /// <summary>Removes the this trigger.</summary>
        public new void RemoveThisTrigger()
        {
            this.BlackBoard.RemoveTrigger(this);
        }

        /// <summary>Called if is fired.</summary>
        /// <param name="triggerStateType">State of the trigger.</param>
        protected override void CalledOnFire(TriggerStateType triggerStateType)
        {
            if (this.onFire != null)
            {
                this.onFire(triggerStateType);
            }
        }

        /// <summary>Checks the condition to fire.</summary>
        /// <returns><see langword="true" /> if condition is fired, <see langword="false" /> otherwise</returns>
        protected override bool CheckConditionToFire()
        {
            return this.condition(this.BlackBoard, this.TriggerStateType);
        }
    }
}