using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis
{
    public abstract class HybridQueueSystemProcessing : EntityProcessingSystem
    {
        public HybridQueueSystemProcessing(Type requiredType, params Type[] otherTypes)
            : base(requiredType, otherTypes)
        {
            foreach (var item in GetMergedTypes(requiredType, otherTypes))
	        {
		        compTypes.Add(ComponentTypeManager.GetTypeFor(item));
	        }
		}
        List<ComponentType> compTypes = new List<ComponentType>();
        public int EntitiesToProcessEachFrame = 50;        
        Queue<Entity> queue = new Queue<Entity>();              

        public void AddToQueue(Entity ent)
        {
            if (!this.Interests(ent))
                throw new Exception("This EntitySystem does not process this kind of entity" );
             
           queue.Enqueue(ent);         
        }

        public int QueueCount
        {
            get
            {            
               return queue.Count;             
            }
        }

        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            if (!enabled)
                return;

            int size = queue.Count > EntitiesToProcessEachFrame ? EntitiesToProcessEachFrame : queue.Count;
            for (int i = 0; i < size; i++)
            {
                Process(queue.Dequeue());
            }
            base.ProcessEntities(entities);
        }

    }
}
