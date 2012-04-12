using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Artemis
{

    class QueueManager
    {
        public void AcquireLock()
        {
            Monitor.Enter(lockobj);
        }

        public void ReleaseLock()
        {
            Monitor.Exit(lockobj);
        }

        public static object lockobj = new object();
        public static Queue<Entity> queue = new Queue<Entity>();
        public static int EntitiesToProcessEachFrame = 50;
    }

    public abstract class QueueSystemProcessingThreadSafe : EntitySystem
    {
       public QueueSystemProcessingThreadSafe(String SystemName)
            : base()
        {
            Id = SystemName;
            queuesManager[Id] = new QueueManager();
        }

        ~QueueSystemProcessingThreadSafe()
        {
            queuesManager.Remove(Id);
        }

        public readonly String Id;        

        static Dictionary<String, QueueManager> queuesManager = new Dictionary<String, QueueManager>();

        public static void SetQueueProcessingLimit(int limit, String EntitySystemName)
        {
            QueueManager QueueManager = queuesManager[EntitySystemName];
            QueueManager.AcquireLock();
            QueueManager.EntitiesToProcessEachFrame = limit;
            QueueManager.ReleaseLock();
            
        }

        public static int GetQueueProcessingLimit(String EntitySystemName)
        {
            QueueManager QueueManager = queuesManager[EntitySystemName];
            QueueManager.AcquireLock();
            int val = QueueManager.EntitiesToProcessEachFrame;
            QueueManager.ReleaseLock();
            return val;
        }
           

        public static void AddToQueue(Entity ent, String EntitySystemName)
        {
            QueueManager QueueManager = queuesManager[EntitySystemName];
            QueueManager.AcquireLock();
            QueueManager.queue.Enqueue(ent);
            QueueManager.ReleaseLock();
        }

        public static int QueueCount(String EntitySystemName)
        {            
                int result;
                QueueManager QueueManager = queuesManager[EntitySystemName];
                QueueManager.AcquireLock();
                result = QueueManager.queue.Count;
                QueueManager.ReleaseLock();
                return result;         
        }


        private static Entity DeQueue(String EntitySystemName)
        {
            QueueManager QueueManager = queuesManager[EntitySystemName];
            QueueManager.AcquireLock();
            Entity e = QueueManager.queue.Dequeue();
            QueueManager.ReleaseLock();
            return e;
        }

        public virtual void Process(Entity Entity)
        {
        }

        public override void Process()
        {
            if (!enabled)
                return;
            Entity[] entities;
            QueueManager QueueManager = queuesManager[Id];
            QueueManager.AcquireLock();
            {
                int count = QueueManager.queue.Count;
                if (count > QueueManager.EntitiesToProcessEachFrame)
                {
                    entities = new Entity[QueueManager.EntitiesToProcessEachFrame];
                    for (int i = 0; i < QueueManager.EntitiesToProcessEachFrame; i++)
                    {
                        entities[i] = QueueManager.queue.Dequeue();
                    }
                }
                else
                {
                    entities = QueueManager.queue.ToArray();
                    QueueManager.queue.Clear();
                }
            }
            QueueManager.ReleaseLock();

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
