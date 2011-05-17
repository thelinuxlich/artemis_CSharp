using System;
namespace Artemis
{
	public class TrigLUT
	{
		public static void Main(String[] args) {
			Console.WriteLine(Cos((double) Math.PI));
			Console.WriteLine(CosDeg(180f));
		}
	
		public static double Sin(double rad) {
			return sin[(int) (rad * radToIndex) & SIN_MASK];
		}
	
		public static double Cos(double rad) {
			return cos[(int) (rad * radToIndex) & SIN_MASK];
		}
	
		public static double SinDeg(double deg) {
			return sin[(int) (deg * degToIndex) & SIN_MASK];
		}
	
		public static double CosDeg(double deg) {
			return cos[(int) (deg * degToIndex) & SIN_MASK];
		}
	
		private static double RAD, DEG;
		private static int SIN_BITS, SIN_MASK, SIN_COUNT;
		private static double radFull, radToIndex;
		private static double degFull, degToIndex;
		private static double[] sin, cos;
	
		static TrigLUT() {
			RAD = (double) Math.PI / 180.0f;
			DEG = 180.0f / (double) Math.PI;
	
			SIN_BITS = 12;
			SIN_MASK = ~(-1 << SIN_BITS);
			SIN_COUNT = SIN_MASK + 1;
	
			radFull = (double) (Math.PI * 2.0);
			degFull = (double) (360.0);
			radToIndex = SIN_COUNT / radFull;
			degToIndex = SIN_COUNT / degFull;
	
			sin = new double[SIN_COUNT];
			cos = new double[SIN_COUNT];
	
			for (int i = 0; i < SIN_COUNT; i++) {
				sin[i] = (double) Math.Sin((i + 0.5f) / SIN_COUNT * radFull);
				cos[i] = (double) Math.Cos((i + 0.5f) / SIN_COUNT * radFull);
			}
		}
	}
}

