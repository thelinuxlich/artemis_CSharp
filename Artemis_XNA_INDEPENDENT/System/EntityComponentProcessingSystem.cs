#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityComponentProcessingSystem.cs" company="GAMADU.COM">
//     Copyright © 2013 GAMADU.COM. All rights reserved.
//     Redistribution and use in source and binary forms, with or without modification, are
//     permitted provided that the following conditions are met:
//        1. Redistributions of source code must retain the above copyright notice, this list of
//           conditions and the following disclaimer.
//        2. Redistributions in binary form must reproduce the above copyright notice, this list
//           of conditions and the following disclaimer in the documentation and/or other materials
//           provided with the distribution.
//     THIS SOFTWARE IS PROVIDED BY GAMADU.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
//     WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//     FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
//     CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//     CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//     SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//     ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//     ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//     The views and conclusions contained in the software and documentation are those of the
//     authors and should not be interpreted as representing official policies, either expressed
//     or implied, of GAMADU.COM.
// </copyright>
// <summary>
//   System which processes entities calling Process(Entity entity, T component) every update.
//   Automatically extracts the components specified by the type parameters.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.System
{
    #region Using statements

    using global::System.Collections.Generic;

    using Artemis.Interface;

    #endregion

    // TODO: GrafSeismo: To be honest, this class is the worst design i have ever seen and should be removed or replaced by anything that do not limit generics by its count of arguments. Also to replace a useful class (EntityProcessingSystem) by this is also a very bad idea in a framework, you may first set an "Obsolete" attribute to get rid of old classes/interfaces. In my game i use the original version near by for any case.

    /// <summary>System which processes entities calling Process(Entity entity, T component) every update.
    /// Automatically extracts the components specified by the type parameters.</summary>
    /// <typeparam name="T">The component.</typeparam>
    public abstract class EntityComponentProcessingSystem<T> : EntitySystem
        where T : IComponent
    {
        /// <summary>Initializes a new instance of the <see cref="EntityComponentProcessingSystem{T}"/> class with an aspect which processes entities which have all the specified component types.</summary>
        protected EntityComponentProcessingSystem()
            : this(Aspect.Empty())
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EntityComponentProcessingSystem{T}"/> class with an aspect which processes entities which have all the specified component types as well as the any additional constraints specified by the aspect.</summary>
        /// <param name="aspect">The aspect specifying the additional constraints.</param>
        protected EntityComponentProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T)))
        {
        }

        /// <summary>Called for every entity in this system with the components automatically passed as arguments.</summary>
        /// <param name="entity">The entity that is processed </param>
        /// <param name="component1">The component.</param>
        public abstract void Process(Entity entity, T component1);

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity entity in entities.Values)
            {
                this.Process(entity, entity.GetComponent<T>());
            }
        }
    }

    /// <summary>System which processes entities calling Process(Entity entity, T1 component1, T2 component2) every update. 
    /// Automatically extracts the components specified by the type parameters.</summary>
    /// <typeparam name="T1">The first component.</typeparam>
    /// <typeparam name="T2">The second component.</typeparam>
    public abstract class EntityComponentProcessingSystem<T1, T2> : EntitySystem
        where T1 : IComponent
        where T2 : IComponent
    {
        /// <summary>Initializes a new instance of the <see cref="EntityComponentProcessingSystem{T1, T2}"/> class 
        /// with an aspect which processes entities which have all the specified component types.</summary>
        protected EntityComponentProcessingSystem()
            : this(Aspect.Empty())
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EntityComponentProcessingSystem{T1, T2}"/> class 
        /// with an aspect which processes entities which have all the specified component types as well as 
        /// the any additional constraints specified by the aspect.</summary>
        /// <param name="aspect"> The aspect specifying the additional constraints. </param>
        protected EntityComponentProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T1), typeof(T2)))
        {
        }

        /// <summary>Called every for every entity in this system with the components automatically passed as arguments.</summary>
        /// <param name="entity">The entity that is processed </param>
        /// <param name="component1">The first component.</param>
        /// <param name="component2">The second component.</param>
        public abstract void Process(Entity entity, T1 component1, T2 component2);

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity entity in entities.Values)
            {
                this.Process(entity, entity.GetComponent<T1>(), entity.GetComponent<T2>());
            }
        }
    }

    /// <summary>
    /// System which processes entities calling Process(Entity entity, T1 component1, T2 component2, T3 component3) every update.
    /// Automatically extracts the components specified by the type parameters.
    /// </summary>
    /// <typeparam name="T1">The first component.</typeparam>
    /// <typeparam name="T2">The second component.</typeparam>
    /// <typeparam name="T3">The third component.</typeparam>
    public abstract class EntityComponentProcessingSystem<T1, T2, T3> : EntitySystem
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        /// <summary>Initializes a new instance of the <see cref="EntityComponentProcessingSystem{T1, T2, T3}"/> class 
        /// with an aspect which processes entities which have all the specified component types.</summary>
        protected EntityComponentProcessingSystem()
            : this(Aspect.Empty())
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EntityComponentProcessingSystem{T1, T2, T3}"/> class 
        /// with an aspect which processes entities which have all the specified component types
        /// as well as the any additional constraints specified by the aspect.</summary>
        /// <param name="aspect">The aspect specifying the additional constraints.</param>
        protected EntityComponentProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T1), typeof(T2), typeof(T3)))
        {
        }

        /// <summary>Called for every entity in this system with the components automatically passed as arguments.</summary>
        /// <param name="entity">The entity that is processed</param>
        /// <param name="component1">The first component.</param>
        /// <param name="component2">The second component.</param>
        /// <param name="component3">The third component.</param>
        public abstract void Process(Entity entity, T1 component1, T2 component2, T3 component3);

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity entity in entities.Values)
            {
                this.Process(entity, entity.GetComponent<T1>(), entity.GetComponent<T2>(), entity.GetComponent<T3>());
            }
        }
    }

    /// <summary>System which processes entities calling Process(Entity entity, T1 t1, T2 t2, T3 t3, T4 t4) every update.
    /// Automatically extracts the components specified by the type parameters.</summary>
    /// <typeparam name="T1">The first component.</typeparam>
    /// <typeparam name="T2">The second component.</typeparam>
    /// <typeparam name="T3">The third component.</typeparam>
    /// <typeparam name="T4">The fourth component.</typeparam>
    public abstract class EntityComponentProcessingSystem<T1, T2, T3, T4> : EntitySystem
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        /// <summary>Initializes a new instance of the <see cref="EntityComponentProcessingSystem{T1, T2, T3, T4}"/> class 
        /// with an aspect which processes entities which have all the specified component types.</summary>
        protected EntityComponentProcessingSystem()
            : this(Aspect.Empty())
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EntityComponentProcessingSystem{T1, T2, T3, T4}"/> class 
        /// with an aspect which processes entities which have all the specified component types
        /// as well as the any additional constraints specified by the aspect.</summary>
        /// <param name="aspect">The aspect specifying the additional constraints.</param>
        protected EntityComponentProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T1), typeof(T2), typeof(T3), typeof(T4)))
        {
        }

        /// <summary>Called every for every entity in this system with the components automatically passed as arguments.</summary>
        /// <param name="entity">The entity that is processed</param>
        /// <param name="component1">The first component.</param>
        /// <param name="component2">The second component.</param>
        /// <param name="component3">The third component.</param>
        /// <param name="component4">The fourth component.</param>
        public abstract void Process(Entity entity, T1 component1, T2 component2, T3 component3, T4 component4);

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity entity in entities.Values)
            {
                this.Process(entity, entity.GetComponent<T1>(), entity.GetComponent<T2>(), entity.GetComponent<T3>(), entity.GetComponent<T4>());
            }
        }
    }

    /// <summary>System which processes entities calling Process(Entity entity, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) every update.
    /// Automatically extracts the components specified by the type parameters.</summary>
    /// <typeparam name="T1">The first component.</typeparam>
    /// <typeparam name="T2">The second component.</typeparam>
    /// <typeparam name="T3">The third component.</typeparam>
    /// <typeparam name="T4">The fourth component.</typeparam>
    /// <typeparam name="T5">The fifth component.</typeparam>
    public abstract class EntityComponentProcessingSystem<T1, T2, T3, T4, T5> : EntitySystem
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        /// <summary>Initializes a new instance of the <see cref="EntityComponentProcessingSystem{T1, T2, T3, T4, T5}"/> class 
        /// with an aspect which processes entities which have all the specified component types.</summary>
        protected EntityComponentProcessingSystem()
            : this(Aspect.Empty())
        {
        }

        /// <summary>Initializes a new instance of the <see cref="EntityComponentProcessingSystem{T1, T2, T3, T4, T5}"/> class 
        /// with an aspect which processes entities which have all the specified component types
        /// as well as the any additional constraints specified by the aspect.</summary>
        /// <param name="aspect">The aspect specifying the additional constraints.</param>
        protected EntityComponentProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)))
        {
        }

        /// <summary>Called every for every entity in this system with the components automatically passed as arguments.</summary>
        /// <param name="entity">The entity that is processed</param>
        /// <param name="component1">The first component.</param>
        /// <param name="component2">The second component.</param>
        /// <param name="component3">The third component.</param>
        /// <param name="component4">The fourth component.</param>
        /// <param name="component5">The fifth component.</param>
        public abstract void Process(Entity entity, T1 component1, T2 component2, T3 component3, T4 component4, T5 component5);

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity entity in entities.Values)
            {
                this.Process(entity, entity.GetComponent<T1>(), entity.GetComponent<T2>(), entity.GetComponent<T3>(), entity.GetComponent<T4>(), entity.GetComponent<T5>());
            }
        }
    }
}
