using System;

namespace Contest.Core.Exceptions
{
    public class AttributeNotFoundException : Exception
    {
        public AttributeNotFoundException(Type attrType)
            : base(string.Format("Attribute {0} not found.", attrType.Name)) { }

        public AttributeNotFoundException(Type attrType, Type objType)
            : base(string.Format("Attribute {0} not found in {1}.", attrType.Name, objType.Name)) { }
    }
}
