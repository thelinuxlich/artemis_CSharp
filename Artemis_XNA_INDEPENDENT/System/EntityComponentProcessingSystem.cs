using Artemis.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.System
{
    public abstract class EntityProcessingSystem<T1> : EntitySystem
        where T1 : IComponent
    {
        public EntityProcessingSystem()
            : this(new Aspect())
        {
        }

        public EntityProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T1)))
        {
        }

        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity e in entities.Values)
            {
                this.Process(e, e.GetComponent<T1>());
            }
        }

        protected abstract void Process(Entity e, T1 t1);
    }
    public abstract class EntityProcessingSystem<T1, T2> : EntitySystem
        where T1 : IComponent
        where T2 : IComponent
    {
        public EntityProcessingSystem()
            : this(new Aspect())
        {
        }

        public EntityProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T1), typeof(T2)))
        {
        }

        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity e in entities.Values)
            {
                this.Process(e, e.GetComponent<T1>(), e.GetComponent<T2>());
            }
        }

        protected abstract void Process(Entity e, T1 t1, T2 t2);
    }
    public abstract class EntityProcessingSystem<T1, T2, T3> : EntitySystem
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
    {
        public EntityProcessingSystem()
            : this(new Aspect())
        {
        }

        public EntityProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T1), typeof(T2), typeof(T3)))
        {
        }

        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity e in entities.Values)
            {
                this.Process(e, e.GetComponent<T1>(), e.GetComponent<T2>(), e.GetComponent<T3>());
            }
        }

        protected abstract void Process(Entity e, T1 t1, T2 t2, T3 t3);
    }
    public abstract class EntityProcessingSystem<T1, T2, T3, T4> : EntitySystem
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
    {
        public EntityProcessingSystem()
            : this(new Aspect())
        {
        }

        public EntityProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T1), typeof(T2), typeof(T3), typeof(T4)))
        {
        }

        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity e in entities.Values)
            {
                this.Process(e, e.GetComponent<T1>(), e.GetComponent<T2>(), e.GetComponent<T3>(), e.GetComponent<T4>());
            }
        }

        protected abstract void Process(Entity e, T1 t1, T2 t2, T3 t3, T4 t4);
    }
    public abstract class EntityProcessingSystem<T1, T2, T3, T4, T5> : EntitySystem
        where T1 : IComponent
        where T2 : IComponent
        where T3 : IComponent
        where T4 : IComponent
        where T5 : IComponent
    {
        public EntityProcessingSystem()
            : this(new Aspect())
        {
        }

        public EntityProcessingSystem(Aspect aspect)
            : base(aspect.GetAll(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)))
        {
        }

        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            foreach (Entity e in entities.Values)
            {
                this.Process(e, e.GetComponent<T1>(), e.GetComponent<T2>(), e.GetComponent<T3>(), e.GetComponent<T4>(), e.GetComponent<T5>());
            }
        }

        protected abstract void Process(Entity e, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
    }
}
