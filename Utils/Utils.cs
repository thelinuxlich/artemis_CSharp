using System;
using System.IO;
namespace Artemis
{
	public class Utils
	{
		public static float CubicInterpolation(float v0, float v1, float v2, float v3, float t) {
			float t2 = t * t;
			float a0 = v3 - v2 - v0 + v1;
			float a1 = v0 - v1 - a0;
			float a2 = v2 - v0;
			float a3 = v1;
	
			return (a0 * (t * t2)) + (a1 * t2) + (a2 * t) + a3;
		}
	
		public static float QuadraticBezierInterpolation(float a, float b, float c, float t) {
			return (((1f - t) * (1f - t)) * a) + (((2f * t) * (1f - t)) * b) + ((t * t) * c);
		}
	
		public static float LengthOfQuadraticBezierCurve(float x0, float y0, float x1, float y1, float x2, float y2) {
			if ((x0 == x1 && y0 == y1) || (x1 == x2 && y1 == y2)) {
				return Distance(x0, y0, x2, y2);
			}
	
			float ax, ay, bx, by;
			ax = x0 - 2 * x1 + x2;
			ay = y0 - 2 * y1 + y2;
			bx = 2 * x1 - 2 * x0;
			by = 2 * y1 - 2 * y0;
			float A = 4 * (ax * ax + ay * ay);
			float B = 4 * (ax * bx + ay * by);
			float C = bx * bx + by * by;
	
			float Sabc = 2f * (float) Math.Sqrt(A + B + C);
			float A_2 = (float) Math.Sqrt(A);
			float A_32 = 2f * A * A_2;
			float C_2 = 2f * (float) Math.Sqrt(C);
			float BA = B / A_2;
	
			return (A_32 * Sabc + A_2 * B * (Sabc - C_2) + (4f * C * A - B * B) * (float) Math.Log((2 * A_2 + BA + Sabc) / (BA + C_2))) / (4 * A_32);
		}
	
		public static float Lerp(float a, float b, float t) {
			if (t < 0)
				return a;
			return a + t * (b - a);
		}
	
		public static float Distance(float x1, float y1, float x2, float y2) {
			return EuclideanDistance(x1, y1, x2, y2);
		}
	
		public static bool DoCirclesCollide(float x1, float y1, float radius1, float x2, float y2, float radius2) {
			float dx = x2 - x1;
			float dy = y2 - y1;
			float d = radius1 + radius2;
			return (dx * dx + dy * dy) < (d * d);
		}
	
		public static float EuclideanDistanceSq2D(float x1, float y1, float x2, float y2) {
			float dx = x1 - x2;
			float dy = y1 - y2;
			return dx * dx + dy * dy;
		}
	
		public static float ManhattanDistance(float x1, float y1, float x2, float y2) {
			return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
		}
	
		public static float EuclideanDistance(float x1, float y1, float x2, float y2) {
			float a = x1 - x2;
			float b = y1 - y2;
	
			return (float) FastMath.Sqrt(a * a + b * b);
		}
	
		public static float AngleInDegrees(float ownerRotation, float x1, float y1, float x2, float y2) {
			return Math.Abs(ownerRotation - AngleInDegrees(x1, y1, x2, y2)) % 360;
		}
	
		public static float AngleInDegrees(float originX, float originY, float targetX, float targetY) {
			return (float)Math.Atan2(targetY - originY, targetX - originX) * (180.0f / (float)Math.PI);
		}
	
		public static float AngleInRadians(float originX, float originY, float targetX, float targetY) {
			return (float) Math.Atan2(targetY - originY, targetX - originX);
		}
	
		public static bool ShouldRotateCounterClockwise(float angleFrom, float angleTo) {
			float diff = (angleFrom - angleTo) % 360;
			return diff > 0 ? diff < 180 : diff < -180;
		}
	
		public static float GetRotatedX(float currentX, float currentY, float pivotX, float pivotY, float angleDegrees) {
			float x = currentX - pivotX;
			float y = currentY - pivotY;
			float xr = (x * TrigLUT.CosDeg(angleDegrees)) - (y * TrigLUT.SinDeg(angleDegrees));
			return xr + pivotX;
		}
	
		public static float GetRotatedY(float currentX, float currentY, float pivotX, float pivotY, float angleDegrees) {
			float x = currentX - pivotX;
			float y = currentY - pivotY;
			float yr = (x * TrigLUT.SinDeg(angleDegrees)) + (y * TrigLUT.CosDeg(angleDegrees));
			return yr + pivotY;
		}
	
		public static float GetXAtEndOfRotatedLineByOrigin(float x, float lineLength, float angleDegrees) {
			return x + TrigLUT.CosDeg(angleDegrees) * lineLength;
		}
	
		public static float GetYAtEndOfRotatedLineByOrigin(float y, float lineLength, float angleDegrees) {
			return y + TrigLUT.SinDeg(angleDegrees) * lineLength;
		}
	
		public static bool Collides(float x1, float y1, float radius1, float x2, float y2, float radius2) {
			float d = Utils.Distance(x1, y1, x2, y2);
	
			d -= radius1 + radius2;
	
			return d < 0;
		}
	
		public static String ReadFileContents(String file) {
		  	string readText = File.ReadAllText(file,System.Text.Encoding.UTF8);
        	return(readText);	
		}
	}
}

