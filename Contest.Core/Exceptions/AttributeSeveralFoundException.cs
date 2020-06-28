using System;

namespace Contest.Core.Exceptions
{
    public class AttributeSeveralFoundException : Exception
    {
        public AttributeSeveralFoundException(Type attrType)
            : base($"Attribute {attrType.Name} found more than once.") { }

        public AttributeSeveralFoundException(Type attrType, Type objType)
            : base($"Attribute {attrType.Name} found more than once in {objType.Name}") { }
    }
}
