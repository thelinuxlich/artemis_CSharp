namespace Artemis.Blackboard
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;

    #endregion Using statements

    /// <summary>Class Trigger.</summary>
    public class Trigger
    {
        /// <summary>The is fired.</summary>
        internal bool IsFired = false;

        /// <summary>Initializes a new instance of the <see cref="Trigger"/> class.</summary>
        /// <param name="propertyName">Name of the property.</param>
        public Trigger(params string[] propertyName)
        {
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