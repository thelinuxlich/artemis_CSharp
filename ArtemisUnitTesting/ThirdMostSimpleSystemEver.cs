#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThirdMostSimpleSystemEver.cs" company="GAMADU.COM">
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
//   The third most simple system ever.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace ArtemisUnitTesting
{
    #region Using statements

    using Artemis;
    using Artemis.Attributes;
    using Artemis.Manager;
    using Artemis.System;

    #endregion Using statements

    /// <summary>The third most simple system ever.</summary>
    [ArtemisEntitySystem(ExecutionType = ExecutionType.UpdateSynchronous, Layer = 0)]
    public class ThirdMostSimpleSystemEver : EntityProcessingSystem
    {
        /// <summary>Initializes a new instance of the <see cref="ThirdMostSimpleSystemEver" /> class.</summary>
        public ThirdMostSimpleSystemEver()
            : base(Aspect.One(typeof(Power2Component), typeof(HealthComponent)))
        {
        }

        /// <summary>The process.</summary>
        /// <param name="entity">The entity.</param>
        public override void Process(Entity entity)
        {
            if (entity.GetComponent<HealthComponent>() != null)
            {
                entity.GetComponent<HealthComponent>().AddDamage(10);
            }

            if (entity.GetComponent<Power2Component>() != null)
            {
                entity.GetComponent<Power2Component>().Power -= 10;
            }
        }
    }
}