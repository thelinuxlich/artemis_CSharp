using System;
using System.IO;
namespace Artemis
{
	public class Utils
	{
		public static double CubicInterpolation(double v0, double v1, double v2, double v3, double t) {
			double t2 = t * t;
			double a0 = v3 - v2 - v0 + v1;
			double a1 = v0 - v1 - a0;
			double a2 = v2 - v0;
			double a3 = v1;
	
			return (a0 * (t * t2)) + (a1 * t2) + (a2 * t) + a3;
		}
	
		public static double QuadraticBezierInterpolation(double a, double b, double c, double t) {
			return (((1f - t) * (1f - t)) * a) + (((2f * t) * (1f - t)) * b) + ((t * t) * c);
		}
	
		public static double LengthOfQuadraticBezierCurve(double x0, double y0, double x1, double y1, double x2, double y2) {
			if ((x0 == x1 && y0 == y1) || (x1 == x2 && y1 == y2)) {
				return Distance(x0, y0, x2, y2);
			}
	
			double ax, ay, bx, by;
			ax = x0 - 2 * x1 + x2;
			ay = y0 - 2 * y1 + y2;
			bx = 2 * x1 - 2 * x0;
			by = 2 * y1 - 2 * y0;
			double A = 4 * (ax * ax + ay * ay);
			double B = 4 * (ax * bx + ay * by);
			double C = bx * bx + by * by;
	
			double Sabc = 2f * (double) Math.Sqrt(A + B + C);
			double A_2 = (double) Math.Sqrt(A);
			double A_32 = 2f * A * A_2;
			double C_2 = 2f * (double) Math.Sqrt(C);
			double BA = B / A_2;
	
			return (A_32 * Sabc + A_2 * B * (Sabc - C_2) + (4f * C * A - B * B) * (double) Math.Log((2 * A_2 + BA + Sabc) / (BA + C_2))) / (4 * A_32);
		}
	
		public static double Lerp(double a, double b, double t) {
			if (t < 0)
				return a;
			return a + t * (b - a);
		}
	
		public static double Distance(double x1, double y1, double x2, double y2) {
			return EuclideanDistance(x1, y1, x2, y2);
		}
	
		public static bool DoCirclesCollide(double x1, double y1, double radius1, double x2, double y2, double radius2) {
			double dx = x2 - x1;
			double dy = y2 - y1;
			double d = radius1 + radius2;
			return (dx * dx + dy * dy) < (d * d);
		}
	
		public static double EuclideanDistanceSq2D(double x1, double y1, double x2, double y2) {
			double dx = x1 - x2;
			double dy = y1 - y2;
			return dx * dx + dy * dy;
		}
	
		public static double ManhattanDistance(double x1, double y1, double x2, double y2) {
			return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
		}
	
		public static double EuclideanDistance(double x1, double y1, double x2, double y2) {
			double a = x1 - x2;
			double b = y1 - y2;
	
			return (double) FastMath.Sqrt(a * a + b * b);
		}
	
		public static double AngleInDegrees(double ownerRotation, double x1, double y1, double x2, double y2) {
			return Math.Abs(ownerRotation - AngleInDegrees(x1, y1, x2, y2)) % 360;
		}
	
		public static double AngleInDegrees(double originX, double originY, double targetX, double targetY) {
			return Math.Atan2(targetY - originY, targetX - originX) * (180.0 / Math.PI);
		}
	
		public static double AngleInRadians(double originX, double originY, double targetX, double targetY) {
			return (double) Math.Atan2(targetY - originY, targetX - originX);
		}
	
		public static bool ShouldRotateCounterClockwise(double angleFrom, double angleTo) {
			double diff = (angleFrom - angleTo) % 360;
			return diff > 0 ? diff < 180 : diff < -180;
		}
	
		public static double GetRotatedX(double currentX, double currentY, double pivotX, double pivotY, double angleDegrees) {
			double x = currentX - pivotX;
			double y = currentY - pivotY;
			double xr = (x * TrigLUT.CosDeg(angleDegrees)) - (y * TrigLUT.SinDeg(angleDegrees));
			return xr + pivotX;
		}
	
		public static double GetRotatedY(double currentX, double currentY, double pivotX, double pivotY, double angleDegrees) {
			double x = currentX - pivotX;
			double y = currentY - pivotY;
			double yr = (x * TrigLUT.SinDeg(angleDegrees)) + (y * TrigLUT.CosDeg(angleDegrees));
			return yr + pivotY;
		}
	
		public static double GetXAtEndOfRotatedLineByOrigin(double x, double lineLength, double angleDegrees) {
			return x + TrigLUT.CosDeg(angleDegrees) * lineLength;
		}
	
		public static double GetYAtEndOfRotatedLineByOrigin(double y, double lineLength, double angleDegrees) {
			return y + TrigLUT.SinDeg(angleDegrees) * lineLength;
		}
	
		public static bool Collides(double x1, double y1, double radius1, double x2, double y2, double radius2) {
			double d = Utils.Distance(x1, y1, x2, y2);
	
			d -= radius1 + radius2;
	
			return d < 0;
		}
	
		public static String ReadFileContents(String file) {
		  	string readText = File.ReadAllText(file,System.Text.Encoding.UTF8);
        	return(readText);	
		}
	}
}

