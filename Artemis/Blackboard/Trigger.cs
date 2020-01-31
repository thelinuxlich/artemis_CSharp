#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Trigger.cs" company="GAMADU.COM">
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
//   Class Trigger.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Blackboard
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;

    #endregion Using statements

    /// <summary>Class Trigger.</summary>
    public class Trigger
    {
        /// <summary>Initializes a new instance of the <see cref="Trigger"/> class.</summary>
        /// <param name="propertyName">Name of the property.</param>
        public Trigger(params string[] propertyName)
        {
            this.IsFired = false;
            this.WorldPropertiesMonitored = new List<string>();
            foreach (string item in propertyName)
            {
                this.WorldPropertiesMonitored.Add(item);
            }
        }

        /// <summary>Occurs when [on fire].</summary>
        public event Action<Trigger> OnFire;

        /// <summary>Gets the black board.</summary>
        /// <value>The black board.</value>
        public BlackBoard BlackBoard { get; internal set; }

        /// <summary>Gets the state of the trigger.</summary>
        /// <value>The state of the trigger.</value>
        public TriggerStateType TriggerStateType { get; private set; }

        /// <summary>Gets or sets the entityWorld properties monitored.</summary>
        /// <value>The entityWorld properties monitored.</value>
        public List<string> WorldPropertiesMonitored { get; protected set; }

        /// <summary>Gets or sets a value indicating whether this instance is fired.</summary>
        /// <value><see langword="true" /> if this instance is fired; otherwise, <see langword="false" />.</value>
        internal bool IsFired { get; set; }

        /// <summary>Removes the this trigger.</summary>
        public void RemoveThisTrigger()
        {
            this.BlackBoard.RemoveTrigger(this);
        }

        /// <summary>Fires the specified trigger state.</summary>
        /// <param name="triggerStateType">State of the trigger.</param>
        internal void Fire(TriggerStateType triggerStateType)
        {
            this.IsFired = true;
            this.TriggerStateType = triggerStateType;
            if (this.CheckConditionToFire())
            {
                this.CalledOnFire(triggerStateType);
                if (this.OnFire != null)
                {
                    this.OnFire(this);
                }
            }

            this.IsFired = false;
        }

        /// <summary>Called if is fired.</summary>
        /// <param name="triggerStateType">State of the trigger.</param>
        protected virtual void CalledOnFire(TriggerStateType triggerStateType)
        {
        }

        /// <summary>Checks the condition to fire.</summary>
        /// <returns><see langword="true" /> if XXXX, <see langword="false" /> otherwise</returns>
        protected virtual bool CheckConditionToFire()
        {
            return true;
        }
    }
}