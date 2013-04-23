using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Exceptions
{
    /// <summary>
    /// Exception that is thrown when trying to use a EntityTemplate which does not exist.
    /// </summary>
    [global::System.Serializable]
    public class MissingEntityTemplateException : global::System.Exception
    {
        internal MissingEntityTemplateException(string entityTemplateTag)
            : this(entityTemplateTag, null) { }
        internal MissingEntityTemplateException(string entityTemplateTag, global::System.Exception inner)
            : base("EntityTemplate for the tag " + entityTemplateTag + " was not registered.", inner) { }
        protected MissingEntityTemplateException(
          global::System.Runtime.Serialization.SerializationInfo info,
          global::System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
