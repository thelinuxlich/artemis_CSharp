#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="GAMADU.COM">
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
//   The main program entry class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace UnitTests
{
    #region Using statements

    using global::System;
    using global::System.Diagnostics;

    #endregion

    /// <summary>The class Program.</summary>
    public class Program
    {
        /// <summary>Defines the entry point of the application.</summary>
        public static void Main()
        {
            Debug.WriteLine("General test begin...");
            TestGeneral testGeneral = new TestGeneral();
            testGeneral.TestIntervalEntitySystem();
            testGeneral.TestAttributes();
            testGeneral.TestDummies();
            testGeneral.TestMultipleSystems();
            testGeneral.TestQueueSystems();
#if !PORTABLE
            testGeneral.TestRenderMultiHealthBarSystem();
#endif
            testGeneral.TestSimpleSystem();
            testGeneral.TestSystemCommunication();
            testGeneral.TestEntityComponentSystem();
            testGeneral.TestDerivedComponents();
            testGeneral.TestInitializeComponentTypes();
            testGeneral.TestInitializeComponentTypesFromAssemblies();
            Debug.WriteLine("General test end.");

#if !METRO && !PORTABLE
            Debug.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
#endif

            Debug.WriteLine("Bag test begin...");
            TestBag testBag = new TestBag();
            testBag.TestPerformance();
            testBag.TestAdd();
            testBag.TestAddRange();
            testBag.TestBagConstructor();
            testBag.TestCapacity();
            testBag.TestClear();
            testBag.TestContains();
            testBag.TestGet();
            testBag.TestGrow();
            testBag.TestIsEmpty();
            testBag.TestItem();
            testBag.TestRemove();
            testBag.TestRemoveAll();
            testBag.TestRemoveLast();
            testBag.TestSet();
            Debug.WriteLine("Bag test end.");

#if !METRO && !PORTABLE
            Debug.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
#endif

            Debug.WriteLine("Aspect test begin...");
            TestAspect testAspect = new TestAspect();
            testAspect.TestAspectEmpty();
            testAspect.TestAspectAllSingle();
            testAspect.TestAspectAllMultiple();
            testAspect.TestAspectOneSingle();
            testAspect.TestAspectOneMultiple();
            testAspect.TestAspectExcludeSingle();
            testAspect.TestAspectExcludeMultiple();
            testAspect.TestAspectAllOneExclude();
            Debug.WriteLine("Aspect test end.");

#if !METRO && !PORTABLE
            Debug.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
#endif
        }
    }
}
