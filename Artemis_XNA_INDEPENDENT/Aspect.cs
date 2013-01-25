using System;
using System.Collections.Generic;
#if !XBOX && !WINDOWS_PHONE
using System.Numerics;
#endif

#if XBOX || WINDOWS_PHONE
using BigInteger = System.Int32;
#endif

namespace Artemis
{
    /// <summary>
    /// Especify a Filter to filter what Entities (with what Components) a EntitySystem will Process
    /// </summary>
    public class Aspect
    {
        protected BigInteger containsTypesMap = 0;
        protected BigInteger excludeTypesMap = 0;
        protected BigInteger oneTypesMap = 0;

        protected Aspect()
        {
        }

        public static Aspect All(params Type[] types)
        {            
            return new Aspect()._All(types);         
        }

        public static Aspect Exclude(params Type[] types)
        {
            return new Aspect()._Exclude(types);
        }

        public static Aspect One(params Type[] types)
        {
            return new Aspect()._One(types);
        }

        public Aspect _One(params Type[] types)
        {
            System.Diagnostics.Debug.Assert(types != null);
            foreach (var item in types)
            {
                ComponentType ct = ComponentTypeManager.GetTypeFor(item);
                oneTypesMap |= ct.Bit;
            }
            return this;
        }

        public Aspect _All(params Type[] types)
        {
            System.Diagnostics.Debug.Assert(types != null);
            foreach (var item in types)
            {
                ComponentType ct = ComponentTypeManager.GetTypeFor(item);
                containsTypesMap |= ct.Bit;
            }            
            return this;
        }

        public Aspect _Exclude(params Type[] types)
        {
            System.Diagnostics.Debug.Assert(types != null);
            foreach (var item in types)
            {
                ComponentType ct = ComponentTypeManager.GetTypeFor(item);
                excludeTypesMap |= ct.Bit;
            }
            return this;
        }

        /// <summary>
        /// Called by the EntitySystem to determine if the system is interested in the passed Entity
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual bool Interests(Entity e)
        {
            if (!(containsTypesMap > 0 || excludeTypesMap > 0 || oneTypesMap > 0))
                return false;

            //Ajudazinha =P
            //10010 & 10000 = 10000
            //10010 | 10000 = 10010
            //10010 | 01000 = 11010

            //1001 & 0000 = 0000 OK
            //1001 & 0100 = 0000 NOK           
            //0011 & 1001 = 0001 Ok
            
            return ((oneTypesMap & e.TypeBits) != 0 || oneTypesMap == 0) && ((containsTypesMap & e.TypeBits) == containsTypesMap || containsTypesMap == 0) && ((excludeTypesMap & e.TypeBits) != excludeTypesMap || excludeTypesMap == 0);
        }
    }  

}
