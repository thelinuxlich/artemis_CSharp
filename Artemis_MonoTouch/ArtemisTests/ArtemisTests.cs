
using System;
using NUnit.Framework;

namespace ArtemisTest
{
	[TestFixture]
	public class ProxyTests
	{
		[Test]
		public void MultiTest ()
		{
			Test.multi ();
		}
		
		[Test]
		public void MultiSystemTest ()
		{
            Test.multsystem();
		}
		
		[Test]
		public void QueueSystemTest ()
		{
            Test.QueueSystemTeste();
		}
		
		[Test]
		public void HybridQueueSystemTest ()
		{
            Test.HybridQueueSystemTeste();
		}
		
		[Test]
		public void SystemCommunicationTest ()
		{
            Test.SystemComunicationTeste();
		}
	}
}
