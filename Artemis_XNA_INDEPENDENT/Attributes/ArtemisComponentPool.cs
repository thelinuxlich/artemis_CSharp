using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ArtemisComponentPool : Attribute
    {
        public ArtemisComponentPool()
        {
            InitialSize = 10;
            ResizeSize = 10;
            Resizes = true;
            isSupportMultiThread = false;
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
        /// The size of the pool resize        
        /// Default 10
        /// </summary>
        public int ResizeSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance should support multi thread environemnt
        /// Default false
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance should support multi thread environemnt; otherwise, <c>false</c>.
        /// </value>
        public bool isSupportMultiThread
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
