using System;
using System.Collections.Generic;
using System.Linq;

namespace Artemis
{
    public sealed class SystemManager {
        private World world;
        private Dictionary<Type, EntitySystem> systems = new Dictionary<Type, EntitySystem>();
        private Bag<EntitySystem> bagged = new Bag<EntitySystem>();

        
        public SystemManager(World world) {
            this.world = world;
        }


        public T SetSystem<T>(T system, SystemType systemType, int processOrder) where T : EntitySystem{
            system.SetProcessOrder(processOrder);
            system.SetSystemType(systemType);

            return SetSystem(system);
        }

        public T SetSystem<T>(T system, SystemType systemType) where T : EntitySystem{
            int processOrder = bagged.Size();

            return SetSystem(system, systemType, processOrder);
        }

        public T SetSystem<T>(T system) where T : EntitySystem {
            system.SetWorld(world);
            
            systems.Add(typeof(T), system);
            
            if(!bagged.Contains(system))
                bagged.Add(system);
            
            system.SetSystemBit(SystemBitManager.GetBitFor(system));
            
            return system;
        }
        
        public T GetSystem<T>() where T : EntitySystem {
            EntitySystem system;
            systems.TryGetValue(typeof(T), out system);
            return (T)system;
        }
        
        public Bag<EntitySystem> GetSystems() {
            return bagged;
        }
        
        /**
         * After adding all systems to the world, you must initialize them all.
         */
        public void InitializeAll() {
           for (int i = 0, j = bagged.Size(); i < j; i++) {
              bagged.Get(i).Initialize();
           }
        } 
    
        public void ProcessSystemsForSystemType(SystemType type)
        {
            foreach (var system in systems.Values.Where(s=>s.GetSystemType() == type).OrderBy(s=>s.GetProcessOrder())){
                system.Process();
            }
        }

        public void ProcessAll()
        {
            for (int i = 0, j = bagged.Size(); i < j; i++)
            {
                bagged.Get(i).Process();
            }
        }
    }

    public enum SystemType
    {
        Render,
        Update
    }
}