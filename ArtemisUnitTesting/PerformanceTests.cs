#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformanceTests.cs" company="GAMADU.COM">
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
//   The performance tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace ArtemisUnitTesting
{
    #region Using statements

    using System;
    using System.Diagnostics;

    using Artemis.Utils;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion Using statements

    /// <summary>The performance tests.</summary>
    [TestClass]
    public class PerformanceTests
    {
        /// <summary>The bag performance test a.</summary>
        [TestMethod]
        public void BagPerformanceTestA()
        {
            Bag<int> bigBag = new Bag<int>(16);
            Stopwatch watch = Stopwatch.StartNew();
            int x = 50;
            for (int index = 0; index <= 1000000; ++index)
            {
                // Set it to a billion to get a OutOfMemoryException
                // due to Bag Grow method generating too many items.
                bigBag.Add(index + (x / 2) + 1);
                ++x;
            }

            watch.Stop();

            // It is fast.
            Console.WriteLine(watch.Elapsed);
        }
    }
}