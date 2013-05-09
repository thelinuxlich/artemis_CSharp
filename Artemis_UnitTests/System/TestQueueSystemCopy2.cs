#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestQueueSystemCopy2.cs" company="GAMADU.COM">
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
//   The queue system test 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace UnitTests.System
{
    #region Using statements

    using Artemis.System;

    using UnitTests.Component;
    using UnitTests.Extra;

    #endregion Using statements

    /// <summary>The queue system test 3.</summary>
    public class TestQueueSystemCopy2 : QueueSystemProcessingThreadSafe<DummyPlaceHolder>
    {
        /// <summary>The damage.</summary>
        private readonly int damage;

        /// <summary>Initializes a new instance of the <see cref="TestQueueSystemCopy2" /> class.</summary>
        /// <param name="damage">The damage.</param>
        public TestQueueSystemCopy2(int damage)
        {
            this.damage = damage;
        }

        /// <summary>The process.</summary>
        /// <param name="entity">The entity.</param>
        public override void Process(DummyPlaceHolder entity)
        {
            TestHealthComponent testHealthComponent = entity.Component as TestHealthComponent;
            if (testHealthComponent != null)
            {
                testHealthComponent.AddDamage(this.damage);
            }
        }
    }
}