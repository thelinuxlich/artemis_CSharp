using System;
namespace Artemis
{
	public class TrigLUT
	{
		public static void Main(String[] args) {
			Console.WriteLine(Cos((float)Math.PI));
			Console.WriteLine(CosDeg(180f));
		}
	
		public static float Sin(float rad) {
			return sin[(int) (rad * radToIndex) & SIN_MASK];
		}
	
		public static float Cos(float rad) {
			return cos[(int) (rad * radToIndex) & SIN_MASK];
		}
	
		public static float SinDeg(float deg) {
			return sin[(int) (deg * degToIndex) & SIN_MASK];
		}
	
		public static float CosDeg(float deg) {
			return cos[(int) (deg * degToIndex) & SIN_MASK];
		}
	
		private static float RAD, DEG;
		private static int SIN_BITS, SIN_MASK, SIN_COUNT;
		private static float radFull, radToIndex;
		private static float degFull, degToIndex;
		private static float[] sin, cos;
	
		static TrigLUT() {
			RAD = (float) Math.PI / 180.0f;
			DEG = 180.0f / (float) Math.PI;
	
			SIN_BITS = 12;
			SIN_MASK = ~(-1 << SIN_BITS);
			SIN_COUNT = SIN_MASK + 1;
	
			radFull = (float) (Math.PI * 2.0);
			degFull = (float) (360.0);
			radToIndex = SIN_COUNT / radFull;
			degToIndex = SIN_COUNT / degFull;
	
			sin = new float[SIN_COUNT];
			cos = new float[SIN_COUNT];
	
			for (int i = 0; i < SIN_COUNT; i++) {
				sin[i] = (float) Math.Sin((i + 0.5f) / SIN_COUNT * radFull);
				cos[i] = (float) Math.Cos((i + 0.5f) / SIN_COUNT * radFull);
			}
		}
	}
}

