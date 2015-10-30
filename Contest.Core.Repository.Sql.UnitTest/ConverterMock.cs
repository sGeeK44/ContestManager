using System;
using Contest.Core.Converters;

namespace Contest.Core.Repository.Sql.UnitTest
{
    public class ConverterMock : IConverter
    {
        public object Convert(Type objectType, string value, object[] customAttr)
        {
            return null;
        }

        public string Convert(object obj, object[] customAttr)
        {
            return null;
        }
    }
}
