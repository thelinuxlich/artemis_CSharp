namespace ArtemisTest
{

    using System;
    using NUnit.Framework;

    [TestFixture]
    public class ProxyTests
    {
        [Test]
        public void MultiTest()
        {
            Test.multi();
        }

        [Test]
        public void MultiSystemTest()
        {
            Test.multsystem();
        }

        [Test]
        public void QueueSystemTest()
        {
            Test.QueueSystemTeste();
        }

        [Test]
        public void SystemCommunicationTest()
        {
            Test.SystemComunicationTeste();
        }
    }
}
