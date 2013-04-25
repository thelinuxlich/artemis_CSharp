using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Artemis.Utils
{
    internal static class MetroCompatibilityExtensions
    {
#if !METRO
        /// <summary>
        /// Extension which only returns the type itself if not using metro.
        /// This lets one call GetTypeInfo on Type objects even when metro is not being used.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Type GetTypeInfo(this Type self)
        {
            return self;
        }

        public static Delegate CreateDelegate(this MethodInfo self, Type type)
        {
            return Delegate.CreateDelegate(type, self);
        }
#endif
    }
}
