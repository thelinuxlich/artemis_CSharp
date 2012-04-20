using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Artemis
{
    public class BlackBoard
    {
        private Dictionary<String, object> intelligence = new Dictionary<string, object>();
        private Dictionary<String, List<Trigger>> triggers = new Dictionary<string, List<Trigger>>();                
        private object entrylock = new object();       
        
        public void SetEntry<T>(String name, T intel)
        {
            lock (entrylock)
            {
                TriggerState TriggerState = intelligence.ContainsKey(name) == true ? TriggerState.VALUE_CHANGED : TriggerState.VALUE_ADDED;
                intelligence[name] = intel;


                if (triggers.ContainsKey(name))
                {
                    foreach (var item in triggers[name])
                    {
                        if(item.fired ==false)
                            item.Fire(TriggerState);
                    }
                }
            }
        }

        public void AtomicOperateOnEntry<T>(Action<BlackBoard> operation)
        {
            lock (entrylock)
            {
                operation(this);
            }
        }


        public void RemoveEntry(String name)
        {
            lock (entrylock)
            {
                intelligence.Remove(name);


                if (triggers.ContainsKey(name))
                {
                    foreach (var item in triggers[name])
                    {
                        if (item.fired == false)
                            item.Fire(TriggerState.VALUE_REMOVED);
                    }
                }
            }
        }

        public T GetEntry<T>(String name)
        {
            if (intelligence.ContainsKey(name))
                return (T)intelligence[name];
            return default(T);
        }

        public object GetEntry(String name)
        {
            if (intelligence.ContainsKey(name))
                return intelligence[name];
            return null;
        }

        public void AddTrigger(Trigger Trigger, bool evaluateNow = false)
        {
            lock (entrylock)
            {
                Trigger.BlackBoard = this;
                foreach (var IntellName in Trigger.WorldPropertiesMonitored)
                {

                    if (triggers.ContainsKey(IntellName))
                    {
                        triggers[IntellName].Add(Trigger);
                    }
                    else
                    {
                        triggers[IntellName] = new List<Trigger>();
                        triggers[IntellName].Add(Trigger);
                    }
                }
                if (evaluateNow)
                {
                    if (Trigger.fired == false)
                        Trigger.Fire(TriggerState.TRIGGER_ADDED);
                }
            }
        }

        public void RemoveTrigger(Trigger Trigger)
        {
            lock (entrylock)
            {
                foreach (var IntellName in Trigger.WorldPropertiesMonitored)
                {
                    triggers[IntellName].Remove(Trigger);
                }
            }
        }

        public List<Trigger> TriggerList(String Name)
        {
            return triggers[Name];
        }
    }
}
