using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class PropertyComponentPool : Attribute
    {
        public PropertyComponentPool(Func<Component> AllocateComponent)
        {
            InitialSize = 10;
            Resizes = false;
            this.AllocateComponent = AllocateComponent;
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

        public  Func<Component> AllocateComponent
        {
            get;
            private set;
        }

        /// <summary>
        /// Called when the object if found in the pool
        /// </summary>
        public Action<Component> Initialize { get; set; }

        /// <summary>
        /// Called when the object is cleaned and returned to the pool
        /// </summary>
        public Action<Component> Deinitialize { get; set; }

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
