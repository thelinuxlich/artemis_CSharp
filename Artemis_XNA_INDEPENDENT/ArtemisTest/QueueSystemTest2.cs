using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using ArtemisTest.Components;

namespace ArtemisTest
{
    public class QueueSystemTest2 : Artemis.QueueProcessingSystemThreadSafe
    {
        public QueueSystemTest2() : base(){}
        public override void Process(Entity Entity)
        {
            Health Health = Entity.GetComponent<Health>();
            Health.AddDamage(10);
        }
    }
}
