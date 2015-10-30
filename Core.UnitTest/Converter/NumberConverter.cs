using Contest.Core.Converters;
using Contest.Core.Converters.NumberConverter;
using NUnit.Framework;

namespace Contest.Core.UnitTest.Converter
{
    [TestFixture]
    public class NumberConverter
    {
        [TestCase]
        public void ConvertUShortToStringNoSpecific()
        {
            ConvertUshortToString(new NumberNoSpecific(), 1, "1");
        }

        [TestCase]
        public void ConvertStringToUShortNoSpecific()
        {
            ConvertStringToUShort(new NumberNoSpecific(), "1", 1);
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToUShortOther()
        {
            ConvertStringToUShort(new NumberNoSpecific(), "dd", 1);
        }

        public void ConvertUshortToString(INumberConverter conv, ushort input, string output)
        {
            var result = conv.ToString(input);
            Assert.AreEqual(output, result);
        }

        public void ConvertStringToUShort(INumberConverter conv, string input, ushort output)
        {
            var result = conv.ToUShort(input);
            Assert.AreEqual(output, result);
        }

        [TestCase]
        public void ConvertShortToStringNoSpecific()
        {
            ConvertShortToString(new NumberNoSpecific(), 1, "1");
        }

        [TestCase]
        public void ConvertStringToShortNoSpecific()
        {
            ConvertStringToShort(new NumberNoSpecific(), "1", 1);
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToShortOther()
        {
            ConvertStringToShort(new NumberNoSpecific(), "dd", 1);
        }

        public void ConvertShortToString(INumberConverter conv, short input, string output)
        {
            var result = conv.ToString(input);
            Assert.AreEqual(output, result);
        }

        public void ConvertStringToShort(INumberConverter conv, string input, short output)
        {
            var result = conv.ToShort(input);
            Assert.AreEqual(output, result);
        }


        [TestCase]
        public void ConvertUIntegerToStringNoSpecific()
        {
            ConvertUIntegerToString(new NumberNoSpecific(), 1, "1");
        }

        [TestCase]
        public void ConvertStringToUIntegerNoSpecific()
        {
            ConvertStringToUInteger(new NumberNoSpecific(), "1", 1);
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToUIntegerOther()
        {
            ConvertStringToUInteger(new NumberNoSpecific(), "dd", 1);
        }

        public void ConvertUIntegerToString(INumberConverter conv, uint input, string output)
        {
            var result = conv.ToString(input);
            Assert.AreEqual(output, result);
        }

        public void ConvertStringToUInteger(INumberConverter conv, string input, uint output)
        {
            var result = conv.ToUInteger(input);
            Assert.AreEqual(output, result);
        }

        [TestCase]
        public void ConvertIntegerToStringNoSpecific()
        {
            ConvertIntegerToString(new NumberNoSpecific(), 1, "1");
        }

        [TestCase]
        public void ConvertStringToIntegerNoSpecific()
        {
            ConvertStringToInteger(new NumberNoSpecific(), "1", 1);
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToIntegerOther()
        {
            ConvertStringToInteger(new NumberNoSpecific(), "dd", 1);
        }

        public void ConvertIntegerToString(INumberConverter conv, int input, string output)
        {
            var result = conv.ToString(input);
            Assert.AreEqual(output, result);
        }

        public void ConvertStringToInteger(INumberConverter conv, string input, int output)
        {
            var result = conv.ToInteger(input);
            Assert.AreEqual(output, result);
        }
    }
}
