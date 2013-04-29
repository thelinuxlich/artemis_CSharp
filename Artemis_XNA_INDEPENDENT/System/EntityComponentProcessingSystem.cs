using Artemis.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.System
{
    /// <summary>
    /// System which processes entities calling Process(Entity e, T t) every update.
    /// Automatically extracts the components specified by the type paremeters.
    /// </summary>
    /// <typeparam name="T1"> The first component. </typeparam>
    public abstract class EntityProcessingSystem<T1> : EntitySystem
        where T1 : IComponent
    {
        /// <summary>
        /// Constructs the system with an aspect which processes entities which have all the specified component types.
        /// </summary>
        public EntityProcessingSystem()
            : this(Aspect.Empty())
        {
        }

        /// <summary>
        /// Constructs the system with an aspect which processes entities which have all the specified component types
        /// as well as the any aditional constraints specified by the aspect.
        /// </summary>
        /// <param name="aspect"> The aspect specifying the additional constraints. </param>
        public EntityProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T1)))
        {
        }

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity e in entities.Values)
            {
                this.Process(e, e.GetComponent<T1>());
            }
        }

        /// <summary>
        /// Called every for every entity in this system with the components automatically passed as arguments.
        /// </summary>
        /// <param name="e"> The entity that is processed </param>
        /// <param name="t1"> The first component. </param>
        protected abstract void Process(Entity e, T1 t1);
    }

    /// <summary>
    /// System which processes entities calling Process(Entity e, T1 t1, T2 t2) every update.
    /// Automatically extracts the components specified by the type paremeters.
    /// </summary>
    /// <typeparam name="T1"> The first component. </typeparam>
    /// <typeparam name="T2"> The second component. </typeparam>
    public abstract class EntityProcessingSystem<T1, T2> : EntitySystem
        where T1 : IComponent
        where T2 : IComponent
    {
        /// <summary>
        /// Constructs the system with an aspect which processes entities which have all the specified component types.
        /// </summary>
        public EntityProcessingSystem()
            : this(Aspect.Empty())
        {
        }

        /// <summary>
        /// Constructs the system with an aspect which processes entities which have all the specified component types
        /// as well as the any aditional constraints specified by the aspect.
        /// </summary>
        /// <param name="aspect"> The aspect specifying the additional constraints. </param>
        public EntityProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T1), typeof(T2)))
        {
        }

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity e in entities.Values)
            {
                this.Process(e, e.GetComponent<T1>(), e.GetComponent<T2>());
            }
        }

        /// <summary>
        /// Called every for every entity in this system with the components automatically passed as arguments.
        /// </summary>
        /// <param name="e"> The entity that is processed </param>
        /// <param name="t1"> The first component. </param>
        /// <param name="t2"> The second component. </param>
        protected abstract void Process(Entity e, T1 t1, T2 t2);
    }

    /// <summary>
    /// System which processes entities calling Process(Entity e, T1 t1, T2 t2, T3 t3) every update.
    /// Automatically extracts the components specified by the type paremeters.
    /// </summary>
    /// <typeparam name="T1"> The first component. </typeparam>
    /// <typeparam name="T2"> The second component. </typeparam>
    /// <typeparam name="T3"> The third component. </typeparam>
    public abstract class EntityProcessingSystem<T1, T2, T3> : EntitySystem
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        /// <summary>
        /// Constructs the system with an aspect which processes entities which have all the specified component types.
        /// </summary>
        public EntityProcessingSystem()
            : this(Aspect.Empty())
        {
        }

        /// <summary>
        /// Constructs the system with an aspect which processes entities which have all the specified component types
        /// as well as the any aditional constraints specified by the aspect.
        /// </summary>
        /// <param name="aspect"> The aspect specifying the additional constraints. </param>
        public EntityProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T1), typeof(T2), typeof(T3)))
        {
        }

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity e in entities.Values)
            {
                this.Process(e, e.GetComponent<T1>(), e.GetComponent<T2>(), e.GetComponent<T3>());
            }
        }

        /// <summary>
        /// Called every for every entity in this system with the components automatically passed as arguments.
        /// </summary>
        /// <param name="e"> The entity that is processed </param>
        /// <param name="t1"> The first component. </param>
        /// <param name="t2"> The second component. </param>
        /// <param name="t3"> The third component. </param>
        protected abstract void Process(Entity e, T1 t1, T2 t2, T3 t3);
    }

    /// <summary>
    /// System which processes entities calling Process(Entity e, T1 t1, T2 t2, T3 t3, T4 t4) every update.
    /// Automatically extracts the components specified by the type paremeters.
    /// </summary>
    /// <typeparam name="T1"> The first component. </typeparam>
    /// <typeparam name="T2"> The second component. </typeparam>
    /// <typeparam name="T3"> The third component. </typeparam>
    /// <typeparam name="T4"> The fourth component. </typeparam>
    public abstract class EntityProcessingSystem<T1, T2, T3, T4> : EntitySystem
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        /// <summary>
        /// Constructs the system with an aspect which processes entities which have all the specified component types.
        /// </summary>
        public EntityProcessingSystem()
            : this(Aspect.Empty())
        {
        }

        /// <summary>
        /// Constructs the system with an aspect which processes entities which have all the specified component types
        /// as well as the any aditional constraints specified by the aspect.
        /// </summary>
        /// <param name="aspect"> The aspect specifying the additional constraints. </param>
        public EntityProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T1), typeof(T2), typeof(T3), typeof(T4)))
        {
        }

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity e in entities.Values)
            {
                this.Process(e, e.GetComponent<T1>(), e.GetComponent<T2>(), e.GetComponent<T3>(), e.GetComponent<T4>());
            }
        }

        /// <summary>
        /// Called every for every entity in this system with the components automatically passed as arguments.
        /// </summary>
        /// <param name="e"> The entity that is processed </param>
        /// <param name="t1"> The first component. </param>
        /// <param name="t2"> The second component. </param>
        /// <param name="t3"> The third component. </param>
        /// <param name="t4"> The fourth component. </param>
        protected abstract void Process(Entity e, T1 t1, T2 t2, T3 t3, T4 t4);
    }

    /// <summary>
    /// System which processes entities calling Process(Entity e, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) every update.
    /// Automatically extracts the components specified by the type paremeters.
    /// </summary>
    /// <typeparam name="T1"> The first component. </typeparam>
    /// <typeparam name="T2"> The second component. </typeparam>
    /// <typeparam name="T3"> The third component. </typeparam>
    /// <typeparam name="T4"> The fourth component. </typeparam>
    /// <typeparam name="T5"> The fifth component. </typeparam>
    public abstract class EntityProcessingSystem<T1, T2, T3, T4, T5> : EntitySystem
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        /// <summary>
        /// Constructs the system with an aspect which processes entities which have all the specified component types.
        /// </summary>
        public EntityProcessingSystem()
            : this(Aspect.Empty())
        {
        }

        /// <summary>
        /// Constructs the system with an aspect which processes entities which have all the specified component types
        /// as well as the any aditional constraints specified by the aspect.
        /// </summary>
        /// <param name="aspect"> The aspect specifying the additional constraints. </param>
        public EntityProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)))
        {
        }

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity e in entities.Values)
            {
                this.Process(e, e.GetComponent<T1>(), e.GetComponent<T2>(), e.GetComponent<T3>(), e.GetComponent<T4>(), e.GetComponent<T5>());
            }
        }

        /// <summary>
        /// Called every for every entity in this system with the components automatically passed as arguments.
        /// </summary>
        /// <param name="e"> The entity that is processed </param>
        /// <param name="t1"> The first component. </param>
        /// <param name="t2"> The second component. </param>
        /// <param name="t3"> The third component. </param>
        /// <param name="t4"> The fourth component. </param>
        /// <param name="t5"> The fifth component. </param>
        protected abstract void Process(Entity e, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
    }
}
