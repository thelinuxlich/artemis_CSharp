using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Artemis.Attributes;

namespace ArtemisTest
{
    [ArtemisComponentPool(InitialSize=10,IsResizable=false)]
    public class Power2 : ComponentPoolable
    {
        public int POWER;

        [ArtemisComponentCreate]
        public static Power2 CreateInstance(Type type)
        {
            return new Power2();            
        }

    }
}
