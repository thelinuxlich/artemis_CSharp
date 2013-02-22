using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Artemis.Attributes;
using ArtemisTest;
using Artemis.Interface;

namespace ConsoleApplication1
{
    [ArtemisEntityTemplate("teste")]
    public class EntTemplate : IEntityTemplate
    {
        public Entity BuildEntity(Entity et1, EntityWorld world , params object[] args)
        {            
            et1.AddComponent(new Power());
            et1.GetComponent<Power>().POWER = 100;
            et1.Refresh();
            return et1;
        }
    }
}
