using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;
using ArtemisTest.Components;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtemisTest
{
    [TestClass]
    class AttributesTests
    {
        [TestMethod]
        public void Teste()
        {
            EntityWorld world = new EntityWorld();
            world.PoolCleanupDelay = 1;
            world.InitializeAll();

            Debug.Assert(world.SystemManager.Systems.Count() == 2);

            Entity et = world.CreateEntity();
            var power = et.AddComponentFromPool<Power2>();                
            power.POWER = 100;                        
            et.Refresh();

            Entity et1 = world.CreateEntityFromTemplate("teste");
            Debug.Assert(et1!=null);
            
            {             
                world.Update(ExecutionType.UpdateSyncronous);            
            }

            et.RemoveComponent<Power>();            
            et.Refresh();

            {            
                world.Update(ExecutionType.UpdateSyncronous);                
            }

            et.AddComponentFromPool<Power2>();
            et.GetComponent<Power2>().POWER = 100;
            et.Refresh();

            
            world.Update(ExecutionType.UpdateSyncronous);                
        }
    }
}
