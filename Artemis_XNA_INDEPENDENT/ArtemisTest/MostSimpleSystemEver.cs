using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using ArtemisTest.Components;

namespace ArtemisTest
{
    public class MostSimpleSystemEver : EntityProcessingSystem
    {
        public MostSimpleSystemEver()
            : base(Aspect.AspectExclude(typeof(Power)))
        {
        }

        public override void Process(Entity e)
        {
            e.GetComponent<Health>().AddDamage(10);
        }

        


    }
}
