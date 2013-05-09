#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingEntityTemplateException.cs" company="GAMADU.COM">
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
//   Represents a Component Type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion File description

namespace Artemis.Exceptions
{
    #region Using statements

    using global::System;

#if !XBOX && !WINDOWS_PHONE && !PORTABLE && !METRO
    using global::System.Runtime.Serialization;
#endif

    #endregion

    /// <summary>Exception that is thrown when trying to use a EntityTemplate which does not exist.</summary>
#if !XBOX && !WINDOWS_PHONE && !PORTABLE && !METRO
    [Serializable]
#endif
    public class MissingEntityTemplateException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="MissingEntityTemplateException" /> class.</summary>
        /// <param name="entityTemplateTag">The entity template tag.</param>
        internal MissingEntityTemplateException(string entityTemplateTag)
            : this(entityTemplateTag, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="MissingEntityTemplateException" /> class.</summary>
        /// <param name="entityTemplateTag">The entity template tag.</param>
        /// <param name="inner">The inner.</param>
        internal MissingEntityTemplateException(string entityTemplateTag, Exception inner)
            : base("EntityTemplate for the tag " + entityTemplateTag + " was not registered.", inner)
        {
        }

#if !XBOX && !WINDOWS_PHONE && !PORTABLE && !METRO
        /// <summary>Initializes a new instance of the <see cref="MissingEntityTemplateException" /> class.</summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The serialization context.</param>
        protected MissingEntityTemplateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}