namespace Artemis.Blackboard
{
    #region Using statements

    using global::System;

    #endregion Using statements

    /// <summary>Class SimpleTrigger.</summary>
    public class SimpleTrigger : Trigger
    {
        /// <summary>The condition.</summary>
        private readonly Func<BlackBoard, TriggerStateType, bool> condition;

        /// <summary>The on fire.</summary>
        private readonly Action<TriggerStateType> onFire;

        /// <summary>Initializes a new instance of the <see cref="SimpleTrigger"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="onFire">The on fire.</param>
        public SimpleTrigger(string name, Func<BlackBoard, TriggerStateType, bool> condition, Action<TriggerStateType> onFire = null)
        {
            this.WorldPropertiesMonitored.Add(name);
            this.condition = condition;
            this.onFire = onFire;
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
        /// <returns><see langword="true" /> if XXXX, <see langword="false" /> otherwise</returns>
        protected override bool CheckConditionToFire()
        {
            return this.condition(this.BlackBoard, this.TriggerStateType);
        }
    }
}