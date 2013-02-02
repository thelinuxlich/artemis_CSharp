namespace Artemis.Attributes
{
    #region Using statements

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Reflection;

    #endregion Using statements

    /// <summary>Class AttributesProcessor.</summary>
    public class AttributesProcessor
    {
        /// <summary>The supported attributes.</summary>
        public static readonly List<Type> SupportedAttributes = new List<Type>
                                                                    {
                                                                        typeof(ArtemisEntitySystem), 
                                                                        typeof(ArtemisEntityTemplate),
                                                                        typeof(ArtemisComponentPool),
                                                                        typeof(ArtemisComponentCreate)
                                                                    };

        /// <summary>Processes the specified supported attributes.</summary>
        /// <param name="supportedAttributes">The supported attributes.</param>
        /// <returns>A Dictionary{TypeList{Attribute}} belonging to the specified attributes.</returns>
        public static IDictionary<Type, List<Attribute>> Process(List<Type> supportedAttributes)
        {
            IDictionary<Type, List<Attribute>> attTypes = new Dictionary<Type, List<Attribute>>();

#if FULLDOTNET
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly item in loadedAssemblies)
            {
                Type[] types = item.GetTypes();
                foreach (Type type in types)
                {
                    object[] attributes = type.GetCustomAttributes(false);
                    foreach (object att in attributes.Where(att => supportedAttributes.Contains(att.GetType())))
                    {
                        if (!attTypes.ContainsKey(type))
                        {
                            attTypes[type] = new List<Attribute>();
                        }

                        attTypes[type].Add((Attribute)att);
                    }
                }
            }

            return attTypes;
#else
            
            var loadedAssemblies = new List<Assembly>()
            {
            System.Reflection.Assembly.GetCallingAssembly(),
            System.Reflection.Assembly.GetExecutingAssembly(),
            };
            foreach (var item in loadedAssemblies)            
            {
                var types = item.GetTypes();
                foreach (var type in types)
                {
                    var attributes = type.GetCustomAttributes(false);
                    foreach (var att in attributes)
                    {
                        if (supportedAttributes.Contains(att.GetType()))
                        {
                            if (!attTypes.ContainsKey(type))
                            {
                                attTypes[type] = new List<Attribute>();
                            }
                            attTypes[type].Add((Attribute)att);
                        }
                    }
                }
            }

            return attTypes;

#endif
        }
    }
}