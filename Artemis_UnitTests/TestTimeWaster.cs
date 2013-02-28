#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestTimeWaster.cs" company="GAMADU.COM">
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
// <author>Jens-Axel Grünewald</author>
// <date>2/23/2013 10:05:38 AM</date>
// <summary>
//     This is a time waster class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace UnitTests
{
    #region Using statements

    using global::System;

    #endregion Using statements

    /// <summary>This is a class.</summary>
    public class TestTimeWaster
    {
        /// <summary>Initializes static members of the <see cref="TestTimeWaster"/> class.</summary>
        static TestTimeWaster()
        {
            Result = 0.0d;
        }

        /// <summary>Gets the result.</summary>
        /// <value>The result.</value>
        public static double Result { get; private set; }

        /// <summary>Delays the specified iterations.</summary>
        /// <param name="iterations">The iterations.</param>
        public static void Delay(int iterations = 10)
        {
            double x = 0.1d;
            for (double index = iterations - 1; index >= 0; --index)
            {
                x *= Math.Log(index);
                x *= Math.Cos(index);
            }

            Result = x;
        }
    }
}