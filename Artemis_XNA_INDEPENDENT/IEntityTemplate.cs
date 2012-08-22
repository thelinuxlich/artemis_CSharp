using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artemis
{
    public interface IEntityTemplate
    {
        Entity BuildEntity(Entity e, params object[] args);
    }
}
