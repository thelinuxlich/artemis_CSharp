#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlackBoard.cs" company="GAMADU.COM">
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
//   Class BlackBoard.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Blackboard
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;

    #endregion Using statements

    /// <summary>Class BlackBoard.</summary>
    public class BlackBoard
    {
        /// <summary>The entry lock.</summary>
        private readonly object entryLock;

        /// <summary>The intelligence.</summary>
        private readonly Dictionary<string, object> intelligence;

        /// <summary>The triggers.</summary>
        private readonly Dictionary<string, List<Trigger>> triggers;

        /// <summary>Initializes a new instance of the <see cref="BlackBoard"/> class.</summary>
        public BlackBoard()
        {
            this.triggers = new Dictionary<string, List<Trigger>>();
            this.intelligence = new Dictionary<string, object>();
            this.entryLock = new object();
        }

        /// <summary>Adds the trigger.</summary>
        /// <param name="trigger">The trigger.</param>
        /// <param name="evaluateNow">if set to <see langword="true" /> [evaluate now].</param>
        public void AddTrigger(Trigger trigger, bool evaluateNow = false)
        {
            lock (this.entryLock)
            {
                trigger.BlackBoard = this;
                foreach (string intelName in trigger.WorldPropertiesMonitored)
                {
                    if (this.triggers.ContainsKey(intelName))
                    {
                        this.triggers[intelName].Add(trigger);
                    }
                    else
                    {
                        this.triggers[intelName] = new List<Trigger>
                                                       {
                                                           trigger
                                                       };
                    }
                }

                if (evaluateNow)
                {
                    if (trigger.IsFired == false)
                    {
                        trigger.Fire(TriggerStateType.TriggerAdded);
                    }
                }
            }
        }

        /// <summary>Atomics the operate on entry.</summary>
        /// <param name="operation">The operation.</param>
        public void AtomicOperateOnEntry(Action<BlackBoard> operation)
        {
            lock (this.entryLock)
            {
                operation(this);
            }
        }

        /// <summary>Gets the entry.</summary>
        /// <typeparam name="T">The <see langword="Type"/> T.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>The specified element.</returns>
        public T GetEntry<T>(string name)
        {
            object ret = this.GetEntry(name);
            return ret == null ? default(T) : (T)ret;
        }

        /// <summary>Gets the entry.</summary>
        /// <param name="name">The name.</param>
        /// <returns>The specified element.</returns>
        public object GetEntry(string name)
        {
            object ret;
            this.intelligence.TryGetValue(name, out ret);
            return ret;
        }

        /// <summary>Removes the entry.</summary>
        /// <param name="name">The name.</param>
        public void RemoveEntry(string name)
        {
            lock (this.entryLock)
            {
                this.intelligence.Remove(name);

                if (this.triggers.ContainsKey(name))
                {
                    foreach (Trigger item in this.triggers[name].Where(item => item.IsFired == false))
                    {
                        item.Fire(TriggerStateType.ValueRemoved);
                    }
                }
            }
        }

        /// <summary>Removes the trigger.</summary>
        /// <param name="trigger">The trigger.</param>
        public void RemoveTrigger(Trigger trigger)
        {
            lock (this.entryLock)
            {
                foreach (string intelName in trigger.WorldPropertiesMonitored)
                {
                    this.triggers[intelName].Remove(trigger);
                }
            }
        }

        /// <summary>Sets the entry.</summary>
        /// <typeparam name="T">The <see langword="Type"/> T.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="intel">The intel.</param>
        public void SetEntry<T>(string name, T intel)
        {
            lock (this.entryLock)
            {
                TriggerStateType triggerStateType = this.intelligence.ContainsKey(name) ? TriggerStateType.ValueChanged : TriggerStateType.ValueAdded;
                this.intelligence[name] = intel;

                if (this.triggers.ContainsKey(name))
                {
                    foreach (Trigger item in this.triggers[name].Where(item => item.IsFired == false))
                    {
                        item.Fire(triggerStateType);
                    }
                }
            }
        }

        /// <summary>Get a list of all related triggers.</summary>
        /// <param name="name">The name.</param>
        /// <returns>A List{Trigger} of appropriated triggers.</returns>
        public List<Trigger> TriggerList(string name)
        {
            return this.triggers[name];
        }
    }
}