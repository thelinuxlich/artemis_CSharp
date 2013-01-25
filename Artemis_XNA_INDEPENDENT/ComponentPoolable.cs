using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis
{
    public interface ComponentPoolable : Component
    {
        void Initialize();
        void Cleanup();
    }
}
