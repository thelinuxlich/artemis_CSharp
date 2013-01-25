using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class PropertyComponentCleanup: Attribute
    {
        public PropertyComponentCleanup()
        {            
        }        
    }
}
