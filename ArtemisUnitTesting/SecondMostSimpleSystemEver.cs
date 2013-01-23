using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using ArtemisTest.Components;

namespace ArtemisTest
{
    public class SecondMostSimpleSystemEver : EntityProcessingSystem
    {
        public SecondMostSimpleSystemEver()
            : base(Aspect.AspectOne(typeof(Power), typeof(Health)))
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
