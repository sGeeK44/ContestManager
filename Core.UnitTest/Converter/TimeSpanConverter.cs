using System;
using Contest.Core.Converters;
using Contest.Core.Converters.TimeSpanConverter;
using NUnit.Framework;

namespace Contest.Core.UnitTest.Converter
{
    [TestFixture]
    public class TimeSpanConverter
    {
        [TestCase]
        public void ConvertTimeSpanToStringDb2()
        {
            ConvertTimeSpanToString(new TimeSpanDb2(), new TimeSpan(01, 01, 01), "01.01.01");
        }

        [TestCase]
        public void ConvertStringToTimeSpanDb2()
        {
            ConvertStringToTimeSpan(new TimeSpanDb2(), "01.01.01", new TimeSpan(01, 01, 01));
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToTimeSpanDb2Other()
        {
            ConvertStringToTimeSpan(new TimeSpanDb2(), "0101-2000", new TimeSpan(01, 01, 01));
        }

        [TestCase]
        public void ConvertTimeSpanToStringHost()
        {
            ConvertTimeSpanToString(new TimeSpanHost(), new TimeSpan(01, 01, 01), "010101");
        }

        [TestCase]
        public void ConvertStringToTimeSpanHost()
        {
            ConvertStringToTimeSpan(new TimeSpanHost(), "010101", new TimeSpan(01, 01, 01));
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToTimeSpanHostOther()
        {
            ConvertStringToTimeSpan(new TimeSpanHost(), "0101-2000", new TimeSpan(01, 01, 01));
        }

        [TestCase]
        public void ConvertTimeSpanToStringSpecific()
        {
            ConvertTimeSpanToString(new TimeSpanSpecific("hh"), new TimeSpan(01, 01, 01), "01");
        }

        [TestCase]
        public void ConvertStringToTimeSpanSpecific()
        {
            ConvertStringToTimeSpan(new TimeSpanSpecific("hh"), "01", new TimeSpan(01, 00, 00));
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToTimeSpanSpecificOther()
        {
            ConvertStringToTimeSpan(new TimeSpanSpecific("hh"), "0101-2000", new TimeSpan(01, 01, 01));
        }

        [TestCase]
        public void ConvertTimeSpanToStringHostWithoutSec()
        {
            ConvertTimeSpanToString(new TimeSpanHostWithoutSec(), new TimeSpan(01, 01, 01), "0101");
        }

        [TestCase]
        public void ConvertStringToTimeSpanHostWithoutSec()
        {
            ConvertStringToTimeSpan(new TimeSpanHostWithoutSec(), "0101", new TimeSpan(01, 01, 00));
        }

        [TestCase]
        [ExpectedException(typeof(ConverterException))]
        public void ConvertStringToTimeSpanHostWithoutSecOther()
        {
            ConvertStringToTimeSpan(new TimeSpanHostWithoutSec(), "0101-2000", new TimeSpan(01, 01, 01));
        }

        public void ConvertTimeSpanToString(ITimeSpanConverter conv, TimeSpan input, string output)
        {
            var result = conv.ToString(input);
            Assert.AreEqual(output, result);
        }

        public void ConvertStringToTimeSpan(ITimeSpanConverter conv, string input, TimeSpan output)
        {
            var result = conv.ToTimeSpan(input);
            Assert.AreEqual(output, result);
        }
    }
}
