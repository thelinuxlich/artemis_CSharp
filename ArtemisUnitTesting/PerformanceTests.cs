using System;
using Artemis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtemisUnitTesting
{
	[TestClass]
	public class PerformanceTests
	{
		[TestMethod]
		public void BagPerformanceTestA()
		{
			Bag<Int32> bigBag = new Bag<Int32>(16);
			var watch = System.Diagnostics.Stopwatch.StartNew();
			int x = 50;
			for (int i = 0; i <= 1000000; i++) 
				// set it to a billion to get a OutOfMemoryException due to Bag Grow method generating too many items
			{
				bigBag.Add(i+x / 2 + 1);
				x++;
			}
			watch.Stop();
			System.Console.WriteLine(watch.Elapsed); // It is fast.
		}
	}
}
