#region File description

//-----------------------------------------------------------------------
// <copyright file="FastDateTime.cs" company="JAG SoftwareDesign">
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
// <email>info@jag-softwaredesign.de</email>
// <date>7/12/2012 4:06:21 AM</date>
// <summary>
// <para>The fast date time.</para>
// <para>The standard DateTime.Now call produces more garbage</para>
// <para>and is slower. For Windows NT 3.5 and later the timer</para>
// <para>resolution is approximately 10ms.</para>
// </summary>
//-----------------------------------------------------------------------
#endregion File description

namespace Artemis.Utils
{
    #region Using statements

    using global::System;

    #endregion Using statements

    /// <summary>
    /// <para>The fast date time.</para>
    /// <para>The standard DateTime.Now call produces more garbage</para>
    /// <para>and is slower. For Windows NT 3.5 and later the timer</para>
    /// <para>resolution is approximately 10 milliseconds.</para>
    /// </summary>
    public static class FastDateTime
    {
        /// <summary>The local UTC offset.</summary>
#if !PORTABLE && !UNITY5        
        private static readonly TimeSpan LocalUtcOffset = TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now);
#else
        private static readonly TimeSpan LocalUtcOffset = DateTime.UtcNow - DateTime.Now;        
#endif

        /// <summary>Gets the now.</summary>
        public static DateTime Now
        {
            get { return DateTime.UtcNow + LocalUtcOffset; }
        }

        /// <summary>Gets the micro seconds from ticks.</summary>
        /// <param name="ticks">The ticks.</param>
        /// <returns>The <c>µs</c> as System.Double.</returns>
        public static double GetMicroSeconds(long ticks)
        {
            return ticks * 0.1;
        }

        /// <summary>Gets the nano seconds.</summary>
        /// <param name="ticks">The ticks.</param>
        /// <returns>The <c>ns</c> as System.Double.</returns>
        public static double GetNanoSeconds(long ticks)
        {
            return ticks * 100.0;
        }

        /// <summary>Returns a <see cref="string" /> that formats a time span.</summary>
        /// <param name="timeSpan">The time span.</param>
        /// <returns>A <see cref="string" /> that formats a time span.</returns>
        public static string ToString(TimeSpan timeSpan)
        {
            return string.Format("{0:00}:{1:00}:{2:00}.{3:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds * 0.1);
        }

        /// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public static new string ToString()
        {
            return Now.ToString("HH:mm:ss.ffffff");
        }
    }
}