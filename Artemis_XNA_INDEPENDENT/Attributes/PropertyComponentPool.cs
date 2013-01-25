using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class PropertyComponentPool : Attribute
    {
        public PropertyComponentPool()
        {
            InitialSize = 10;
            Resizes = false;            
        }

        /// <summary>
        /// Initial size of the Pool
        /// Default 10
        /// </summary>
        public int InitialSize
        {
            get;
            set;
        }

        /// <summary>
        /// If the pool can be resized
        /// Default False
        /// </summary>
        public bool Resizes
        {
            get;
            set;
        }


    }
}
