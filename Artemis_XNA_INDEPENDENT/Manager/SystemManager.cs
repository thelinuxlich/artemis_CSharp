#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemManager.cs" company="GAMADU.COM">
//     Copyright � 2013 GAMADU.COM. All rights reserved.
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
//   Class SystemManager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Manager
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Reflection;
#if FULLDOTNET
    using global::System.Threading.Tasks;
#endif
    using Artemis.Attributes;
    using Artemis.Interface;
    using Artemis.System;
    using Artemis.Utils;
#if !FULLDOTNET
    using ParallelTasks;
#endif

    #endregion Using statements

    /// <summary>Class SystemManager.</summary>
    public sealed class SystemManager
    {
        /// <summary>The entity world.</summary>
        private readonly EntityWorld entityWorld;

        /// <summary>The systems.</summary>
        private readonly IDictionary<Type, List<EntitySystem>> systems;

        /// <summary>The merged bag.</summary>
        private readonly Bag<EntitySystem> mergedBag;
#if FULLDOTNET
        /// <summary>The factory.</summary>
        private readonly TaskFactory factory;
#endif
        /// <summary>The tasks.</summary>
        private readonly List<Task> tasks;

        /// <summary>The update layers.</summary>
        private IDictionary<int, Bag<EntitySystem>> updateLayers;

        /// <summary>The draw layers.</summary>
        private IDictionary<int, Bag<EntitySystem>> drawLayers;

#if FULLDOTNET
        /// <summary>Initializes a new instance of the <see cref="SystemManager" /> class.</summary>
        /// <param name="entityWorld">The entity world.</param>
        /// <param name="keepEntitiesSorted">if set to <c>true</c> [keep entities sorted].</param>
        internal SystemManager(EntityWorld entityWorld, bool keepEntitiesSorted = true)
#else
        /// <summary>Initializes a new instance of the <see cref="SystemManager" /> class.</summary>
        /// <param name="entityWorld">The entity world.</param>
        internal SystemManager(EntityWorld entityWorld)
#endif
        {
            this.tasks = new List<Task>();
            this.mergedBag = new Bag<EntitySystem>();
#if FULLDOTNET
            this.factory = new TaskFactory(TaskScheduler.Default);
            if (keepEntitiesSorted)
            {
                this.drawLayers = new SortedDictionary<int, Bag<EntitySystem>>();
                this.updateLayers = new SortedDictionary<int, Bag<EntitySystem>>();
            }
#else
            this.drawLayers = new Dictionary<int, Bag<EntitySystem>>();
            this.updateLayers = new Dictionary<int, Bag<EntitySystem>>();     
#endif
            this.systems = new Dictionary<Type, List<EntitySystem>>();
            this.entityWorld = entityWorld;
        }

        /// <summary>Gets the systems.</summary>
        /// <value>The systems.</value>
        public Bag<EntitySystem> Systems
        {
            get
            {
                return this.mergedBag;
            }
        }

        /// <summary>Sets the system.</summary>
        /// <typeparam name="T">The <see langword="Type"/> T.</typeparam>
        /// <param name="system">The system.</param>
        /// <param name="execType">Type of the exec.</param>
        /// <param name="layer">The layer.</param>
        /// <returns>The set system.</returns>
        public T SetSystem<T>(T system, ExecutionType execType, int layer = 0) where T : EntitySystem
        {
            system.EntityWorld = this.entityWorld;

            if (this.systems.ContainsKey(typeof(T)))
            {
                this.systems[typeof(T)].Add(system);
            }
            else
            {
                this.systems[typeof(T)] = new List<EntitySystem> { system };
            }

            switch (execType)
            {
                case ExecutionType.DrawAsynchronous:
                case ExecutionType.DrawSynchronous:
                    {
                        if (!this.drawLayers.ContainsKey(layer))
                        {
                            this.drawLayers[layer] = new Bag<EntitySystem>();
                        }

                        Bag<EntitySystem> drawBag = this.drawLayers[layer];
                        if (drawBag == null)
                        {
                            drawBag = this.drawLayers[layer] = new Bag<EntitySystem>();
                        }

                        if (!drawBag.Contains(system))
                        {
                            drawBag.Add(system);
                        }
#if FULLDOTNET
                    this.drawLayers = new SortedDictionary<int, Bag<EntitySystem>>((from d in this.drawLayers orderby d.Key ascending select d).ToDictionary(pair => pair.Key, pair => pair.Value));
#else
                    this.drawLayers = new Dictionary<int, Bag<EntitySystem>>((from d in this.drawLayers orderby d.Key ascending select d).ToDictionary(pair => pair.Key, pair => pair.Value));
#endif
                    }

                    break;
                case ExecutionType.UpdateAsynchronous:
                case ExecutionType.UpdateSynchronous:
                    {
                        if (!this.updateLayers.ContainsKey(layer))
                        {
                            this.updateLayers[layer] = new Bag<EntitySystem>();
                        }

                        Bag<EntitySystem> updateBag = this.updateLayers[layer];
                        if (updateBag == null)
                        {
                            updateBag = this.updateLayers[layer] = new Bag<EntitySystem>();
                        }

                        if (!updateBag.Contains(system))
                        {
                            updateBag.Add(system);
                        }
#if FULLDOTNET
                    this.updateLayers = new SortedDictionary<int, Bag<EntitySystem>>((from d in this.updateLayers orderby d.Key ascending select d).ToDictionary(pair => pair.Key, pair => pair.Value));
#else
                    this.updateLayers = new Dictionary<int, Bag<EntitySystem>>((from d in this.updateLayers orderby d.Key ascending select d).ToDictionary(pair => pair.Key, pair => pair.Value));
#endif
                    }

                    break;
            }

            if (!this.mergedBag.Contains(system))
            {
                this.mergedBag.Add(system);
            }

            system.SystemBit = SystemBitManager.GetBitFor(system);

            return system;
        }

        /// <summary>Gets the system.</summary>
        /// <typeparam name="T">The <see langword="Type"/> T.</typeparam>
        /// <returns>The List{EntitySystem} of systems.</returns>
        public List<EntitySystem> GetSystem<T>() where T : EntitySystem
        {
            List<EntitySystem> system;

            this.systems.TryGetValue(typeof(T), out system);

            return system;
        }

        /// <summary>Initializes all.</summary>
        /// <param name="processAttributes">if set to <see langword="true" /> [process attributes].</param>
        internal void InitializeAll(bool processAttributes)
        {
            if (processAttributes)
            {
                IDictionary<Type, List<Attribute>> types = AttributesProcessor.Process(AttributesProcessor.SupportedAttributes);
                foreach (KeyValuePair<Type, List<Attribute>> item in types)
                {
                    if (typeof(EntitySystem).IsAssignableFrom(item.Key))
                    {
                        Type type = item.Key;
                        ArtemisEntitySystem pee = (ArtemisEntitySystem)item.Value[0];
                        EntitySystem instance = (EntitySystem)Activator.CreateInstance(type);
                        this.SetSystem(instance, pee.ExecutionType, pee.Layer);
                    }
                    else if (typeof(IEntityTemplate).IsAssignableFrom(item.Key))
                    {
                        Type type = item.Key;
                        ArtemisEntityTemplate pee = (ArtemisEntityTemplate)item.Value[0];
                        IEntityTemplate instance = (IEntityTemplate)Activator.CreateInstance(type);
                        this.entityWorld.SetEntityTemplate(pee.Name, instance);
                    }
                    else if (typeof(ComponentPoolable).IsAssignableFrom(item.Key))
                    {
                        ArtemisComponentPool propertyComponentPool = null;

                        foreach (ArtemisComponentPool artemisComponentPool in item.Value.OfType<ArtemisComponentPool>())
                        {
                            propertyComponentPool = artemisComponentPool;
                        }

                        Type type = item.Key;
                        MethodInfo[] methods = type.GetMethods();

                        Func<Type, ComponentPoolable> create = null;
                        foreach (MethodInfo methodInfo in from methodInfo in methods let attributes = methodInfo.GetCustomAttributes(false) from attribute in attributes.OfType<ArtemisComponentCreate>() select methodInfo)
                        {
                            create = (Func<Type, ComponentPoolable>)Delegate.CreateDelegate(typeof(Func<Type, ComponentPoolable>), methodInfo);
                        }

                        if (create == null)
                        {
                            create = CreateInstance;
                        }

                        IComponentPool<ComponentPoolable> pool;

                        ////Type[] typeArgs = { type };
                        ////Type d1 = typeof(ComponentPool<>);
                        ////var typeGen = d1.MakeGenericType(typeArgs);
                        ////Activator.CreateInstance(typeGen, new object[] {PropertyComponentPool.InitialSize, PropertyComponentPool.IsResizable, create}

                        if (propertyComponentPool == null)
                        {
                            throw new Exception("propertyComponentPool is null.");
                        }

                        if (!propertyComponentPool.IsSupportMultiThread)
                        {
                            pool = new ComponentPool<ComponentPoolable>(propertyComponentPool.InitialSize, propertyComponentPool.ResizeSize, propertyComponentPool.IsResizable, create, type);
                        }
                        else
                        {
                            pool = new ComponentPoolMultiThread<ComponentPoolable>(propertyComponentPool.InitialSize, propertyComponentPool.ResizeSize, propertyComponentPool.IsResizable, create, type);
                        }

                        this.entityWorld.SetPool(type, pool);
                    }
                }
            }

            for (int index = 0, j = this.mergedBag.Size; index < j; ++index)
            {
                this.mergedBag.Get(index).Initialize();
            }
        }

        /// <summary>Updates the specified execution type.</summary>
        /// <param name="executionType">Type of the execution.</param>
        internal void Update(ExecutionType executionType)
        {
            switch (executionType)
            {
                case ExecutionType.UpdateSynchronous:
                    foreach (int item in this.updateLayers.Keys)
                    {
                        UpdateBagSynchronous(this.updateLayers[item]);
                    }

                    break;
                case ExecutionType.DrawSynchronous:
                    foreach (int item in this.drawLayers.Keys)
                    {
                        UpdateBagSynchronous(this.drawLayers[item]);
                    }

                    break;
                case ExecutionType.DrawAsynchronous:
                    foreach (int item in this.drawLayers.Keys)
                    {
                        this.UpdateBagAsynchronous(this.drawLayers[item]);
                    }

                    break;
                case ExecutionType.UpdateAsynchronous:
                    foreach (int item in this.updateLayers.Keys)
                    {
                        this.UpdateBagAsynchronous(this.updateLayers[item]);
                    }

                    break;
            }
        }

        /// <summary>Creates the instance.</summary>
        /// <param name="type">The type.</param>
        /// <returns>The specified ComponentPool-able instance.</returns>
        private static ComponentPoolable CreateInstance(Type type)
        {
            return (ComponentPoolable)Activator.CreateInstance(type);
        }

        /// <summary>Updates the bag synchronous.</summary>
        /// <param name="temp">The temp.</param>
        private static void UpdateBagSynchronous(Bag<EntitySystem> temp)
        {
            for (int index = 0, j = temp.Size; index < j; ++index)
            {
                temp.Get(index).Process();
            }
        }

        /// <summary>Updates the bag asynchronous.</summary>
        /// <param name="entitySystems">The entity systems.</param>
        private void UpdateBagAsynchronous(Bag<EntitySystem> entitySystems)
        {
            this.tasks.Clear();
            for (int index = 0, j = entitySystems.Size; index < j; ++index)
            {
                EntitySystem entitySystem = entitySystems.Get(index);
#if FULLDOTNET
                this.tasks.Add(this.factory.StartNew(entitySystem.Process));
#else
                this.tasks.Add(Parallel.Start(entitySystem.Process));
#endif
            }
#if FULLDOTNET
            Task.WaitAll(this.tasks.ToArray());
#else
            foreach (var item in this.tasks)
            {
                item.Wait();
            }
#endif
        }
    }
}