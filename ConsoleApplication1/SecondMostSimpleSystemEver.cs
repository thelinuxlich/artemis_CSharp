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
    [AttributeEntitySystem(ExecutionType = ExecutionType.Update, Layer = 0)]
    public class SecondMostSimpleSystemEver : EntityProcessingSystem
    {        
        public SecondMostSimpleSystemEver()
            : base(Aspect.One(typeof(Power), typeof(Health)))
        {
        }

        public override void Process(Entity e)
        {
            if(e.GetComponent<Health>()!=null)
                e.GetComponent<Health>().AddDamage(10);

            if (e.GetComponent<Power>() != null)
                e.GetComponent<Power>().POWER -=10;
        }

        


    }
}
