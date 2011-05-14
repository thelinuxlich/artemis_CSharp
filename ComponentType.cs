using System;
namespace Artemis
{
	public class ComponentType {
		private static long nextBit = 1;
		private static int nextId = 0;
		
		private long bit;
		private int id;
		
		public ComponentType() {
			Init();
		}
		
		private void Init() {
			bit = nextBit;
			nextBit = nextBit << 1;
			id = nextId++;
		}
		
		public long GetBit() {
			return bit;
		}
		
		public int GetId() {
			return id;
		}
	}
}

