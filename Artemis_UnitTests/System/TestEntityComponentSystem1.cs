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
    class TestEntityComponentSystem1 : EntityComponentProcessingSystem<TestHealthComponent>
    {
        public override void Process(Entity e, TestHealthComponent health)
        {
            Assert.IsTrue(this.Aspect.Interests(e));

            Assert.IsNotNull(health);
            Assert.AreEqual(health, e.GetComponent<TestHealthComponent>());
        }
    }
}
