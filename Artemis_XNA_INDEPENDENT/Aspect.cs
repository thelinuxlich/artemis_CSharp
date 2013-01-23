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
    public class Aspect
    {
        protected BigInteger containsTypesMap = 0;
        protected BigInteger excludeTypesMap = 0;

        protected Aspect()
        {
        }

        public static Aspect AspectContains(params Type[] types)
        {            
            return new Aspect().Contains(types);         
        }

        public static Aspect AspectExclude(params Type[] types)
        {
            return new Aspect().Exclude(types);
        }
        
        public Aspect Contains(params Type[] types)
        {
            foreach (var item in types)
            {
                ComponentType ct = ComponentTypeManager.GetTypeFor(item);
                containsTypesMap |= ct.Bit;
            }            
            return this;
        }

        public Aspect Exclude(params Type[] types)
        {
            foreach (var item in types)
            {
                ComponentType ct = ComponentTypeManager.GetTypeFor(item);
                excludeTypesMap |= ct.Bit;
            }
            return this;
        }

        public virtual bool Interest(Entity e)
        {
            if (!(containsTypesMap > 0 || excludeTypesMap > 0))
                return false;

            return ((containsTypesMap & e.TypeBits) == containsTypesMap) && ((excludeTypesMap & e.TypeBits) != excludeTypesMap || excludeTypesMap == 0);
        }
    }  

}
