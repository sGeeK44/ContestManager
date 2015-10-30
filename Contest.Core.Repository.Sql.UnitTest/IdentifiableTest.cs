using System;
using NUnit.Framework;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [TestFixture]
    public class IdentifiableTest
    {
        [TestCase]
        public void ShouldBeNotEqual_Null()
        {
            // Arrange
            var field1 = new Identifiable<string>();

            Assert.IsFalse(field1.Equals((IIdentifiable)null));
            Assert.IsFalse(field1.Equals((string)null));
        }

        [TestCase]
        public void ShouldBeNotEqual()
        {
            // Arrange
            var field1 = new Identifiable<string>();
            var field2 = new Identifiable<string>();

            Assert.IsFalse(field1.Equals(field2));
            Assert.IsFalse(field2.Equals(field1));
        }

        [TestCase]
        public void ShouldBeEqual()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var field1 = new Identifiable<string> { Id = guid };
            var field2 = new Identifiable<string> { Id = guid };
            var field3 = new Identifiable<string> { Id = guid };

            // Reflexive 
            Assert.IsTrue(field1.Equals(field1));
            Assert.IsTrue(field2.Equals(field2));

            // Symmetric
            Assert.IsTrue(field1.Equals(field2));
            Assert.IsTrue(field2.Equals(field1));

            // Transitive 
            Assert.IsTrue(field1.Equals(field2));
            Assert.IsTrue(field2.Equals(field3));
            Assert.IsTrue(field3.Equals(field1));
        }
    }
}
