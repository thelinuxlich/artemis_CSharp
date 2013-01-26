using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis
{
    public abstract class ComponentPoolable : Component
    {
        internal int poolId = 0;
        public virtual void Initialize() { }
        public virtual void Cleanup() { }
    }
}
