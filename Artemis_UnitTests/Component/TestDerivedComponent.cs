using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.Component
{
    class TestDerivedComponent : TestBaseComponent
    {
        public override bool IsDerived()
        {
            return true;
        }

        public int DerivedValue { get; set; }
    }
}
