using System;
#if !XBOX && !WINDOWS_PHONE
using System.Numerics;
#endif

#if XBOX || WINDOWS_PHONE
using BigInteger = System.Int32;
#endif

namespace Artemis
{
	public sealed class ComponentType {
		private static BigInteger nextBit = 1;
		private static int nextId = 0;
		
		private BigInteger bit;
		private int id;
		
		public ComponentType() {
			Init();
		}
		
		private void Init() {
			bit = nextBit;
			nextBit = nextBit << 1;
			id = nextId++;
		}
		
		public BigInteger Bit {
			get { return bit;}
		}
		
		public int Id {
			get { return id; }
		}
	}
}

