using System;
using NUnit.Framework;

namespace Contest.Core.Repository.Sql.UnitTest
{
    [TestFixture]
    public class IdentifiableTest
    {
        [TestCase]
        public void AreSame_CompareToNull_ShouldBeNotEqual()
        {
            // Arrange
            var field1 = new Identifiable<string>();

            Assert.IsFalse(field1.AreSame((IIdentifiable)null));
            Assert.IsFalse(field1.AreSame((string)null));
        }

        [TestCase]
        public void AreSame_CompareToObjectWithoutSameId_ShouldBeNotEqual()
        {
            // Arrange
            var field1 = new Identifiable<string>();
            var field2 = new Identifiable<string>();

            Assert.IsFalse(field1.AreSame(field2));
            Assert.IsFalse(field2.AreSame(field1));
        }

        [TestCase]
        public void AreSame_CompareToObjectWithSameId_ShouldBeEqual()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var field1 = new Identifiable<string> { Id = guid };
            var field2 = new Identifiable<string> { Id = guid };
            var field3 = new Identifiable<string> { Id = guid };

            // Reflexive 
            Assert.IsTrue(field1.AreSame(field1));
            Assert.IsTrue(field2.AreSame(field2));

            // Symmetric
            Assert.IsTrue(field1.AreSame(field2));
            Assert.IsTrue(field2.AreSame(field1));

            // Transitive 
            Assert.IsTrue(field1.AreSame(field2));
            Assert.IsTrue(field2.AreSame(field3));
            Assert.IsTrue(field3.AreSame(field1));
        }
    }
}
