using System;
using Contest.Core.Converters;
using Contest.Core.Converters.DateTimeConverter;
using NUnit.Framework;

namespace Contest.Core.UnitTest.Converter
{
    [TestFixture]
    public class DateTimeConverter
    {
        [TestCase]
        public void ConvertDateTimeToStringDb2()
        {
            ConvertDateTimeToString(new DateTimeDb2(), new DateTime(2000, 01, 01), "2000-01-01");
        }

        [TestCase]
        public void ConvertStringToDateTimeDb2()
        {
            ConvertStringToDateTime(new DateTimeDb2(), "2000-01-01", new DateTime(2000, 01, 01));
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToDateTimeDb2Other()
        {
            ConvertStringToDateTime(new DateTimeDb2(), "0101-2000", new DateTime(2000, 01, 01));
        }

        [TestCase]
        public void ConvertDateTimeToStringHost()
        {
            ConvertDateTimeToString(new DateTimeHost(), new DateTime(2000, 01, 01), "20000101");
        }

        [TestCase]
        public void ConvertStringToDateTimeHost()
        {
            ConvertStringToDateTime(new DateTimeHost(), "20000101", new DateTime(2000, 01, 01));
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToDateTimeHostOther()
        {
            ConvertStringToDateTime(new DateTimeHost(), "0101-2000", new DateTime(2000, 01, 01));
        }

        [TestCase]
        public void ConvertDateTimeToStringSpecific()
        {
            ConvertDateTimeToString(new DateTimeSpecific("yyyy"), new DateTime(2000, 01, 01), "2000");
        }

        [TestCase]
        public void ConvertStringToDateTimeSpecific()
        {
            ConvertStringToDateTime(new DateTimeSpecific("yyyy"), "2000", new DateTime(2000, 01, 01));
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToDateTimeSpecificOther()
        {
            ConvertStringToDateTime(new DateTimeSpecific("yyyy"), "0101-2000", new DateTime(2000, 01, 01));
        }

        [TestCase]
        public void ConvertDateTimeToStringTimestamp()
        {
            ConvertDateTimeToString(new DateTimeTimestamp(), new DateTime(2000, 01, 01), "2000-01-01-00.00.00.000000");
        }

        [TestCase]
        public void ConvertStringToDateTimeTimestamp()
        {
            ConvertStringToDateTime(new DateTimeTimestamp(), "2000-01-01-00.00.00.000000", new DateTime(2000, 01, 01));
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToDateTimeTimestampOther()
        {
            ConvertStringToDateTime(new DateTimeTimestamp(), "0101-2000", new DateTime(2000, 01, 01));
        }

        public void ConvertDateTimeToString(IDateTimeConverter conv, DateTime input, string output)
        {
            var result = conv.ToString(input);
            Assert.AreEqual(output, result);
        }

        public void ConvertStringToDateTime(IDateTimeConverter conv, string input, DateTime output)
        {
            var result = conv.ToDateTime(input);
            Assert.AreEqual(output, result);
        }
    }
}
