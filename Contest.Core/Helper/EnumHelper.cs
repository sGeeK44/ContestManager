using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Core.Helper
{
    public static class EnumHelper
    {
        public static List<T> GetValueList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }
}
