using Artemis;
using Artemis.System;
#if METRO
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitTests.Component;

namespace UnitTests.System
{
    class TestEntityComponentSystem2 : EntityProcessingSystem<TestHealthComponent, TestPowerComponent>
    {
        protected override void Process(Entity e, TestHealthComponent health, TestPowerComponent power)
        {
            Assert.IsTrue(this.Aspect.Interests(e));

            Assert.IsNotNull(health);
            Assert.IsNotNull(power);
            Assert.AreEqual(health, e.GetComponent<TestHealthComponent>());
            Assert.AreEqual(power, e.GetComponent<TestPowerComponent>());
        }
    }
}
