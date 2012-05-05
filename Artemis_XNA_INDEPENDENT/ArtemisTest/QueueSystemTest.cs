using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using ArtemisTest.Components;

namespace ArtemisTest
{
    public class QueueSystemTest : Artemis.QueueSystemProcessingThreadSafe
    {
        public QueueSystemTest() : base()
        {
        }

        public override void Process(Entity Entity)
        {
            Health Health = Entity.GetComponent<Health>();
            Health.AddDamage(10);
        }
    }
}
