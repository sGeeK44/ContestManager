using System;
using Contest.Core.Converters;
using Contest.Core.Converters.GuidConverter;
using NUnit.Framework;

namespace Contest.Core.UnitTest.Converter
{
    [TestFixture]
    public class GuidConverter
    {
        [TestCase]
        public void ConvertGuidToString()
        {
            ConvertGuidToString(new GuidNoSpecific(), new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"), "6A4A4F81-0C29-43C4-863E-AD10398B3A8C");
        }

        [TestCase]
        public void ConvertStringToGuid()
        {
            ConvertStringToGuid(new GuidNoSpecific(), "6A4A4F81-0C29-43C4-863E-AD10398B3A8C", new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToGuidDb2Other()
        {
            ConvertStringToGuid(new GuidNoSpecific(), "6A4A4F81-0C29-43C4-863E-AD98B3A8C", new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C"));
        }

        public void ConvertGuidToString(IGuidConverter conv, Guid input, string output)
        {
            var result = conv.ToString(input);
            Assert.AreEqual(output, result);
        }

        public void ConvertStringToGuid(IGuidConverter conv, string input, Guid output)
        {
            var result = conv.ToGuid(input);
            Assert.AreEqual(output, result);
        }
    }
}
