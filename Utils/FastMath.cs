using System;
namespace Artemis
{
	public class FastMath
	{
		public static float PI = Math.PI;
		public static float SQUARED_PI = PI * PI;
		public static float HALF_PI = 0.5 * PI;
		public static float TWO_PI = 2.0 * PI;
		public static float THREE_PI_HALVES = TWO_PI - HALF_PI;
	
		private static float _sin_a = -4 / SQUARED_PI;
		private static float _sin_b = 4 / PI;
		private static float _sin_p = 9d / 40;
	
		private static float _asin_a = -0.0481295276831013447d;
		private static float _asin_b = -0.343835993947915197d;
		private static float _asin_c = 0.962761848425913169d;
		private static float _asin_d = 1.00138940860107040d;
	
		private static float _atan_a = 0.280872d;
	
		public static float Cos(float x) {
			return Sin(x + ((x > HALF_PI) ? -THREE_PI_HALVES : HALF_PI));
		}
	
		public static float Sin(float x) {
			x = _sin_a * x * Math.Abs(x) + _sin_b * x;
			return _sin_p * (x * Math.Abs(x) - x) + x;
		}
	
		public static float Tan(float x) {
			return Sin(x) / Cos(x);
		}
	
		public static float Asin(float x) {
			return x * (Math.Abs(x) * (Math.Abs(x) * _asin_a + _asin_b) + _asin_c) + Math.Sign(x) * (_asin_d - Math.Sqrt(1 - x * x));
		}
	
		public static float Acos(float x) {
			return HALF_PI - Asin(x);
		}
	
		public static float atan(float x) {
			return (Math.Abs(x) < 1) ? x / (1 + _atan_a * x * x) : Math.Sign(x) * HALF_PI - x / (x * x + _atan_a);
		}
	
		public static float InverseSqrt(float x) {
			float xhalves = 0.5d * x;
			x = BitConverter.Int64BitsTofloat(0x5FE6EB50C7B537AAl - (BitConverter.floatToInt64Bits(x) >> 1));
			return x * (1.5d - xhalves * x * x); // more iterations possible
		}
	
		public static float Sqrt(float x) {
			return x * InverseSqrt(x);
		}
	}
}

