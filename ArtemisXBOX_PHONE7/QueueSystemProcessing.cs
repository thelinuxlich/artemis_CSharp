using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis
{
    public class QueueSystemProcessing : EntitySystem
    {
        public QueueSystemProcessing()
            : base()
        {
        }

        public int EntitiesToProcessEachFrame = 50;        
        Queue<Entity> queue = new Queue<Entity>();              

        public void AddToQueue(Entity ent)
        {
           queue.Enqueue(ent);         
        }

        public int QueueCount
        {
            get
            {            
               return queue.Count;             
            }
        }


        private Entity DeQueue()
        {            
                if(queue.Count > 0)
                {
                    return queue.Dequeue();
                }
                return null;            
        }

        public virtual void Process(Entity Entity)
        {
        }

        public override void Process()
        {
            if (!enabled)
                return;
            int size = queue.Count > EntitiesToProcessEachFrame ? EntitiesToProcessEachFrame : queue.Count;
            for (int i = 0; i < size; i++)
            {
                Process(queue.Dequeue());
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
