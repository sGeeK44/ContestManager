using System;
using Contest.Core.Repository.Exceptions;
using NUnit.Framework;

namespace Contest.Core.Repository.UnitTest
{
    [TestFixture]
    public class DataContextTest
    {
        [TestCase(null, false)]
        [TestCase("Test", false)]
        [TestCase("test", false)]
        public void IsExist_EmptyContext_ShouldReturnIsSouldExist(string itemToTest, bool isShouldExist)
        {
            // Arrange
            var dataContext = new DataContext<StringTester>();

            // Assert
            Assert.AreEqual(isShouldExist, dataContext.IsExist(new StringTester(itemToTest)));
        }

        [TestCase("Test", "Test", true)]
        [TestCase("Test", "test", false)]
        public void IsExist_PopulateContext_ShouldReturnIsSouldExist(string item, string itemToTest, bool isShouldExist)
        {
            // Arrange
            var dataContext = new DataContext<StringTester>();
            dataContext.Insert(new StringTester(item));

            // Assert
            Assert.AreEqual(isShouldExist, dataContext.IsExist(new StringTester(itemToTest)));
        }

        [TestCase(ExpectedException = typeof(ArgumentNullException))]
        public void Insert_NullElement_ShouldThrowException()
        {
            // Arrange
            var dataContext = new DataContext<StringTester>();

            // Act
            dataContext.Insert(null);
        }

        [TestCase]
        public void Insert_ItemDoesntExistYet_DataShouldAppendIt()
        {
            // Arrange
            const string ITEM = "Test";
            var dataContext = new DataContext<StringTester>();

            // Act
            dataContext.Insert(new StringTester(ITEM));

            // Assert
            Assert.AreEqual(1, dataContext.Count);
        }

        [TestCase]
        public void Insert_TwoItemDoesntExistYet_ShouldAppendIt()
        {
            // Arrange
            const string ITEM = "Test";
            const string ITEM2 = "test";
            var dataContext = new DataContext<StringTester>();

            // Act
            dataContext.Insert(new StringTester(ITEM));
            dataContext.Insert(new StringTester(ITEM2));

            // Assert
            Assert.AreEqual(2, dataContext.Count);
        }

        [TestCase(ExpectedException = typeof(ItemAlreadyExistException))]
        public void Insert_ItemAlreadyExistYet_ShouldThrowException()
        {
            // Arrange
            const string ITEM = "Test";
            var dataContext = new DataContext<StringTester>();
            dataContext.Insert(new StringTester(ITEM));

            // Act
            dataContext.Insert(new StringTester(ITEM));
        }

        [TestCase(ExpectedException = typeof(ArgumentNullException))]
        public void Update_NullElement_ShouldThrowException()
        {
            // Arrange
            var dataContext = new DataContext<StringTester>();

            // Act
            dataContext.Update(null);
        }

        [TestCase(ExpectedException = typeof(ItemNotFoundException))]
        public void Update_ItemDoesntExistYet_DataShouldThrowException()
        {
            // Arrange
            const string ITEM = "Test";
            var dataContext = new DataContext<StringTester>();

            // Act
            dataContext.Update(new StringTester(ITEM));
        }

        [TestCase]
        public void Update_ItemAlreadyExistYet_ShouldReplaceIt()
        {
            // Arrange
            const string ITEM = "Test";
            var expected = new StringTester(ITEM);
            var dataContext = new DataContext<StringTester>();
            dataContext.Insert(new StringTester(ITEM));

            // Act
            dataContext.Update(new StringTester(ITEM));
        }

        [TestCase]
        public void Delete_ItemExist_ShouldRemoveItem()
        {
            // Arrange
            const string ITEM = "Test";
            var dataContext = new DataContext<StringTester>();
            dataContext.Insert(new StringTester(ITEM));

            // Act
            dataContext.Delete(new StringTester(ITEM));

            // Assert
            Assert.AreEqual(0, dataContext.Count);
        }

        [TestCase]
        public void Delete_TwoItemExist_ShouldRemoveThem()
        {
            // Arrange
            const string ITEM = "Test";
            const string ITEM2 = "test";
            var dataContext = new DataContext<StringTester>();
            dataContext.Insert(new StringTester(ITEM));
            dataContext.Insert(new StringTester(ITEM2));

            // Act
            dataContext.Delete(new StringTester(ITEM));
            dataContext.Delete(new StringTester(ITEM2));

            // Assert
            Assert.AreEqual(0, dataContext.Count);
        }

        [TestCase(ExpectedException = typeof(ItemNotFoundException))]
        public void Delete_ItemDoesntExist_ShouldThrowException()
        {
            // Arrange
            const string ITEM = "Test";
            var dataContext = new DataContext<StringTester>();

            // Act
            dataContext.Delete(new StringTester(ITEM));
        }

        [TestCase]
        public void Clear_PopulateDataContext_ShouldBeEmpty()
        {
            // Arrange
            const string ITEM = "Test";
            const string ITEM2 = "test";
            var dataContext = new DataContext<StringTester>();
            dataContext.Insert(new StringTester(ITEM));
            dataContext.Insert(new StringTester(ITEM2));

            // Act
            dataContext.Clear();

            // Assert
            Assert.AreEqual(0, dataContext.Count);
        }

        [TestCase]
        public void FirstOrDefault_PredicateMatchOneItem_ShouldReturnItem()
        {
            // Arrange
            const string ITEM = "Test";
            var expected = new StringTester(ITEM);
            var dataContext = new DataContext<StringTester>();
            dataContext.Insert(expected);
            
            // Act and Assert
            Assert.AreEqual(expected, dataContext.FirstOrDefault(_ => _.ItemToTest == ITEM));
        }

        [TestCase]
        public void FirstOrDefault_PredicateMatchZeroItem_ShouldReturnNull()
        {
            // Arrange
            const string ITEM = "Test";
            var dataContext = new DataContext<StringTester>();
            dataContext.Insert(new StringTester(ITEM));

            // Act and Assert
            Assert.AreEqual(null, dataContext.FirstOrDefault(_ => _.ItemToTest == string.Empty));
        }

        [TestCase]
        public void Find_PredicateMatchingZeroItem_ShouldReturnEmptyCollection()
        {
            // Arrange
            const string ITEM = "Test";
            var dataContext = new DataContext<StringTester>();

            // Act
            var result = dataContext.Find(_ => _.ItemToTest.Contains(ITEM));

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestCase]
        public void Find_PredicateMatchingOneItem_ShouldReturnNewCollectionWithItemMatched()
        {
            // Arrange
            const string ITEM = "Test";
            const string ITEM2 = "Tet3";
            var dataContext = new DataContext<StringTester>();
            dataContext.Insert(new StringTester(ITEM));
            dataContext.Insert(new StringTester(ITEM2));

            // Act
            var result = dataContext.Find(_ => _.ItemToTest.Contains(ITEM));

            // Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestCase]
        public void Find_PredicateMatchingTwoItem_ShouldReturnNewCollectionWithItemsMatched()
        {
            // Arrange
            const string ITEM = "Test";
            const string ITEM2 = "Test2";
            const string ITEM3 = "Tet3";
            var dataContext = new DataContext<StringTester>();
            dataContext.Insert(new StringTester(ITEM));
            dataContext.Insert(new StringTester(ITEM2));
            dataContext.Insert(new StringTester(ITEM3));

            // Act
            var result = dataContext.Find(_ => _.ItemToTest.Contains(ITEM));

            // Assert
            Assert.AreEqual(2, result.Count);
        }
    }
}
