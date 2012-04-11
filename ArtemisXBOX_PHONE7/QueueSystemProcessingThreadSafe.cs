using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis
{
    public class QueueSystemProcessingThreadSafe : EntitySystem
    {
        public QueueSystemProcessingThreadSafe()
            : base()
        {
        }

        public static int EntitiesToProcessEachFrame = 20;
        static object lockobj = new object();
        static Queue<Entity> queue = new Queue<Entity>();              

        public static void AddToQueue(Entity ent)
        {
            lock (lockobj)
            {
                queue.Enqueue(ent);
            }
        }

        private static int QueueCount()
        {
            lock (lockobj)
            {
                return queue.Count;
            }
        }


        private static Entity DeQueue()
        {
            lock (lockobj)
            {
                if(queue.Count > 0)
                {
                    return queue.Dequeue();
                }
                return null;
            }
        }

        public virtual void Process(Entity Entity)
        {
        }

        public override void Process()
        {
            Entity[] entities; 
            lock (lockobj)
            {
                int count = queue.Count;
                if (count > QueueSystemProcessingThreadSafe.EntitiesToProcessEachFrame)
                {
                    entities = new Entity[QueueSystemProcessingThreadSafe.EntitiesToProcessEachFrame];
                    for (int i = 0; i < QueueSystemProcessingThreadSafe.EntitiesToProcessEachFrame; i++)
                    {
                        entities[i] = queue.Dequeue();
                    }
                }
                else
                {
                       entities = queue.ToArray();
                }
            }

            foreach (var item in entities)
            {
                Process(item);
            }           
        }

        public override void Initialize()
        {
        }

        public override void Change(Entity e)
        {
        }

        public override void Removed(Entity e)
        {
        }

        public override void Added(Entity e)
        {
        }
    }
}
