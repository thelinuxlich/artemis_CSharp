using System;
using System.Collections.Generic;
namespace Artemis
{
	public class SystemBitManager {
		private static int POS = 0;
		private static Dictionary<EntitySystem, long> systemBits = new Dictionary<EntitySystem, long>();
		
		public static sealed long GetBitFor(EntitySystem es){
			long bit = systemBits[es];
			
			if(bit == null){
				bit = 1L << POS;
				POS++;
				systemBits.Add(es, bit);
			}
			
			return bit;
		}
	}
}

