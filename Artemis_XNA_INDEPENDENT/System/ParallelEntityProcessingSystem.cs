

namespace Artemis.System
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;
#if FULLDOTNET
    using global::System.Threading.Tasks;
#else
    using ParallelTasks;
#endif

    #endregion Using statements

    /// <summary>Class ParallelEntityProcessingSystem.</summary>
    public abstract class ParallelEntityProcessingSystem : EntitySystem
    {
#if FULLDOTNET
        /// <summary>The factory.</summary>
        private readonly TaskFactory factory;
#endif

        /// <summary>Initializes a new instance of the <see cref="ParallelEntityProcessingSystem"/> class.</summary>
        /// <param name="requiredType">Type of the required.</param>
        /// <param name="otherTypes">The other types.</param>
        protected ParallelEntityProcessingSystem(Type requiredType, params Type[] otherTypes)
            : base(GetMergedTypes(requiredType, otherTypes))
        {
#if FULLDOTNET
            this.factory = new TaskFactory(TaskScheduler.Default);
#endif
        }

        /// <summary>Initializes a new instance of the <see cref="EntitySystem" /> class.</summary>
        /// <param name="aspect">The aspect.</param>
        protected ParallelEntityProcessingSystem(Aspect aspect)
            : base(aspect)
        {
#if FULLDOTNET
            this.factory = new TaskFactory(TaskScheduler.Default);
#endif
        }

        /// <summary>Processes the specified entity.</summary>
        /// <param name="entity">The entity.</param>
        public abstract void Process(Entity entity);

        /// <summary>Processes the entities.</summary>
        /// <param name="entities">The entities.</param>
        protected override void ProcessEntities(IDictionary<int, Entity> entities)
        {
            float simultaneous = Environment.ProcessorCount * 2;
            int perThread = (int)Math.Ceiling((entities.Values.Count) / simultaneous);
            Entity[] threadEntities = new Entity[entities.Values.Count];
            entities.Values.CopyTo(threadEntities, 0);
            int numberOfEntities = entities.Values.Count - 1;
            List<Task> tasks = new List<Task>();

            for (int processorIndex = 0; processorIndex < simultaneous; ++processorIndex)
            {
                int initial = numberOfEntities;
#if FULLDOTNET
                tasks.Add(
                    this.factory.StartNew(
#else
                tasks.Add(Parallel.Start(
#endif
                        () =>
                            {
                                for (int spartialIndex = initial; spartialIndex > initial - perThread && spartialIndex >= 0; --spartialIndex)
                                {
                                    this.Process(threadEntities[spartialIndex]);
                                }
                            }));
                numberOfEntities -= perThread;
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