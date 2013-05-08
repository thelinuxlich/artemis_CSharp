#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestEntityFactory.cs" company="GAMADU.COM">
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
// <author>Jens-Axel Grünewald</author>
// <date>2/23/2013 10:05:38 AM</date>
// <summary>
//     TThe class TestEntityFactory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace UnitTests
{
    #region Using statements

    using Artemis;

    using UnitTests.Component;

    #endregion

    /// <summary>The class TestEntityFactory.</summary>
    internal class TestEntityFactory
    {
        /// <summary>Creates the test health entity.</summary>
        /// <param name="entityWorld">The entity world.</param>
        /// <param name="group">The group.</param>
        /// <param name="tag">The tag.</param>
        /// <returns>The specified entity.</returns>
        public static Entity CreateTestHealthEntity(EntityWorld entityWorld, string group = "", string tag = "")
        {
            Entity entity = entityWorld.CreateEntity();
            entity.AddComponent(new TestHealthComponent());

            entity.GetComponent<TestHealthComponent>().Points = 100;

            if (!string.IsNullOrEmpty(group))
            {
                entity.Group = group;
            }

            if (!string.IsNullOrEmpty(tag))
            {
                entity.Tag = tag;
            }

            return entity;
        }

        /// <summary>Creates the test health entity with ID.</summary>
        /// <param name="entityWorld">The entity world.</param>
        /// <param name="id">The id.</param>
        /// <returns>The Entity.</returns>
        public static Entity CreateTestHealthEntityWithId(EntityWorld entityWorld, long id)
        {
            Entity entity = entityWorld.CreateEntity(id);
            entity.AddComponent(new TestHealthComponent());
            entity.GetComponent<TestHealthComponent>().Points = 100;            
            return entity;
        }

        /// <summary>Creates the test power1 entity.</summary>
        /// <param name="entityWorld">The entity world.</param>
        /// <returns>The specified entity.</returns>
        public static Entity CreateTestPowerEntity(EntityWorld entityWorld)
        {
            Entity entity = entityWorld.CreateEntity();

            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestPowerComponent());

            entity.GetComponent<TestHealthComponent>().Points = 100.0f;
            entity.GetComponent<TestPowerComponent>().Power = 100;

            return entity;
        }

        /// <summary>Creates the test power2 entity with pooled component.</summary>
        /// <param name="entityWorld">The entity world.</param>
        /// <returns>The specified entity.</returns>
        public static Entity CreateTestPowerEntityWithPooledComponent(EntityWorld entityWorld)
        {
            Entity entity = entityWorld.CreateEntity();

            TestPowerComponentPoolable testPower = entity.AddComponentFromPool<TestPowerComponentPoolable>();

            testPower.Power = 100;

            return entity;
        }

        /// <summary>Creates the test power2 entity.</summary>
        /// <param name="entityWorld">The entity world.</param>
        /// <returns>The specified entity.</returns>
        public static Entity CreateTestPower2Entity(EntityWorld entityWorld)
        {
            Entity entity = entityWorld.CreateEntity();

            entity.AddComponent(new TestHealthComponent());
            entity.AddComponent(new TestPowerComponentPoolable());

            entity.GetComponent<TestHealthComponent>().Points = 100.0f;
            entity.GetComponent<TestPowerComponentPoolable>().Power = 100;

            return entity;
        }
    }
}
