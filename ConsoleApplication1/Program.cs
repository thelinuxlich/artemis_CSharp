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
            SystemManager systemManager = world.SystemManager;            
            systemManager.InitializeAll();

            Entity et = world.CreateEntity();
            et.AddComponent(new Health());
            et.GetComponent<Health>().HP = 100;
            et.Refresh();


            Entity et1 = world.CreateEntityFromTemplate("teste");

            {
                DateTime dt = DateTime.Now;
                world.LoopStart();
                systemManager.UpdateSynchronous(ExecutionType.Update);
                Console.WriteLine((DateTime.Now - dt).TotalMilliseconds);
            }

            Debug.Assert(et.GetComponent<Health>().HP == 90);
            Debug.Assert(et1.GetComponent<Power>().POWER == 90);

        }
    }
}
