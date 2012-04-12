using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis
{
    public class Trigger
    {
        public Trigger(params String[] PropetyName)
        {
            WorldPropertiesMonitored = new List<string>();
            foreach (var item in PropetyName)
            {
                WorldPropertiesMonitored.Add(item);
            }
        }

        public List<String> WorldPropertiesMonitored
        {
            get;
            protected set;
        }

        public TriggerState TriggerState
        {
            get;
            private set;
        }

        public BlackBoard BlackBoard
        {
            get;
            internal set;
        }

        public void RemoveThisTrigger()
        {
            BlackBoard.RemoveTrigger(this);
        }

        internal bool fired = false;
        internal void Fire(TriggerState TriggerState)
        {
            fired = true;
            this.TriggerState = TriggerState;
            if (CheckConditionToFire())
            {
                CalledOnFire(TriggerState);
                if (OnFire != null)
                    OnFire(this);
            }
            fired = false;
        }

        public event Action<Trigger> OnFire;

        protected virtual bool CheckConditionToFire()
        {
            return true;
        }

        protected virtual void CalledOnFire(TriggerState TriggerState)
        {
        }
    }


    public enum TriggerState : long
    {
        VALUE_ADDED = 0x00001,
        VALUE_REMOVED = 0x00010,
        VALUE_CHANGED = 0x00100,
        TRIGGER_ADDED = 0x01000
    }
}
