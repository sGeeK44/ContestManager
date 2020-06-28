using System;

namespace Contest.Core.Exceptions
{
    public class AttributeNotFoundException : Exception
    {
        public AttributeNotFoundException(Type attrType)
            : base($"Attribute {attrType.Name} not found.") { }

        public AttributeNotFoundException(Type attrType, Type objType)
            : base($"Attribute {attrType.Name} not found in {objType.Name}.") { }
    }
}
