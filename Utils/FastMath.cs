using System;
namespace Artemis
{
	public class FastMath
	{
		public static sealed double PI = Math.PI;
		public static sealed double SQUARED_PI = PI * PI;
		public static sealed double HALF_PI = 0.5 * PI;
		public static sealed double TWO_PI = 2.0 * PI;
		public static sealed double THREE_PI_HALVES = TWO_PI - HALF_PI;
	
		private static sealed double _sin_a = -4 / SQUARED_PI;
		private static sealed double _sin_b = 4 / PI;
		private static sealed double _sin_p = 9d / 40;
	
		private sealed static double _asin_a = -0.0481295276831013447d;
		private sealed static double _asin_b = -0.343835993947915197d;
		private sealed static double _asin_c = 0.962761848425913169d;
		private sealed static double _asin_d = 1.00138940860107040d;
	
		private sealed static double _atan_a = 0.280872d;
	
		public sealed static double Cos(double x) {
			return Sin(x + ((x > HALF_PI) ? -THREE_PI_HALVES : HALF_PI));
		}
	
		public sealed static double Sin(double x) {
			x = _sin_a * x * Math.Abs(x) + _sin_b * x;
			return _sin_p * (x * Math.Abs(x) - x) + x;
		}
	
		public sealed static double Tan(double x) {
			return Sin(x) / Cos(x);
		}
	
		public sealed static double Asin(double x) {
			return x * (Math.Abs(x) * (Math.Abs(x) * _asin_a + _asin_b) + _asin_c) + Math.Sign(x) * (_asin_d - Math.Sqrt(1 - x * x));
		}
	
		public sealed static double Acos(double x) {
			return HALF_PI - Asin(x);
		}
	
		public sealed static double atan(double x) {
			return (Math.Abs(x) < 1) ? x / (1 + _atan_a * x * x) : Math.Sign(x) * HALF_PI - x / (x * x + _atan_a);
		}
	
		public sealed static double InverseSqrt(double x) {
			double xhalves = 0.5d * x;
			x = BitConverter.Int64BitsToDouble(0x5FE6EB50C7B537AAl - (BitConverter.DoubleToInt64Bits(x) >> 1));
			return x * (1.5d - xhalves * x * x); // more iterations possible
		}
	
		public sealed static double Sqrt(double x) {
			return x * InverseSqrt(x);
		}
	}
}

