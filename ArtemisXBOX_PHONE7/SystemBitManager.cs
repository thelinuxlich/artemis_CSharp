using System;
using System.Collections.Generic;
using System.Numerics;
namespace Artemis
{
	public static class SystemBitManager {
		private static int POS = 0;
		private static Dictionary<EntitySystem, BigInteger> systemBits = new Dictionary<EntitySystem, BigInteger>();
		
		public static BigInteger GetBitFor(EntitySystem es){
            BigInteger bit;
            bool hasBit = systemBits.TryGetValue(es, out bit);
			if(!hasBit){
				bit = 1L << POS;
				POS++;
				systemBits.Add(es, bit);
			}
			
			return bit;
		}
	}
}

