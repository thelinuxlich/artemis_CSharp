#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemManager.cs" company="GAMADU.COM">
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
#if FULLDOTNET || CLIENTPROFILE
    using global::System.Threading.Tasks;
#endif
    using Artemis.Attributes;
    using Artemis.Interface;
    using Artemis.System;
    using Artemis.Utils;
#if (!FULLDOTNET && !PORTABLE && !METRO) && !CLIENTPROFILE
    using ParallelTasks;
#endif

#if METRO
    using Parallel =  global::System.Threading.Tasks.Parallel;
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

        /// <summary>The update layers.</summary>
        private IDictionary<int, Bag<EntitySystem>[]> updateLayers;

        /// <summary>The draw layers.</summary>
        private IDictionary<int, Bag<EntitySystem>[]> drawLayers;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemManager" /> class.
        /// </summary>
        /// <param name="entityWorld">The entity world.</param>
        internal SystemManager(EntityWorld entityWorld)
        {
            this.mergedBag = new Bag<EntitySystem>();
#if FULLDOTNET
                this.drawLayers = new SortedDictionary<int, Bag<EntitySystem>[]>();
                this.updateLayers = new SortedDictionary<int, Bag<EntitySystem>[]>();                
#else
                this.drawLayers = new Dictionary<int, Bag<EntitySystem>[]>();
                this.updateLayers = new Dictionary<int, Bag<EntitySystem>[]>();                
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

        /// <summary>
        /// Sets the system.
        /// </summary>
        /// <typeparam name="T">The <see langword="Type" /> T.</typeparam>
        /// <param name="system">The system.</param>
        /// <param name="gameLoopType">Type of the game loop.</param>
        /// <param name="layer">The layer.</param>
        /// <param name="executionType">Type of the execution.</param>
        /// <returns>
        /// The set system.
        /// </returns>
        public T SetSystem<T>(T system, GameLoopType gameLoopType, int layer = 0, ExecutionType executionType = ExecutionType.Synchronous) where T : EntitySystem
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

            switch (gameLoopType)
            {
                case GameLoopType.Draw:
                    {
                        
                            if (!this.drawLayers.ContainsKey(layer))
                            {
                                this.drawLayers[layer] = new Bag<EntitySystem>[2];
                                this.drawLayers[layer][0] = new Bag<EntitySystem>();
                                this.drawLayers[layer][1] = new Bag<EntitySystem>();
                            }

                            Bag<EntitySystem> drawBag  = null;
                            if(executionType == ExecutionType.Synchronous)
                                drawBag = this.drawLayers[layer][0];
                            else
                                drawBag = this.drawLayers[layer][1];
                            
                            if (!drawBag.Contains(system))
                            {
                                drawBag.Add(system);
                            }
                                                        
#if !FULLDOTNET                            
                        this.drawLayers = (from d in this.drawLayers orderby d.Key ascending select d).ToDictionary(pair => pair.Key, pair => pair.Value);                            
#endif
                        
                    }

                    break;
                case GameLoopType.Update:
                    {
                        
                            if (!this.updateLayers.ContainsKey(layer))
                            {
                                this.updateLayers[layer] = new Bag<EntitySystem>[2];
                                this.updateLayers[layer][0] = new Bag<EntitySystem>();
                                this.updateLayers[layer][1] = new Bag<EntitySystem>();
                            }


                            Bag<EntitySystem> updateBag = null;
                            if (executionType == ExecutionType.Synchronous)
                                updateBag = this.updateLayers[layer][0];
                            else
                                updateBag = this.updateLayers[layer][1];
                            
                            if (!updateBag.Contains(system))
                            {
                                updateBag.Add(system);
                            }                            
#if !FULLDOTNET
                            this.updateLayers = (from d in this.updateLayers orderby d.Key ascending select d).ToDictionary(pair => pair.Key, pair => pair.Value);
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
        /// <param name="assembliesToScan">The assemblies to scan.</param>
        /// <exception cref="System.Exception">propertyComponentPool is null.</exception>
        internal void InitializeAll(bool processAttributes, IEnumerable<Assembly> assembliesToScan = null)
        {
            if (processAttributes)
            {
                IDictionary<Type, List<Attribute>> types;
                if (assembliesToScan == null)
                {
                    types = AttributesProcessor.Process(AttributesProcessor.SupportedAttributes);
                }
                else
                {
                    types = AttributesProcessor.Process(AttributesProcessor.SupportedAttributes, assembliesToScan);
                }
                foreach (KeyValuePair<Type, List<Attribute>> item in types)
                {
                    if (typeof(EntitySystem).GetTypeInfo().IsAssignableFrom(item.Key.GetTypeInfo()))
                    {
                        Type type = item.Key;
                        ArtemisEntitySystem pee = (ArtemisEntitySystem)item.Value[0];
                        EntitySystem instance = (EntitySystem)Activator.CreateInstance(type);
                        this.SetSystem(instance, pee.GameLoopType, pee.Layer, pee.ExecutionType);
                    }
                    else if (typeof(IEntityTemplate).GetTypeInfo().IsAssignableFrom(item.Key.GetTypeInfo()))
                    {
                        Type type = item.Key;
                        ArtemisEntityTemplate pee = (ArtemisEntityTemplate)item.Value[0];
                        IEntityTemplate instance = (IEntityTemplate)Activator.CreateInstance(type);
                        this.entityWorld.SetEntityTemplate(pee.Name, instance);
                    }
                    else if (typeof(ComponentPoolable).GetTypeInfo().IsAssignableFrom(item.Key.GetTypeInfo()))
                    {
                        ArtemisComponentPool propertyComponentPool = null;

                        foreach (ArtemisComponentPool artemisComponentPool in item.Value.OfType<ArtemisComponentPool>())
                        {
                            propertyComponentPool = artemisComponentPool;
                        }

                        Type type = item.Key;
#if METRO            
                        IEnumerable<MethodInfo> methods = type.GetRuntimeMethods();
#else
                        MethodInfo[] methods = type.GetMethods();
#endif


                        Func<Type, ComponentPoolable> create = null;
                        foreach (MethodInfo methodInfo in from methodInfo in methods let attributes = methodInfo.GetCustomAttributes(false) from attribute in attributes.OfType<ArtemisComponentCreate>() select methodInfo)
                        {
                            create = (Func<Type, ComponentPoolable>) methodInfo.CreateDelegate(typeof(Func<Type, ComponentPoolable>));
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
                            throw new NullReferenceException("propertyComponentPool is null.");
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

            for (int index = 0, j = this.mergedBag.Count; index < j; ++index)
            {
                this.mergedBag.Get(index).LoadContent();
            }
        }

        /// <summary>Terminates all.</summary>
        internal void TerminateAll()
        {
            for (int index = 0; index < this.Systems.Count; ++index)
            {
                EntitySystem entitySystem = this.Systems.Get(index);
                entitySystem.UnloadContent();
            }

            this.Systems.Clear();
        }

        /// <summary>
        /// Updates the specified execution type.
        /// </summary>
        internal void Update()
        {
                foreach (int item in this.updateLayers.Keys)
                    {
                        if (this.updateLayers[item][0].Count > 0)
                        {
                            ProcessBagSynchronous(this.updateLayers[item][0]);
                        }
#if !PORTABLE
                        if (this.updateLayers[item][1].Count > 0)
                        {
                            ProcessBagAsynchronous(this.updateLayers[item][1]);
                        }      
#endif
                    }            

}

        /// <summary>
        /// Updates the specified execution type.
        /// </summary>
        internal void Draw()
        {

                foreach (int item in this.drawLayers.Keys)
                {
                    if (this.drawLayers[item][0].Count > 0)
                    {
                        ProcessBagSynchronous(this.drawLayers[item][0]);
                    }
#if !PORTABLE
                    if (this.drawLayers[item][1].Count > 0)
                    {
                        ProcessBagAsynchronous(this.drawLayers[item][1]);
                    }
#endif
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
        /// <param name="entitySystems">The entitySystems.</param>
        private void ProcessBagSynchronous(Bag<EntitySystem> entitySystems)
        {
            for (int index = 0, j = entitySystems.Count; index < j; ++index)
            {
                entitySystems.Get(index).Process();
            }
        }

#if !PORTABLE
        /// <summary>Updates the bag asynchronous.</summary>
        /// <param name="entitySystems">The entity systems.</param>
        private static void ProcessBagAsynchronous(IEnumerable<EntitySystem> entitySystems)
        {
            Parallel.ForEach(entitySystems, entitySystem => entitySystem.Process());
        }
#endif
    }
}