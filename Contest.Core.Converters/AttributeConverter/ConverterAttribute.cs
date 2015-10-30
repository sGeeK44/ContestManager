using System;
using System.Collections.Generic;

namespace Contest.Core.Converters.AttributeConverter
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public abstract class ConverterAttribute : Attribute
    {
        protected static readonly Dictionary<Type, object> Instances = new Dictionary<Type, object>();

        protected ConverterAttribute(Type typeOfConverter)
        {
            if (typeOfConverter == null) throw new ArgumentNullException("typeOfConverter");
            if (!Instances.ContainsKey(typeOfConverter))
            {
                Instances.Add(typeOfConverter, Activator.CreateInstance(typeOfConverter));
            }
        }
    }
}
