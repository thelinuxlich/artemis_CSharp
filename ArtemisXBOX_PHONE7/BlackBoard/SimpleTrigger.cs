using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis
{
    public class SimpleTrigger : Trigger
    {
        Func<BlackBoard, TriggerState, bool> Condition;
        Action<TriggerState> onFire;
        public SimpleTrigger(String Name, Func<BlackBoard, TriggerState, bool> Condition, Action<TriggerState> onFire = null)
        {
            this.WorldPropertiesMonitored.Add(Name);
            this.Condition = Condition;
            this.onFire = onFire;
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
