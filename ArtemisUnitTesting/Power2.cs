using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Artemis.Attributes;

namespace ArtemisTest
{
    [ArtemisComponentPool(InitialSize=10,Resizes=false)]
    public class Power2 : ComponentPoolable
    {
        public int POWER;

        [ArtemisComponentCreate]
        public static Power2 CreateInstance(Type type)
        {
            return new Power2();            
        }

        #region ComponentPoolable Members

        public void Initialize()
        {            
        }

        public void Cleanup()
        {
        }

        #endregion
    }
}
