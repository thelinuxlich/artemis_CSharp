using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using ArtemisTest.Components;
using Artemis.Attributes;
using ConsoleApplication1;

namespace ArtemisTest
{
    [ArtemisEntitySystem(ExecutionType = ExecutionType.UpdateSynchronous, Layer = 0)]
    public class ThirdMostSimpleSystemEver : EntityProcessingSystem
    {
        public ThirdMostSimpleSystemEver()
            : base(Aspect.One(typeof(Power2), typeof(Health)))
        {
        }

        public override void Process(Entity e)
        {
            if(e.GetComponent<Health>()!=null)
                e.GetComponent<Health>().AddDamage(10);

            if (e.GetComponent<Power2>() != null)
                e.GetComponent<Power2>().POWER -=10;
        }

        


    }
}
