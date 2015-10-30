using System;

namespace Contest.Core.Exceptions
{
    public class AttributeSeveralFoundException : Exception
    {
        public AttributeSeveralFoundException(Type attrType)
            : base(string.Format("Attribute {0} found more than once.", attrType.Name)) { }

        public AttributeSeveralFoundException(Type attrType, Type objType)
            : base(string.Format("Attribute {0} found more than once in {1}", attrType.Name, objType.Name)) { }
    }
}
