using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using ArtemisTest.Components;
using Artemis.System;

namespace ArtemisTest
{
    public class MostSimpleSystemEver : EntityProcessingSystem
    {
        public MostSimpleSystemEver()
            : base(Aspect.Exclude(typeof(Power)))
        {
        }

        public override void Process(Entity e)
        {
            e.GetComponent<Health>().AddDamage(10);
        }

        


    }
}
