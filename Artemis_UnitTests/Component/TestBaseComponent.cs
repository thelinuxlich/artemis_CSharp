using Artemis.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.Component
{
    class TestBaseComponent : IComponent<TestBaseComponent>
    {
        public virtual bool IsDerived()
        {
            return false;
        }

        public int Value { get; set; }
    }
}
