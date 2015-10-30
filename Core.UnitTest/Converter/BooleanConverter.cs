using Contest.Core.Converters;
using Contest.Core.Converters.BooleanConverter;
using NUnit.Framework;

namespace Contest.Core.UnitTest.Converter
{
    [TestFixture]
    public class BooleanConverter
    {
        [TestCase]
        public void ConvertBoolToStringOBlankTrue()
        {
            ConvertBoolToString(new BooleanOBlank(), true, "O");
        }

        [TestCase]
        public void ConvertBoolToStringOBlankFalse()
        {
            ConvertBoolToString(new BooleanOBlank(), false, string.Empty);
        }

        [TestCase]
        public void ConvertStringToBoolOBlankTrue()
        {
            ConvertStringToBool(new BooleanOBlank(), "O", true);
        }

        [TestCase]
        public void ConvertStringToBoolOBlankFalse()
        {
            ConvertStringToBool(new BooleanOBlank(), string.Empty, false);
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToBoolOBlankOther()
        {
            ConvertStringToBool(new BooleanOBlank(), "C", false);
        }

        [TestCase]
        public void ConvertBoolToStringONTrue()
        {
            ConvertBoolToString(new BooleanON(), true, "O");
        }

        [TestCase]
        public void ConvertBoolToStringONFalse()
        {
            ConvertBoolToString(new BooleanON(), false, "N");
        }

        [TestCase]
        public void ConvertStringToBoolONTrue()
        {
            ConvertStringToBool(new BooleanON(), "O", true);
        }

        [TestCase]
        public void ConvertStringToBoolONFalse()
        {
            ConvertStringToBool(new BooleanON(), "N", false);
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToBoolONOther()
        {
            ConvertStringToBool(new BooleanON(), "C", false);
        }
        
        [TestCase]
        public void ConvertBoolToStringOneZeroTrue()
        {
            ConvertBoolToString(new BooleanOneZero(), true, "1");
        }

        [TestCase]
        public void ConvertBoolToStringOneZeroFalse()
        {
            ConvertBoolToString(new BooleanOneZero(), false, "0");
        }

        [TestCase]
        public void ConvertStringToBoolOneZeroTrue()
        {
            ConvertStringToBool(new BooleanOneZero(), "1", true);
        }

        [TestCase]
        public void ConvertStringToBoolOneZeroFalse()
        {
            ConvertStringToBool(new BooleanOneZero(), "0", false);
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToBoolOneZeroOther()
        {
            ConvertStringToBool(new BooleanOneZero(), "C", false);
        }

        [TestCase]
        public void ConvertBoolToStringWildCardBlankTrue()
        {
            ConvertBoolToString(new BooleanWildCardBlank(), true, "*");
        }

        [TestCase]
        public void ConvertBoolToStringWildCardBlankFalse()
        {
            ConvertBoolToString(new BooleanWildCardBlank(), false, string.Empty);
        }

        [TestCase]
        public void ConvertStringToBoolWildCardBlankTrue()
        {
            ConvertStringToBool(new BooleanWildCardBlank(), "*", true);
        }

        [TestCase]
        public void ConvertStringToBoolWildCardBlankFalse()
        {
            ConvertStringToBool(new BooleanWildCardBlank(), string.Empty, false);
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToBoolWildCardBlankOther()
        {
            ConvertStringToBool(new BooleanWildCardBlank(), "C", false);
        }

        [TestCase]
        public void ConvertBoolToStringYNTrue()
        {
            ConvertBoolToString(new BooleanYN(), true, "Y");
        }

        [TestCase]
        public void ConvertBoolToStringYNFalse()
        {
            ConvertBoolToString(new BooleanYN(), false, "N");
        }

        [TestCase]
        public void ConvertStringToBoolYNTrue()
        {
            ConvertStringToBool(new BooleanYN(), "Y", true);
        }

        [TestCase]
        public void ConvertStringToBoolYNFalse()
        {
            ConvertStringToBool(new BooleanYN(), "N", false);
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToBoolYNOther()
        {
            ConvertStringToBool(new BooleanYN(), "C", false);
        }

        public void ConvertBoolToString(IBooleanConverter conv, bool input, string output)
        {
            var result = conv.ToString(input);
            Assert.AreEqual(output, result);
        }

        public void ConvertStringToBool(IBooleanConverter conv, string input, bool output)
        {
            var result = conv.ToBoolean(input);
            Assert.AreEqual(output, result);
        }
    }
}
