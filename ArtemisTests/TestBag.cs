#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestBag.cs" company="GAMADU.COM">
//     Copyright © 2013 GAMADU.COM. All rights reserved.
//
//     Redistribution and use in source and binary forms, with or without modification, are
//     permitted provided that the following conditions are met:
//
//        1. Redistributions of source code must retain the above copyright notice, this list of
//           conditions and the following disclaimer.
//
//        2. Redistributions in binary form must reproduce the above copyright notice, this list
//           of conditions and the following disclaimer in the documentation and/or other materials
//           provided with the distribution.
//
//     THIS SOFTWARE IS PROVIDED BY GAMADU.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
//     WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//     FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
//     CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//     CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//     SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//     ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//     ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//     The views and conclusions contained in the software and documentation are those of the
//     authors and should not be interpreted as representing official policies, either expressed
//     or implied, of GAMADU.COM.
// </copyright>
// <summary>
//   This is a test class for TestBag and is intended to contain all TestBag Unit Tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace UnitTests
{
    #region Usind statemnets

    using global::System;
    using global::System.Diagnostics;
    using global::System.Globalization;
    using Artemis.Utils;
    
#if METRO    
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#elif MONO
    using NUnit.Framework;
#else
    using Microsoft.VisualStudio.TestTools.UnitTesting;    
#endif

    #endregion Usind statemnets

    /// <summary>This is a test class for TestBag and is intended to contain all TestBag Unit Tests.</summary>
#if MONO
    [TestFixture]
#else
    [TestClass]
#endif

    public class TestBag
    {
        /// <summary>The test Capacity.</summary>
        private const int Capacity = 10;

        /// <summary>The test element1.</summary>
        private const string TestElement1 = "Test element 1";

        /// <summary>The test element2.</summary>
        private const string TestElement2 = "Test element 2";

        /// <summary>The test element3.</summary>
        private const string TestElement3 = "Test element 3";

        /// <summary>Gets or sets the test context which provides information about and functionality for the current test run.</summary>
        /// <value>The test context.</value>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        /*
         * You can use the following additional attributes as you write your tests:
         * Use ClassInitialize to run code before running the first test in the class
         * [ClassInitialize()]
         * public static void MyClassInitialize(TestContext testContext)
         * {
         * }
         * 
         * Use ClassCleanup to run code after all tests in a class have run
         * [ClassCleanup()]
         * public static void MyClassCleanup()
         * {
         * }
         * 
         * Use TestInitialize to run code before running each test
         * [TestInitialize()]
         * public void MyTestInitialize()
         * {
         * }
         * 
         * Use TestCleanup to run code after each test has run
         * [TestCleanup()]
         * public void MyTestCleanup()
         * {
         * }
         */
        #endregion

        /// <summary>Tests the bag constructor.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestBagConstructor()
        {
            Bag<string> target = new Bag<string>(Capacity);
            Assert.IsNotNull(target);
        }

        /// <summary>Tests the add.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestAdd()
        {
            // ReSharper disable UseObjectOrCollectionInitializer
            Bag<string> target = new Bag<string>(Capacity);
            // ReSharper restore UseObjectOrCollectionInitializer
            target.Add(TestElement1);
            target.Add(TestElement2);
            target.Add(TestElement3);

            Assert.IsTrue(target.Contains(TestElement1) && target.Contains(TestElement2) && target.Contains(TestElement3));
        }

        /// <summary>Tests the add range.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestAddRange()
        {
            Bag<string> target = new Bag<string>(Capacity);
            Bag<string> rangeOfElements = new Bag<string>(Capacity) { TestElement1, TestElement2, TestElement3 };
            target.AddRange(rangeOfElements);
            Assert.IsTrue(target.Contains(TestElement1) && target.Contains(TestElement2) && target.Contains(TestElement3));
        }

        /// <summary>Tests the clear.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestClear()
        {
            Bag<string> target = new Bag<string>(Capacity) { TestElement1, TestElement2, TestElement3 };
            target.Clear();
            Assert.AreEqual(0, target.Count);
        }

        /// <summary>Tests the contains.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestContains()
        {
            Bag<string> target = new Bag<string>(Capacity) { TestElement1, TestElement2, TestElement3 };
            const string Element = TestElement2;
            const bool Expected = true;
            bool actual = target.Contains(Element);
            Assert.AreEqual(Expected, actual);
        }

        /// <summary>Tests the get.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestGet()
        {
            Bag<string> target = new Bag<string>(Capacity) { TestElement1, TestElement2, TestElement3 };
            const int Index = 1;
            const string Expected = TestElement2;
            string actual = target.Get(Index);
            Assert.AreEqual(Expected, actual);
        }

        /// <summary>Tests the grow.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
#if !METRO && !MONO
        [DeploymentItem("artemis.dll")]
#endif
        public void TestGrow()
        {
            Bag<string> target = new Bag<string>(0);
            int beforeGrow = target.Capacity;
            target.AddRange(new Bag<string> { TestElement1, TestElement2, TestElement3 });
            const int Expected = 4;
            int actual = target.Capacity;
            Assert.IsTrue(beforeGrow < actual);
            Assert.AreEqual(Expected, actual);
        }

        /// <summary>Tests the remove.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestRemove()
        {
            Bag<string> target = new Bag<string>(Capacity) { TestElement1, TestElement2, TestElement3 };
            const int Index = 1;
            const string Expected = TestElement2;
            string actual = target.Remove(Index);
            Assert.AreEqual(Expected, actual);
        }

        /// <summary>Tests the remove all.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestRemoveAll()
        {
            Bag<string> target = new Bag<string>(Capacity) { TestElement1, TestElement2, TestElement3 };
            Bag<string> bag = new Bag<string>(Capacity) { TestElement2, TestElement3 };
            const bool Expected = true;
            bool actual = target.RemoveAll(bag);
            Assert.AreEqual(Expected, actual);
        }

        /// <summary>Tests the remove last.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestRemoveLast()
        {
            Bag<string> target = new Bag<string>(Capacity) { TestElement1, TestElement2, TestElement3 };
            const string Expected = TestElement3;
            string actual = target.RemoveLast();
            Assert.AreEqual(Expected, actual);
        }

        /// <summary>Tests the set.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestSet()
        {
            Bag<string> target = new Bag<string>(Capacity) { TestElement1, TestElement2, TestElement3 };
            const int Index = Capacity - 1;
            const string Element = "TestSetElement";
            target.Set(Index, Element);
            const string Expected = Element;
            string actual = target.RemoveLast();
            Assert.AreEqual(Expected, actual);
        }

        /// <summary>Tests the capacity.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestCapacity()
        {
            Bag<string> target = new Bag<string>(Capacity);
            int actual = target.Capacity;
            Assert.AreEqual(actual, Capacity);
        }

        /// <summary>Tests the is empty.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestIsEmpty()
        {
            Bag<string> target = new Bag<string>(Capacity);
            Assert.IsTrue(target.IsEmpty);
        }

        /// <summary>Tests the item.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestItem()
        {
            Bag<string> target = new Bag<string>(Capacity) { TestElement1, TestElement2, TestElement3 };
            const int Index = 1;
            const string Expected = TestElement2;
            string actual = target[Index];
            Assert.AreEqual(Expected, actual);
        }

        /// <summary>Tests the performance.</summary>
#if MONO
    [Test]
#else
    [TestMethod]
#endif
        public void TestPerformance()
        {
            Debug.WriteLine("Number of elements: ");

            // Identify max mem size.
            Bag<int> bigBag = new Bag<int>();

#if METRO
        int maxMem = 50;
#else
            int maxMem = 5000;
#endif
            
            
            // pointless to use int.maxvalue (sometimes it works, some it does not ... depends on other process)
            for (int index = 0; index < maxMem; ++index)
            {
                try
                {
                    bigBag.Add(index);
                }
                catch (Exception)
                {
                    // some extra to be sure (there are some memory allocs we cant control in other threads)
                    maxMem = index;
                    break;
                }
            }
            
            bigBag = null;
#if !MONO
            // This is need to secure that enough memory is left.
            GC.Collect();
            GC.WaitForPendingFinalizers();
#if !METRO
            GC.WaitForFullGCComplete();
#endif
            GC.Collect();
#endif

            Debug.WriteLine(maxMem.ToString(CultureInfo.InvariantCulture));

            // Reset bag.
            bigBag = new Bag<int>(0);

            // Start measurement.
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Fill
            for (int index = maxMem; index >= 0; --index)
            {
                bigBag.Add(index);
            }

            stopwatch.Stop();
            Debug.WriteLine("Load  duration: {0}", FastDateTime.ToString(stopwatch.Elapsed));

            stopwatch.Restart();
            bigBag.Clear();
            stopwatch.Stop();
            Debug.WriteLine("Clear duration: {0}", FastDateTime.ToString(stopwatch.Elapsed));
        }

/*
        /// <summary>Tests the limit.</summary>
        [TestMethod]
        public void TestLimit()
        {
            Debug.WriteLine("Number of elements: ");

            // Identify max mem size.
            Bag<int> bigBag = new Bag<int>();
            int maxMem = int.MaxValue;

            // pointless to use int.maxvalue (sometimes it works, some it does not ... depends on other process)
            for (int index = 0; index < maxMem; ++index)
            {
                try
                {
                    bigBag.Add(index);
                }
                catch (Exception)
                {
                    // some extra to be sure (there are some memory allocs we cant control in other threads)
                    maxMem = index - 1;
                    break;
                }
            }

            bigBag = null;

            // This is need to secure that enough memory is left.
            GC.Collect();
            GC.WaitForPendingFinalizers();
#if !METRO
            GC.WaitForFullGCComplete();
#endif
            GC.Collect();

            Debug.WriteLine(maxMem.ToString(CultureInfo.InvariantCulture));

            // Reset bag.
            bigBag = new Bag<int>(0);

            // Start measurement.
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Fill
            for (int index = maxMem; index >= 0; --index)
            {
                bigBag.Add(index);
            }

            stopwatch.Stop();
            Debug.WriteLine("Load  duration: {0}", FastDateTime.ToString(stopwatch.Elapsed));

            stopwatch.Restart();
            bigBag.Clear();
            stopwatch.Stop();
            Debug.WriteLine("Clear duration: {0}", FastDateTime.ToString(stopwatch.Elapsed));
        }
*/
    }
}
