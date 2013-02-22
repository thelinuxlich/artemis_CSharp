using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using ArtemisTest.Components;
using Artemis.System;

namespace ArtemisTest
{
    public class HybridQueueSystemTest : HybridQueueSystemProcessing
    {
        public HybridQueueSystemTest()
            : base(typeof(Health))
        {}

        public override void Process(Entity Entity)
        {
            Health Health = Entity.GetComponent<Health>();
            Health.AddDamage(10);
        }
        
    }
}
