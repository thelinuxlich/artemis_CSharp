#if FULLDOTNET
using System.Threading.Tasks;
#else
using ParallelTasks;
#endif
using System;
using System.Collections.Generic;
namespace Artemis
{
    public abstract class ParallelEntityProcessingSystem : EntitySystem
    {

        /**
         * Create a new EntityProcessingSystem. It requires at least one component.
         * @param requiredType the required component type.
         * @param otherTypes other component types.
         */
        public ParallelEntityProcessingSystem(Type requiredType, params Type[] otherTypes)
            : base(GetMergedTypes(requiredType, otherTypes))
        {
        }

        public ParallelEntityProcessingSystem(Aspect aspect)
            : base(aspect)
        {
        }

        /**
         * Process a entity this system is interested in.
         * @param e the entity to process.
         */
        public abstract void Process(Entity e);

#if FULLDOTNET
        TaskFactory factory = new TaskFactory(TaskScheduler.Default);
#endif
        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {            
            float simultaneous = Environment.ProcessorCount *2;
            int perThread = (int) Math.Ceiling(((float)entities.Values.Count) / simultaneous);
            Entity[] ents = new Entity[entities.Values.Count];            
            entities.Values.CopyTo(ents,0);            
            int num = entities.Values.Count - 1;
            List<Task> tasks = new List<Task>();

            for (int j = 0; j < simultaneous; j++)
            {
                int initial = num;
#if FULLDOTNET
                tasks.Add(factory.StartNew(
#else
                tasks.Add(Parallel.Start(
#endif
                    () =>
                    {
                        for (int i = initial; i > initial - perThread && i >= 0; i--)
                        {
                            Process(ents[i]);
                        }
                    }
                ));
                num -= perThread;
            }
#if FULLDOTNET
            Task.WaitAll(tasks.ToArray());
#else
            foreach (var item in tasks)
            {
                item.Wait();
            }
#endif
        }
    }
}

