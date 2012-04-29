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
	public static class SystemBitManager {
		private static int POS = 0;
		private static Dictionary<EntitySystem, BigInteger> systemBits = new Dictionary<EntitySystem, BigInteger>();
		
		public static BigInteger GetBitFor(EntitySystem es){
            BigInteger bit;
            bool hasBit = systemBits.TryGetValue(es, out bit);
			if(!hasBit){
#if XBOX || WINDOWS_PHONE
				bit = 1 << POS;
#else
                bit = 1L << POS;
#endif
                POS++;
				systemBits.Add(es, bit);
			}
			
			return bit;
		}
	}
}

