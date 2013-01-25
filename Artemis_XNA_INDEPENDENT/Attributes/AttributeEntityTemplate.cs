using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AttributeEntityTemplate : Attribute
    {
        readonly string name;
     
        public AttributeEntityTemplate(string name)
        {
            this.name = name;

        }

        public string Name
        {
            get { return name; }
        }
    }
}
