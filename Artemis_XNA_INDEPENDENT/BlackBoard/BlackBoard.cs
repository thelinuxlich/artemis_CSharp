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
        /// <returns>The specified <see langword="Type"/> T.</returns>
        public T GetEntry<T>(string name)
        {
            if (this.intelligence.ContainsKey(name))
            {
                return (T)this.intelligence[name];
            }

            return default(T);
        }

        /// <summary>Gets the entry.</summary>
        /// <param name="name">The name.</param>
        /// <returns>The specified System.Object.</returns>
        public object GetEntry(string name)
        {
            if (this.intelligence.ContainsKey(name))
            {
                return this.intelligence[name];
            }

            return null;
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

        /// <summary>Triggers the list.</summary>
        /// <param name="name">The name.</param>
        /// <returns>The specified List{Trigger}.</returns>
        public List<Trigger> TriggerList(string name)
        {
            return this.triggers[name];
        }
    }
}