using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class PropertyEntityTemplate : Attribute
    {
        readonly string name;
     
        public PropertyEntityTemplate(string name)
        {
            this.name = name;

        }

        public string Name
        {
            get { return name; }
        }
    }
}
