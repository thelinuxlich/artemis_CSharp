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
		protected int referenceContainsType = -1;
		protected int referenceExcludeType = -1;
		protected int referenceOneType = -1;
		static int IdGenerator = 0;
		static Bag<Aspect> aspectsCache = new Bag<Aspect>();
		protected int Id = 0;
		protected bool executed = false;
		protected Bag<Entity> allEntitiesCache = new Bag<Entity>();
		protected Bag<Entity> containsEntitiesCache = new Bag<Entity>();
		protected Bag<Entity> oneEntitiesCache = new Bag<Entity>();
		protected Bag<Entity> excludeEntitiesCache = new Bag<Entity>();

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

		public static void FinishBuildAll()
		{
			for (int i = 0; i < aspectsCache.Size; i++) {
				Aspect a = aspectsCache.Get(i);
				for (int j = 0; j < aspectsCache.Size;j++) {
					Aspect b = aspectsCache.Get(j);
					if(a.Id != b.Id && (a.containsTypesMap & b.containsTypesMap) == a.containsTypesMap && a.containsTypesMap > b.containsTypesMap) {
						a.referenceContainsType = b.Id;
					} else if(a.Id != b.Id && (a.excludeTypesMap & b.excludeTypesMap) == a.excludeTypesMap && a.excludeTypesMap > b.excludeTypesMap) {
						a.referenceExcludeType = b.Id;
					} else if(a.Id != b.Id && a.referenceOneType == -1 && a.oneTypesMap == b.oneTypesMap) {
						a.referenceOneType = b.Id;
					}
				}
				if(a.referenceContainsType != -1) {
					for (int j = 0; j < aspectsCache.Size;j++) {
						Aspect b = aspectsCache.Get(j);
						if(b.referenceContainsType == a.Id) {
							b.referenceContainsType = a.referenceContainsType;
						}
					}
				}
				if(a.referenceExcludeType != -1) {
					for (int j = 0; j < aspectsCache.Size;j++) {
						Aspect b = aspectsCache.Get(j);
						if(b.referenceExcludeType == a.Id) {
							b.referenceExcludeType = a.referenceExcludeType;
						}
					}
				}
			}
		}

		public static void ClearEntitiesCache()
		{
			for (int i = 0; i < aspectsCache.Size; i++) {
				Aspect a = aspectsCache.Get(i);
				a.containsEntitiesCache.Clear();
				a.oneEntitiesCache.Clear();
				a.excludeEntitiesCache.Clear();
				a.allEntitiesCache.Clear();
				a.executed = false;
			}
		}

		public void FinishBuild()
		{
			Id = IdGenerator;
			Aspect.IdGenerator++;
			Aspect.aspectsCache.Add(this);
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

		private Aspect GetFather(int id,EntityManager em) {
			Aspect father = Aspect.aspectsCache.Get (id);
			if(!father.executed) {
				father.InterestedEntities(em);
			}
			return father;
		}

		public Bag<Entity> InterestedEntities (EntityManager em)
		{
			Bag<Entity> cache;
			if (!executed) {
				if (referenceContainsType != -1) {
					Aspect father = GetFather (referenceContainsType,em);
					cache = containsEntitiesCache = father.containsEntitiesCache;
				} else if(referenceOneType != -1) {
					Aspect father = GetFather (referenceOneType,em);
					cache = oneEntitiesCache = father.oneEntitiesCache;
				} else if(referenceExcludeType != -1) {
					Aspect father = GetFather (referenceExcludeType,em);
					cache = excludeEntitiesCache = father.excludeEntitiesCache;
				} else {
					cache = em.ActiveEntities;
				}
				for (int i = 0; i < cache.Size; i++) {
					Entity e = cache.Get (i);
					if (Interests (e)) {
						allEntitiesCache.Add (e);
					}
				}
				executed = true;
			} 
			return allEntitiesCache;
		}

        /// <summary>
        /// Called by the EntitySystem to determine if the system is interested in the passed Entity
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual bool Interests (Entity e)
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
			var interested = true;
			if (referenceExcludeType == -1 && excludeTypesMap != 0) {
				if ((excludeTypesMap & e.TypeBits) != excludeTypesMap) {
					excludeEntitiesCache.Add (e);
				} else {
					interested = false;
				}
			}
			if (referenceOneType == -1 && oneTypesMap != 0) {
				if ((oneTypesMap & e.TypeBits) != 0) {
					oneEntitiesCache.Add(e);
				} else {
					interested = false;
				}
			}
			if (referenceContainsType == -1 && containsTypesMap != 0) {
				if ((containsTypesMap & e.TypeBits) == containsTypesMap) {
					containsEntitiesCache.Add (e);
				} else {
					interested = false;
				}
			}
            return interested;
        }
    }  

}
