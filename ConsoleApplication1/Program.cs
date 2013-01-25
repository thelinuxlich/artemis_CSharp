using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using ArtemisTest;
using ArtemisTest.Components;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            EntityWorld world = new EntityWorld();
            world.PoolCleanupDelay = 1;
            world.InitializeAll();

            Entity et = world.CreateEntity();
            et.AddComponentFromPool<Power>().POWER = 100;                        
            et.Refresh();

            Entity et1 = world.CreateEntityFromTemplate("teste");

            {
                DateTime dt = DateTime.Now;
                world.Update(ExecutionType.UpdateSyncronous);
                Console.WriteLine((DateTime.Now - dt).TotalMilliseconds);
            }

            et.RemoveComponent<Power>();            
            et.Refresh();

            {
                DateTime dt = DateTime.Now;
                world.Update(ExecutionType.UpdateSyncronous);
                Console.WriteLine((DateTime.Now - dt).TotalMilliseconds);
            }

            et.AddComponentFromPool<Power>();
            et.GetComponent<Power>().POWER = 100;
            et.Refresh();
        }
    }
}
