using Artemis;
using Artemis.System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitTests.Component;

namespace UnitTests.System
{
    class TestEntityComponentSystem1 : EntityProcessingSystem<TestHealthComponent>
    {
        protected override void Process(Entity e, TestHealthComponent health)
        {
            Assert.IsTrue(this.Aspect.Interests(e));

            Assert.IsNotNull(health);
            Assert.AreEqual(health, e.GetComponent<TestHealthComponent>());
        }
    }
}
