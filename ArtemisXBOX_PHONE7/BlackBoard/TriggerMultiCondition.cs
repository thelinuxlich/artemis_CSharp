using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis
{
    public class TriggerMultiCondition : Trigger
    {
        Func<BlackBoard, TriggerState, bool> Condition;
        Action<TriggerState> onFire;
        public TriggerMultiCondition(Func<BlackBoard, TriggerState, bool> Condition, Action<TriggerState> onFire = null, params String[] Names)
        {
            this.WorldPropertiesMonitored.AddRange(Names);
            this.Condition = Condition;
            this.onFire = onFire;
        }

        public void RemoveThisTrigger()
        {
            BlackBoard.RemoveTrigger(this);
        }

        protected override bool CheckConditionToFire()
        {
            return Condition(BlackBoard, TriggerState);
        }

        protected override void CalledOnFire(TriggerState TriggerState)
        {
            if (onFire != null)
                onFire(TriggerState);
        }
    }
}
