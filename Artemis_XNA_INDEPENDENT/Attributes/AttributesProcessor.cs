using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Artemis.Attributes
{
    public class AttributesProcessor
    {
        public static readonly List<Type> SupportedAttributes = new List<Type>()
        {
        typeof(PropertyEntitySystem),typeof(PropertyEntityTemplate),typeof(PropertyComponentPool), typeof(PropertyComponentCleanup), typeof(PropertyComponentCreate)
        }
        ;

        public static IDictionary<Type, List<Attribute>> Process(List<Type> supportedAttributes)
        {
            IDictionary<Type, List<Attribute>> attTypes = new Dictionary<Type, List<Attribute>>();

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var item in loadedAssemblies)
            //var item = loadedAssemblies[15];
            {
                var types = item.GetTypes();
                foreach (var type in types)
                {
                    var attributes = type.GetCustomAttributes(false);
                    foreach (var att in attributes)
                    {
                        if (supportedAttributes.Contains(att.GetType()))
                        {
                            if (!attTypes.ContainsKey(type) )
                            {
                                attTypes[type] = new List<Attribute>();
                            }
                            attTypes[type].Add((Attribute)att);
                        }
                    }
                }
            }
            
                return attTypes;
        }
    }
}
