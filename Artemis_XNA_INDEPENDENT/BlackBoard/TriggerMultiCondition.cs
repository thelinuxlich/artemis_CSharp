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