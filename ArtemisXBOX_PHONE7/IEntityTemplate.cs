using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artemis;

namespace ArtemisXbox
{
    public interface IEntityTemplate
    {
        Entity BuildEntity(Entity e);
    }
}
