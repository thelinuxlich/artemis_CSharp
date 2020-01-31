#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestNormalEntityProcessingSystem3.cs" company="GAMADU.COM">
// Copyright © 2013 GAMADU.COM. All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are
// permitted provided that the following conditions are met:
//
// 1. Redistributions of source code must retain the above copyright notice, this list of
// conditions and the following disclaimer.
//
// 2. Redistributions in binary form must reproduce the above copyright notice, this list
// of conditions and the following disclaimer in the documentation and/or other materials
// provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY GAMADU.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
// ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
// ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// The views and conclusions contained in the software and documentation are those of the
// authors and should not be interpreted as representing official policies, either expressed
// or implied, of GAMADU.COM.
// </copyright>
// <summary>
// The third most simple system ever.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace UnitTests.System
{
    #region Using statements

    using Artemis;
    using Artemis.Attributes;
    using Artemis.Manager;
    using Artemis.System;

    using UnitTests.Component;

    #endregion Using statements

    /// <summary>The third most simple system ever.</summary>
#if !PORTABLE
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 0, ExecutionType = ExecutionType.Asynchronous)]
#else
    [ArtemisEntitySystem(GameLoopType = GameLoopType.Update, Layer = 0)]
#endif
    public class TestNormalEntityProcessingSystem3 : EntityProcessingSystem
    {
        /// <summary>Initializes a new instance of the <see cref="TestNormalEntityProcessingSystem3" /> class.</summary>
        public TestNormalEntityProcessingSystem3()
            : base(Aspect.One(typeof(TestPowerComponentPoolable), typeof(TestHealthComponent)))
        {
        }

        /// <summary>The process.</summary>
        /// <param name="entity">The entity.</param>
        public override void Process(Entity entity)
        {
            TestHealthComponent testHealth = entity.GetComponent<TestHealthComponent>();
            if (testHealth != null)
            {
                testHealth.AddDamage(10);
            }

            TestPowerComponentPoolable testPower = entity.GetComponent<TestPowerComponentPoolable>();
            if (testPower != null)
            {
                testPower.Power -= 10;
            }
        }
    }
}