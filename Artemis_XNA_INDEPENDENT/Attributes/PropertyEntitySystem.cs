using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class PropertyEntitySystem : Attribute
    {
        public PropertyEntitySystem()
        {
            this.ExecutionType = Artemis.ExecutionType.Update;
            this.Layer = 0;
        }

        public ExecutionType ExecutionType
        {
            get;
            set;
        }

        public int Layer
        {
            get;
            set;
        }

    }
}
