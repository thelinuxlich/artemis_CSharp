#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestAspects.cs" company="GAMADU.COM">
//     Copyright © 2013 GAMADU.COM. All rights reserved.
//
//     Redistribution and use in source and binary forms, with or without modification, are
//     permitted provided that the following conditions are met:
//
//        1. Redistributions of source code must retain the above copyright notice, this list of
//           conditions and the following disclaimer.
//
//        2. Redistributions in binary form must reproduce the above copyright notice, this list
//           of conditions and the following disclaimer in the documentation and/or other materials
//           provided with the distribution.
//
//     THIS SOFTWARE IS PROVIDED BY GAMADU.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
//     WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//     FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
//     CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//     CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//     SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//     ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//     ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//     The views and conclusions contained in the software and documentation are those of the
//     authors and should not be interpreted as representing official policies, either expressed
//     or implied, of GAMADU.COM.
// </copyright>
// <summary>
//   Aspect unit tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace UnitTests
{
    #region Using statements

    using Artemis;
    using global::System;
#if METRO
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#elif MONO
    using NUnit.Framework;
#else
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

    using UnitTests.Component;

    #endregion Using statements

    /// <summary>Aspect test.</summary>
#if MONO
    [TestFixture]
#else
    [TestClass]
#endif
    public class TestAspect
    {
        #region Aspect.Empty
        /// <summary>
        /// Tests Aspect.Empty
        /// </summary>
#if MONO
    [Test]
#else
        [TestMethod]
#endif
        public void TestAspectEmpty()
        {
            Aspect aspect = Aspect.Empty();
            EntityWorld entityWorld = new EntityWorld();
            Entity entity;

            // Aspect.Empty is a base Aspect for EntitySystem meaning "no entities to process"
            entity = entityWorld.CreateEntity();
            Assert.IsFalse(aspect.Interests(entity), "Entity without components must NOT be subject to empty Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            Assert.IsFalse(aspect.Interests(entity), "Entity with any component must NOT be subject to empty Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestPowerComponent());
            entity.AddComponent(new TestHealthComponent());
            Assert.IsFalse(aspect.Interests(entity), "Entity with any components must NOT be subject to empty Aspect");
        } 
        #endregion

        #region Aspect.All
        /// <summary>
        /// Tests Aspect.All with single type required
        /// </summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestAspectAllSingle()
        {
            Aspect aspect = Aspect.All(typeof(TestPowerComponent));
            EntityWorld entityWorld = new EntityWorld();
            Entity entity;

            entity = entityWorld.CreateEntity();
            Assert.IsFalse(aspect.Interests(entity), "Entity without components must NOT be subject to \"Powered\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            Assert.IsFalse(aspect.Interests(entity), "Entity with {TestHealthComponent} must NOT be subject to \"Powered\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestPowerComponent());
            Assert.IsTrue(aspect.Interests(entity), "Entity with {TestPowerComponent} must be subject to \"Powered\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestPowerComponent());
            Assert.IsTrue(aspect.Interests(entity), "Entity with {TestHealthComponent, TestPowerComponent} must be subject to \"Powered\" Aspect");
        }

        /// <summary>
        /// Tests Aspect.All with multiple types required
        /// </summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestAspectAllMultiple()
        {
            Aspect aspect = Aspect.All(typeof(TestHealthComponent), typeof(TestPowerComponent));
            EntityWorld entityWorld = new EntityWorld();
            Entity entity;

            entity = entityWorld.CreateEntity();
            Assert.IsFalse(aspect.Interests(entity), "Entity without components must NOT be subject to \"Healthy Powered\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            Assert.IsFalse(aspect.Interests(entity), "Entity with {TestHealthComponent} must NOT be subject to \"Healthy Powered\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestPowerComponentPoolable());
            Assert.IsFalse(aspect.Interests(entity), "Entity with {TestHealthComponent, TestPowerComponentPoolable} must NOT be subject to \"Healthy Powered\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestPowerComponent());
            Assert.IsTrue(aspect.Interests(entity), "Entity with {TestHealthComponent, TestPowerComponent} must be subject to \"Healthy Powered\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestPowerComponent());
            entity.AddComponent(new TestPowerComponentPoolable());
            Assert.IsTrue(aspect.Interests(entity), "Entity with {TestHealthComponent, TestPowerComponent, TestPowerComponentPoolable} must be subject to \"Healthy Powered\" Aspect");
        } 
        #endregion

        #region Aspect.One
        /// <summary>
        /// Tests Aspect.One with single type.
        /// </summary>
        /// <remarks>
        /// Should work exactly like Aspect.All with single type.
        /// Should work like negated Aspect.Exclude with same type.
        /// </remarks>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestAspectOneSingle()
        {
            Aspect aspect = Aspect.One(typeof(TestPowerComponent));
            EntityWorld entityWorld = new EntityWorld();
            Entity entity;

            entity = entityWorld.CreateEntity();
            Assert.IsFalse(aspect.Interests(entity), "Entity without components must NOT be subject to \"Powered\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            Assert.IsFalse(aspect.Interests(entity), "Entity with {TestHealthComponent} must NOT be subject to \"Powered\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestPowerComponent());
            Assert.IsTrue(aspect.Interests(entity), "Entity with {TestPowerComponent} must be subject to \"Powered\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestPowerComponent());
            entity.AddComponent(new TestHealthComponent());
            Assert.IsTrue(aspect.Interests(entity), "Entity with {TestPowerComponent, TestHealthComponent} must be subject to \"Powered\" Aspect");
        }

        /// <summary>
        /// Tests Aspect.One with multiple types, of which an entity must possess.
        /// </summary>
        /// <remarks>
        /// Should work like negated Aspect.Exclude with same set of types
        /// </remarks>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestAspectOneMultiple()
        {
            Aspect aspect = Aspect.One(typeof(TestPowerComponent), typeof(TestPowerComponentPoolable));
            EntityWorld entityWorld = new EntityWorld();
            Entity entity;

            entity = entityWorld.CreateEntity();
            Assert.IsFalse(aspect.Interests(entity), "Entity without components must NOT be subject to \"Powered by any means\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            Assert.IsFalse(aspect.Interests(entity), "Entity with {TestHealthComponent} must NOT be subject to \"Powered by any means\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestPowerComponent());
            Assert.IsTrue(aspect.Interests(entity), "Entity with {TestPowerComponent} must be subject to \"Powered by any means\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestPowerComponentPoolable());
            Assert.IsTrue(aspect.Interests(entity), "Entity with {TestPowerComponentPoolable} must be subject to \"Powered by any means\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestPowerComponent());
            Assert.IsTrue(aspect.Interests(entity), "Entity with {TestHealthComponent, TestPowerComponent} must be subject to \"Powered by any means\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestPowerComponent());
            entity.AddComponent(new TestPowerComponentPoolable());
            Assert.IsTrue(aspect.Interests(entity), "Entity with {TestHealthComponent, TestPowerComponent, TestPowerComponentPoolable} must be subject to \"Powered by any means\" Aspect");
        } 
        #endregion

        #region Aspect.Exclude
        /// <summary>
        /// Test Aspect.Exclude with single type excluded.
        /// </summary>
        /// <remarks>
        /// Should work like negated Aspect.One with same type
        /// </remarks>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestAspectExcludeSingle()
        {
            Aspect aspect = Aspect.Exclude(typeof(TestPowerComponent));
            EntityWorld entityWorld = new EntityWorld();
            Entity entity;

            entity = entityWorld.CreateEntity();
            Assert.IsTrue(aspect.Interests(entity), "Entity without components must be subject to \"Not Powered\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            Assert.IsTrue(aspect.Interests(entity), "Entity with {TestHealthComponent} must be subject to \"Not Powered\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestPowerComponent());
            Assert.IsFalse(aspect.Interests(entity), "Entity with {TestPowerComponent} must NOT be subject to \"Not Powered\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestPowerComponent());
            Assert.IsFalse(aspect.Interests(entity), "Entity with {TestHealthComponent, TestPowerComponent} must NOT be subject to \"Not Powered\" Aspect");
        }

        /// <summary>
        /// Test Aspect.Exclude with multiple types excluded
        /// </summary>
        /// <remarks>
        /// Should work like negated Aspect.One with same set of types
        /// </remarks>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestAspectExcludeMultiple()
        {
            Aspect aspect = Aspect.Exclude(typeof(TestPowerComponent), typeof(TestPowerComponentPoolable));
            EntityWorld entityWorld = new EntityWorld();
            Entity entity;

            entity = entityWorld.CreateEntity();
            Assert.IsTrue(aspect.Interests(entity), "Entity without components must be subject to \"Not Powered by any means\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            Assert.IsTrue(aspect.Interests(entity), "Entity with {TestHealthComponent} must be subject to \"Not Powered by any means\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestPowerComponent());
            Assert.IsFalse(aspect.Interests(entity), "Entity with {TestHealthComponent, TestPowerComponent} must NOT be subject to \"Not Powered by any means\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestPowerComponent());
            entity.AddComponent(new TestPowerComponentPoolable());
            Assert.IsFalse(aspect.Interests(entity), "Entity with {TestHealthComponent, TestPowerComponent, TestPowerComponentPoolable} must NOT be subject to \"Not Powered by any means\" Aspect");
        } 
        #endregion

        /// <summary>
        /// Tests Aspect combined of All, One, Exclude filters
        /// </summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestAspectAllOneExclude()
        {
            Aspect aspect = Aspect.Empty()
                .GetAll(typeof(TestHealthComponent))
                .GetOne(typeof(TestBaseComponent), typeof(TestDerivedComponent))
                .GetExclude(typeof(TestPowerComponent), typeof(TestPowerComponentPoolable));

            EntityWorld entityWorld = new EntityWorld();
            Entity entity;

            entity = entityWorld.CreateEntity();
            Assert.IsFalse(aspect.Interests(entity), "Entity without components must NOT be subject to \"(Healthy) && (Base || Derived) && !(Power || PowerPoolable)\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            Assert.IsFalse(aspect.Interests(entity), "Entity with {TestHealthComponent} must NOT be subject to \"(Healthy) && (Base || Derived) && !(Power || PowerPoolable)\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestBaseComponent());
            Assert.IsTrue(aspect.Interests(entity), "Entity with {TestHealthComponent, TestBaseComponent} must be subject to \"(Healthy) && (Base || Derived) && !(Power || PowerPoolable)\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestBaseComponent());
            entity.AddComponent(new TestDerivedComponent());
            Assert.IsTrue(aspect.Interests(entity), "Entity with {TestHealthComponent, TestBaseComponent, TestDerivedComponent} must be subject to \"(Healthy) && (Base || Derived) && !(Power || PowerPoolable)\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestBaseComponent());
            entity.AddComponent(new TestDerivedComponent());
            entity.AddComponent(new TestPowerComponent());
            Assert.IsFalse(aspect.Interests(entity), "Entity with {TestHealthComponent, TestBaseComponent, TestDerivedComponent, TestPowerComponent} must NOT be subject to \"(Healthy) && (Base || Derived) && !(Power || PowerPoolable)\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestDerivedComponent());
            entity.AddComponent(new TestPowerComponentPoolable());
            Assert.IsFalse(aspect.Interests(entity), "Entity with {TestHealthComponent, TestDerivedComponent, TestPowerComponentPoolable} must NOT be subject to \"(Healthy) && (Base || Derived) && !(Power || PowerPoolable)\" Aspect");

            entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestBaseComponent());
            entity.AddComponent(new TestDerivedComponent());
            entity.AddComponent(new TestPowerComponent());
            entity.AddComponent(new TestPowerComponentPoolable());
            Assert.IsFalse(aspect.Interests(entity), "Entity with {TestHealthComponent, TestBaseComponent, TestDerivedComponent, TestPowerComponent, TestPowerComponentPoolable} must NOT be subject to \"(Healthy) && (Base || Derived) && !(Power || PowerPoolable)\" Aspect");
        }
    }
}
