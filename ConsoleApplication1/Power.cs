using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using Artemis.Attributes;

namespace ConsoleApplication1
{
    [PropertyComponentPool(InitialSize=10,Resizes=false)]
    public class Power : Component
    {
        public int POWER;

        [PropertyComponentCreate]
        public static Power CreateInstance()
        {
            return new Power();            
        }

        [PropertyComponentInitialize]
        public static void Initialize(Component Power)
        {
        }

        [PropertyComponentCleanup]
        public static void Cleanup(Component Power)
        {
        }

    }
}
