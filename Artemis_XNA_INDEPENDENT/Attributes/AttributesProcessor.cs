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
        typeof(PropertyEntitySystem),typeof(PropertyEntityTemplate)
        }
        ;

        public static IDictionary<Type, Attribute> Process(List<Type> supportedAttributes)
        {
            IDictionary<Type, Attribute> attTypes = new Dictionary<Type, Attribute>();

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var item in loadedAssemblies)
            {
                var types = item.GetTypes();
                foreach (var type in types)
                {
                    var attributes = type.GetCustomAttributes(false);
                    foreach (var att in attributes)
                    {
                        if (supportedAttributes.Contains(attributes.GetType()))
                        {
                            if (attTypes[type] != null)
                            {
                                throw new Exception("Type " + type.ToString() + "Cannot have more than one attribute ");
                            }
                            attTypes[type] = (Attribute)att;
                        }
                    }
                }
            }
            
                return attTypes;
        }
    }
}
