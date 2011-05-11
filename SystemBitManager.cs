using System;
namespace Artemis
{
	public class SystemBitManager {
		private static int POS = 0;
		private static Dictionary<EntitySystem, Long> systemBits = new Dictionary<EntitySystem, Long>();
		
		protected static sealed long getBitFor(EntitySystem es){
			Long bit = systemBits[es];
			
			if(bit == null){
				bit = 1L << POS;
				POS++;
				systemBits.Add(es, bit);
			}
			
			return bit;
		}
	}
}

