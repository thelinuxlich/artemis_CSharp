#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneralTest.cs" company="GAMADU.COM">
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
//   The general test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace UnitTests
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Diagnostics;
    using global::System.Linq;

    using Artemis;
    using Artemis.Interface;
    using Artemis.Manager;
    using Artemis.System;
    using Artemis.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using UnitTests.Component;
    using UnitTests.System;

    #endregion Using statements

    /// <summary>The general test.</summary>
    [TestClass]
    public class GeneralTest
    {
        /// <summary>The component pool.</summary>
        private static readonly Dictionary<Type, Bag<IComponent>> ComponentPool = new Dictionary<Type, Bag<IComponent>>();

        /// <summary>The health bag.</summary>
        private static readonly Bag<IComponent> HealthBag = new Bag<IComponent>();

        /// <summary>The attributes tests method.</summary>
        [TestMethod]
        public void AttributesTestsMethod()
        {
            EntityWorld entityWorld = new EntityWorld
                                          {
                                              PoolCleanupDelay = 1
                                          };
            entityWorld.InitializeAll();

            Debug.Assert(entityWorld.SystemManager.Systems.Count == 2, "Number of Systems is not 2.");

            Entity entity1 = entityWorld.CreateEntity();
            Power2Component power = entity1.AddComponentFromPool<Power2Component>();
            power.Power = 100;
            entity1.Refresh();

            Entity entity2 = entityWorld.CreateEntityFromTemplate("test");
            Debug.Assert(entity2 != null, "Entity from test template is null.");
            {
                entityWorld.Update();
            }

            entity1.RemoveComponent<Power2Component>();
            entity1.Refresh();
            {
                entityWorld.Update();
            }

            entity1.AddComponentFromPool<Power2Component>();
            entity1.GetComponent<Power2Component>().Power = 100;
            entity1.Refresh();

            entityWorld.Update();
        }

        /// <summary>The dummy tests.</summary>
        [TestMethod]
        public void DummyTests()
        {
            EntityWorld entityWorld = new EntityWorld();
            SystemManager systemManager = entityWorld.SystemManager;
            DummyCommunicationSystem dummyCommunicationSystem = new DummyCommunicationSystem();
            systemManager.SetSystem(dummyCommunicationSystem, GameLoopType.Update);
            entityWorld.InitializeAll(false);

            for (int index = 0; index < 100; ++index)
            {
                Entity entity = entityWorld.CreateEntity();
                entity.AddComponent(new HealthComponent());
                entity.GetComponent<HealthComponent>().Points += 100;
                entity.Group = "test";
                entity.Refresh();
            }

            {
                Entity entity = entityWorld.CreateEntity();
                entity.Tag = "tag";
                entity.AddComponent(new HealthComponent());
                entity.GetComponent<HealthComponent>().Points += 100;
                entity.Refresh();
            }

            {
                DateTime dateTime = DateTime.Now;
                entityWorld.Update();
                Console.WriteLine((DateTime.Now - dateTime).TotalMilliseconds);
            }

            Debug.Assert(entityWorld.TagManager.GetEntity("tag") != null, "Tagged entity not found.");
            Debug.Assert(entityWorld.GroupManager.GetEntities("test").Count == 100, "Test entity size is not 100.");
            Debug.Assert(entityWorld.EntityManager.ActiveEntitiesCount == 101, "Number of active entities is not 101.");
            Debug.Assert(entityWorld.SystemManager.Systems.Count == 1, "Number of Systems is not 1.");
        }

        /// <summary>The hybrid queue system test.</summary>
        [TestMethod]
        public void HybridQueueSystemTest()
        {
            EntityWorld entityWorld = new EntityWorld();
            SystemManager systemManager = entityWorld.SystemManager;
            HybridQueueSystem hybridQueueSystem = new HybridQueueSystem();
            systemManager.SetSystem(hybridQueueSystem, GameLoopType.Update);
            entityWorld.InitializeAll(false);

            List<Entity> entities = new List<Entity>();
            for (int index = 0; index < 100; ++index)
            {
                Entity entity = entityWorld.CreateEntity();
                entity.AddComponent(new HealthComponent());
                entity.GetComponent<HealthComponent>().Points += 100;
                entity.Refresh();
                entities.Add(entity);
            }

            for (int index = 0; index < 100; ++index)
            {
                Entity entity = entityWorld.CreateEntity();
                entity.AddComponent(new HealthComponent());
                entity.GetComponent<HealthComponent>().Points += 100;
                hybridQueueSystem.AddToQueue(entity);
                entities.Add(entity);
            }

            int numberOfQueues = 0;
            while (hybridQueueSystem.QueueCount > 0)
            {
                ++numberOfQueues;
                DateTime dateTime = DateTime.Now;
                entityWorld.Update();
                Console.WriteLine((DateTime.Now - dateTime).TotalMilliseconds);
            }

            for (int index = 0; index < 100; ++index)
            {
                Debug.Assert(Math.Abs(entities[index].GetComponent<HealthComponent>().Points - (100 - (10 * numberOfQueues))) < float.Epsilon, "Queue is not 100.");
            }

            for (int index = 100; index < 200; ++index)
            {
                Debug.Assert(Math.Abs(entities[index].GetComponent<HealthComponent>().Points - 90) < float.Epsilon, "Health is not 90.");
            }
        }

        /// <summary>The most simple system ever test.</summary>
        [TestMethod]
        public void MostSimpleSystemEverTest()
        {
            EntityWorld entityWorld = new EntityWorld();
            SystemManager systemManager = entityWorld.SystemManager;
            Simple1System dummyCommunicationSystem = new Simple1System();
            systemManager.SetSystem(dummyCommunicationSystem, GameLoopType.Update);
            entityWorld.InitializeAll(false);

            Entity entity1 = entityWorld.CreateEntity();
            entity1.AddComponent(new HealthComponent());
            entity1.GetComponent<HealthComponent>().Points = 100;
            entity1.Refresh();

            Entity entity2 = entityWorld.CreateEntity();
            entity2.AddComponent(new HealthComponent());
            entity2.AddComponent(new Power1Component());
            entity2.GetComponent<HealthComponent>().Points = 100;
            entity2.GetComponent<Power1Component>().Power = 100;
            entity2.Refresh();
            {
                DateTime dateTime = DateTime.Now;
                entityWorld.Update();
                Console.WriteLine((DateTime.Now - dateTime).TotalMilliseconds);
            }

            Debug.Assert(Math.Abs(entity1.GetComponent<HealthComponent>().Points - 90) < float.Epsilon, "Health is not 90.");
            Debug.Assert(Math.Abs(entity2.GetComponent<HealthComponent>().Points - 100) < float.Epsilon, "Health is not 100.");
            Debug.Assert(Math.Abs(entity2.GetComponent<Power1Component>().Power - 100) < float.Epsilon, "Power is not 100.");
        }

        /// <summary>The queue system test.</summary>
        [TestMethod]
        public void QueueSystemTest()
        {
            EntityWorld entityWorld = new EntityWorld();
            SystemManager systemManager = entityWorld.SystemManager;
            Queue1System queue1System = new Queue1System();
            Queue1System queue1SystemTest2 = new Queue1System();
            systemManager.SetSystem(queue1System, GameLoopType.Update);
            systemManager.SetSystem(queue1SystemTest2, GameLoopType.Update);

            Queue2System queue2SystemTest3 = new Queue2System();
            systemManager.SetSystem(queue2SystemTest3, GameLoopType.Update);

            entityWorld.InitializeAll(false);

            QueueSystemProcessingThreadSafe.SetQueueProcessingLimit(20, queue1System.Id);
            Debug.Assert(QueueSystemProcessingThreadSafe.GetQueueProcessingLimit(queue1System.Id) == QueueSystemProcessingThreadSafe.GetQueueProcessingLimit(queue1SystemTest2.Id), "QueueProcessingLimit reached.");

            Debug.Assert(QueueSystemProcessingThreadSafe.GetQueueProcessingLimit(queue2SystemTest3.Id) != QueueSystemProcessingThreadSafe.GetQueueProcessingLimit(queue1SystemTest2.Id), "QueueProcessingLimit not reached.");

            QueueSystemProcessingThreadSafe.SetQueueProcessingLimit(1000, queue1System.Id);
            QueueSystemProcessingThreadSafe.SetQueueProcessingLimit(2000, queue2SystemTest3.Id);

            List<Entity> entities1 = new List<Entity>();
            for (int index = 0; index < 1000000; ++index)
            {
                Entity entity = entityWorld.CreateEntity();
                entity.AddComponent(new HealthComponent());
                entity.GetComponent<HealthComponent>().Points = 100;
                QueueSystemProcessingThreadSafe.AddToQueue(entity, queue1System.Id);
                entities1.Add(entity);
            }

            List<Entity> entities2 = new List<Entity>();
            for (int index = 0; index < 1000000; ++index)
            {
                Entity entity = entityWorld.CreateEntity();
                entity.AddComponent(new HealthComponent());
                entity.GetComponent<HealthComponent>().Points = 100;
                QueueSystemProcessingThreadSafe.AddToQueue(entity, queue2SystemTest3.Id);
                entities2.Add(entity);
            }

            Console.WriteLine("Start");
            while (QueueSystemProcessingThreadSafe.QueueCount(queue1System.Id) > 0 || QueueSystemProcessingThreadSafe.QueueCount(queue2SystemTest3.Id) > 0)
            {
                DateTime dateTime = DateTime.Now;
                entityWorld.Update(ExecutionType.Asynchronous);
                Console.WriteLine("Count: " + QueueSystemProcessingThreadSafe.QueueCount(queue1System.Id));
                Console.WriteLine("Time: " + (DateTime.Now - dateTime).TotalMilliseconds);
            }

            Console.WriteLine("End");

            foreach (Entity item in entities1)
            {
                Debug.Assert(Math.Abs(item.GetComponent<HealthComponent>().Points - 90) < float.Epsilon, "Health points is not 90.");
            }

            foreach (Entity item in entities2)
            {
                Debug.Assert(Math.Abs(item.GetComponent<HealthComponent>().Points - 80) < float.Epsilon, "Health points is not 80.");
            }
        }

        /// <summary>The second most simple system ever test.</summary>
        [TestMethod]
        public void SecondMostSimpleSystemEverTest()
        {
            EntityWorld entityWorld = new EntityWorld();
            entityWorld.InitializeAll();

            Entity entity1 = entityWorld.CreateEntity();
            entity1.AddComponent(new HealthComponent());
            entity1.GetComponent<HealthComponent>().Points = 100;
            entity1.Refresh();

            Entity entity2 = entityWorld.CreateEntity();
            entity2.AddComponent(new Power1Component());
            entity2.GetComponent<Power1Component>().Power = 100;
            entity2.Refresh();
            {
                entityWorld.Update();
            }

            // two systems running
            // each remove 10 HealthPoints
            Debug.Assert(Math.Abs(entity1.GetComponent<HealthComponent>().Points - 80) < float.Epsilon, "Health points is not 80.");
            Debug.Assert(Math.Abs(entity2.GetComponent<Power1Component>().Power - 90) < float.Epsilon, "Power is not 90.");
        }

        /// <summary>Systems the communication test.</summary>
        [TestMethod]
        public void SystemCommunicationTest()
        {
            EntitySystem.BlackBoard.SetEntry("Damage", 5);

            EntityWorld entityWorld = new EntityWorld();
            SystemManager systemManager = entityWorld.SystemManager;
            DummyCommunicationSystem dummyCommunicationSystem = new DummyCommunicationSystem();
            systemManager.SetSystem(dummyCommunicationSystem, GameLoopType.Update);
            entityWorld.InitializeAll(false);

            List<Entity> entities = new List<Entity>();
            for (int index = 0; index < 100; ++index)
            {
                Entity entity = entityWorld.CreateEntity();
                entity.AddComponent(new HealthComponent());
                entity.GetComponent<HealthComponent>().Points += 100;
                entity.Refresh();
                entities.Add(entity);
            }

            {
                DateTime dateTime = DateTime.Now;
                entityWorld.Update();
                Console.WriteLine((DateTime.Now - dateTime).TotalMilliseconds);
            }

            EntitySystem.BlackBoard.SetEntry("Damage", 10);
            {
                DateTime dateTime = DateTime.Now;
                entityWorld.Update();
                Console.WriteLine((DateTime.Now - dateTime).TotalMilliseconds);
            }

            foreach (Entity item in entities)
            {
                Debug.Assert(Math.Abs(item.GetComponent<HealthComponent>().Points - 85) < float.Epsilon, "Health points is not 85.");
            }
        }

        /// <summary>The multi.</summary>
        [TestMethod]
        public void Multi()
        {
            HealthBag.Add(new HealthComponent());
            HealthBag.Add(new HealthComponent());
            ComponentPool.Add(typeof(HealthComponent), HealthBag);

            EntityWorld entityWorld = new EntityWorld();
            SystemManager systemManager = entityWorld.SystemManager;
            entityWorld.EntityManager.RemovedComponentEvent += RemovedComponent;
            entityWorld.EntityManager.RemovedEntityEvent += RemovedEntity;

            systemManager.SetSystem(new RenderMultiHealthBarSystem(), GameLoopType.Update);
            entityWorld.InitializeAll(false);

            List<Entity> entities = new List<Entity>();
            for (int index = 0; index < 1000; ++index)
            {
                Entity entity = entityWorld.CreateEntity();
                entity.AddComponent(new HealthComponent());
                entity.GetComponent<HealthComponent>().Points += 100;
                entity.Refresh();
                entities.Add(entity);
            }

            for (int index = 0; index < 100; ++index)
            {
                DateTime dateTime = DateTime.Now;
                entityWorld.Update();
                Console.WriteLine((DateTime.Now - dateTime).TotalMilliseconds);
            }

            int df = entities.Count(item => Math.Abs(item.GetComponent<HealthComponent>().Points - 90) < float.Epsilon);

            Console.WriteLine("Found {0} entities with health of 90.", df);
        }

        /// <summary>The multi system.</summary>
        [TestMethod]
        public void MultiSystem()
        {
            HealthBag.Clear();
            ComponentPool.Clear();

            HealthBag.Add(new HealthComponent());
            HealthBag.Add(new HealthComponent());
            ComponentPool.Add(typeof(HealthComponent), HealthBag);

            EntityWorld entityWorld = new EntityWorld();
            SystemManager systemManager = entityWorld.SystemManager;
            entityWorld.EntityManager.RemovedComponentEvent += RemovedComponent;
            entityWorld.EntityManager.RemovedEntityEvent += RemovedEntity;
            systemManager.SetSystem(new RenderSingleHealthBarSystem(), GameLoopType.Update);
            systemManager.SetSystem(new Dummy1System(), GameLoopType.Update);
            systemManager.SetSystem(new Dummy2System(), GameLoopType.Update);
            systemManager.SetSystem(new Dummy3System(), GameLoopType.Update);
            entityWorld.InitializeAll(false);

            List<Entity> entities = new List<Entity>();
            for (int index = 0; index < 100000; ++index)
            {
                Entity entity = entityWorld.CreateEntity();
                entity.AddComponent(new HealthComponent());
                entity.GetComponent<HealthComponent>().Points += 100;
                entity.Refresh();
                entities.Add(entity);
            }

            for (int index = 0; index < 100; ++index)
            {
                DateTime dateTime = DateTime.Now;
                entityWorld.Update(ExecutionType.Asynchronous);

                // systemManager.UpdateSynchronous(ExecutionType.Update);
                Console.WriteLine((DateTime.Now - dateTime).TotalMilliseconds);
            }

            /*
            int df = 0;
            foreach (Entity entity in entities)
            {
                if (Math.Abs(entity.GetComponent<HealthComponent>().Points - 90) < float.Epsilon)
                {
                    df++;
                }
                else
                {
                    Console.WriteLine("Error " + df);
                }
            }
            */
        }

        /// <summary>The removed component.</summary>
        /// <param name="entity">The entity.</param>
        /// <param name="component">The component.</param>
        private static void RemovedComponent(Entity entity, IComponent component)
        {
            Console.WriteLine("This was the component removed: " + component.GetType());
            Bag<IComponent> tempBag;
            if (ComponentPool.TryGetValue(component.GetType(), out tempBag))
            {
                Console.WriteLine("Health Component Pool has " + tempBag.Count + " objects");
                tempBag.Add(component);
            }

            if (ComponentPool.TryGetValue(component.GetType(), out tempBag))
            {
                Console.WriteLine("Health Component Pool now has " + tempBag.Count + " objects");
            }
        }

        /// <summary>The removed entity.</summary>
        /// <param name="entity">The entity.</param>
        private static void RemovedEntity(Entity entity)
        {
            Console.WriteLine("The entity {0} was removed successfully.", entity.UniqueId);
        }
    }
}