using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Artemis
{

    class QueueManager
    {
        public QueueManager()
        {
            this.AcquireLock();
            refCount++;
            this.ReleaseLock();
        }

        public void AcquireLock()       
        {
            Monitor.Enter(lockobj);
        }

        public void ReleaseLock()
        {
            Monitor.Exit(lockobj);
        }

        public int refCount = 0;
        static object lockobj = new object();
        public static Queue<Entity> queue = new Queue<Entity>();
        public static int EntitiesToProcessEachFrame = 50;
    }

    public abstract class QueueProcessingSystemThreadSafe : EntitySystem
    {
        public QueueProcessingSystemThreadSafe()
            : base()
        {
            Id = this.GetType();
            if (!queuesManager.ContainsKey(Id))
            {
                queuesManager[Id] = new QueueManager();
            }
            else
            {
                queuesManager[Id].AquireLock();
                queuesManager[Id].refCount++;
                queuesManager[Id].ReleaseLock();
            }
        }

        ~QueueProcessingSystemThreadSafe()
        {
            QueueManager QueueManager = queuesManager[Id];
            QueueManager.AcquireLock();
            QueueManager.refCount--;
            if (QueueManager.refCount == 0)
                queuesManager.Remove(Id);
            QueueManager.ReleaseLock();
        }

        public readonly Type Id;

        static Dictionary<Type, QueueManager> queuesManager = new Dictionary<Type, QueueManager>();

        public static void SetQueueProcessingLimit(int limit, Type EntitySystemType)
        {

            QueueManager QueueManager = queuesManager[EntitySystemType];
            QueueManager.AcquireLock();
            QueueManager.EntitiesToProcessEachFrame = limit;
            QueueManager.ReleaseLock();

        }

        public static int GetQueueProcessingLimit(Type EntitySystemType)
        {
            QueueManager QueueManager = queuesManager[EntitySystemType];
            QueueManager.AcquireLock();
            int val = QueueManager.EntitiesToProcessEachFrame;
            QueueManager.ReleaseLock();
            return val;
        }


        public static void AddToQueue(Entity ent, Type EntitySystemType)
        {
            QueueManager QueueManager = queuesManager[EntitySystemType];
            QueueManager.AcquireLock();
            QueueManager.queue.Enqueue(ent);
            QueueManager.ReleaseLock();
        }

        public static int QueueCount(Type EntitySystemType)
        {
            int result;
            QueueManager QueueManager = queuesManager[EntitySystemType];
            QueueManager.AquireLock();
            result = QueueManager.queue.Count;
            QueueManager.ReleaseLock();
            return result;
        }

        private static Entity DeQueue(Type EntitySystemType)
        {
            QueueManager QueueManager = queuesManager[EntitySystemType];
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
