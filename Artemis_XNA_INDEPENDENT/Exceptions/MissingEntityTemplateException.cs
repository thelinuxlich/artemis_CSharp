using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Exceptions
{
    /// <summary>
    /// Exception that is thrown when trying to use a EntityTemplate which does not exist.
    /// </summary>
#if !XBOX && !WINDOWS_PHONE && !PORTABLE
    [global::System.Serializable]
#endif
    public class MissingEntityTemplateException : global::System.Exception
    {
        internal MissingEntityTemplateException(string entityTemplateTag)
            : this(entityTemplateTag, null) { }
        internal MissingEntityTemplateException(string entityTemplateTag, global::System.Exception inner)
            : base("EntityTemplate for the tag " + entityTemplateTag + " was not registered.", inner) { }
#if !XBOX && !WINDOWS_PHONE && !PORTABLE
        /// <summary>
        /// Initializes the exception with serialized data.
        /// </summary>
        /// <param name="info"> The serialization info. </param>
        /// <param name="context"> The serialization context. </param>
        protected MissingEntityTemplateException(
          global::System.Runtime.Serialization.SerializationInfo info,
          global::System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
#endif
    }
}
