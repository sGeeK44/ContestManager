using System;
using Contest.Core.Converters;
using Contest.Core.Converters.EnumConverter;
using NUnit.Framework;

namespace Contest.Core.UnitTest.Converter
{
    [TestFixture]
    public class EnumConverter
    {
        public enum TestEnum
        {
            [StringValue("Fake")]
            Fake = 0,
            [StringValue("StringValue")]
            Value = 1
        }

        [TestCase]
        public void ConvertEnumToStringByFieldName()
        {
            ConvertEnumToString(new StringEnumByFieldName(), TestEnum.Value, "Value");
        }

        [TestCase]
        public void ConvertStringToEnumByFieldName()
        {
            ConvertStringToEnum(new StringEnumByFieldName(), "Value", TestEnum.Value);
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToEnumByFieldNameOther()
        {
            ConvertStringToEnum(new StringEnumByFieldName(), "value", TestEnum.Value);
        }

        [TestCase]
        public void ConvertEnumToStringByInt()
        {
            ConvertEnumToString(new StringEnumByInt(), TestEnum.Value, "1");
        }

        [TestCase]
        public void ConvertStringToEnumByInt()
        {
            ConvertStringToEnum(new StringEnumByInt(), "1", TestEnum.Value);
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToEnumByIntOther()
        {
            (new StringEnumByInt()).ToEnum("2", typeof(TestEnum));
        }

        [TestCase]
        public void ConvertEnumToStringByStringValue()
        {
            ConvertEnumToString(new StringEnumByStringValue(), TestEnum.Value, "StringValue");
        }

        [TestCase]
        public void ConvertStringToEnumByStringValue()
        {
            ConvertStringToEnum(new StringEnumByStringValue(), "StringValue", TestEnum.Value);
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToEnumByStringValueOther()
        {
            ConvertStringToEnum(new StringEnumByStringValue(), "StringValu", TestEnum.Value);
        }

        public void ConvertEnumToString(IEnumConverter conv, Enum input, string output)
        {
            var result = conv.ToString(input);
            Assert.AreEqual(output, result);
        }

        public void ConvertStringToEnum(IEnumConverter conv, string input, Enum output)
        {
            var result = conv.ToEnum(input, typeof(TestEnum));
            Assert.AreEqual(output, result);
        }
    }
}
