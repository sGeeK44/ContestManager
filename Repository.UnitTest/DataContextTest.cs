using System;
using System.Collections.Generic;
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
        public void IsExistTest_EmptyContext(string itemToTest, bool isShouldExist)
        {
            // Arrange
            var dataContext = new DataContext<string>();

            // Assert
            Assert.AreEqual(isShouldExist, dataContext.IsExist(itemToTest));
        }

        [TestCase("Test", "Test", true)]
        [TestCase("Test", "test", false)]
        public void IsExistTest_PopulateContext(string item, string itemToTest, bool isShouldExist)
        {
            // Arrange
            var dataContext = new DataContext<string>();
            dataContext.Insert(item);

            // Assert
            Assert.AreEqual(isShouldExist, dataContext.IsExist(itemToTest));
        }

        [TestCase(ExpectedException = typeof(ArgumentNullException))]
        public void InsertNullElement()
        {
            // Arrange
            var dataContext = new DataContext<string>();

            // Act
            dataContext.Insert(null);
        }

        [TestCase]
        public void InsertElement_DoesntExistYet()
        {
            // Arrange
            const string ITEM = "Test";
            var dataContext = new DataContext<string>();

            // Act
            dataContext.Insert(ITEM);

            // Assert
            CollectionAssert.AreEqual(new List<string> { ITEM }, dataContext.Find(_ => _ == ITEM));
        }

        [TestCase]
        public void InsertElementTwoElement_DoesntExistYet()
        {
            // Arrange
            const string ITEM = "Test";
            const string ITEM2 = "test";
            var dataContext = new DataContext<string>();

            // Act
            dataContext.Insert(ITEM);
            dataContext.Insert(ITEM2);

            // Assert
            CollectionAssert.AreEqual(new List<string>{ ITEM }, dataContext.Find(_ => _ == ITEM));
            CollectionAssert.AreEqual(new List<string>{ ITEM2 }, dataContext.Find(_ => _ == ITEM2));
        }

        [TestCase(ExpectedException = typeof(ItemAlreadyExistException))]
        public void InsertElement_AlreadyExistYet()
        {
            // Arrange
            const string ITEM = "Test";
            var dataContext = new DataContext<string>();
            dataContext.Insert(ITEM);

            // Act
            dataContext.Insert(ITEM);
        }

        [TestCase]
        public void DeleteElement_AlreadyExistYet()
        {
            // Arrange
            const string ITEM = "Test";
            var dataContext = new DataContext<string>();

            // Act
            dataContext.Insert(ITEM);
            dataContext.Delete(ITEM);

            // Assert
            CollectionAssert.IsEmpty(dataContext.Find(_ => _ == ITEM));
        }

        [TestCase]
        public void DeleteElementTwoElement_DoesntExistYet()
        {
            // Arrange
            const string ITEM = "Test";
            const string ITEM2 = "test";
            var dataContext = new DataContext<string>();
            dataContext.Insert(ITEM);
            dataContext.Insert(ITEM2);

            // Act
            dataContext.Delete(ITEM);
            dataContext.Delete(ITEM2);

            // Assert
            CollectionAssert.IsEmpty(dataContext.Find(_ => _ == ITEM));
            CollectionAssert.IsEmpty(dataContext.Find(_ => _ == ITEM2));
        }

        [TestCase(ExpectedException = typeof(ItemNotFoundException))]
        public void DeleteElement_DoesntExistYet()
        {
            // Arrange
            const string ITEM = "Test";
            var dataContext = new DataContext<string>();

            // Act
            dataContext.Delete(ITEM);
        }

        [TestCase]
        public void ClearCollection()
        {
            // Arrange
            const string ITEM = "Test";
            const string ITEM2 = "test";
            var dataContext = new DataContext<string>();
            dataContext.Insert(ITEM);
            dataContext.Insert(ITEM2);

            // Act
            dataContext.Clear();

            // Assert
            CollectionAssert.IsEmpty(dataContext.Find(_ => _ == ITEM));
            CollectionAssert.IsEmpty(dataContext.Find(_ => _ == ITEM2));
        }

        [TestCase]
        public void FirstOrDefault()
        {
            // Arrange
            const string ITEM = "Test";
            var dataContext = new DataContext<string>();
            dataContext.Insert(ITEM);
            
            // Assert
            Assert.AreEqual(ITEM, dataContext.FirstOrDefault(_ => _ == ITEM));
            Assert.AreEqual(null, dataContext.FirstOrDefault(_ => _ == string.Empty));
            Assert.AreEqual(null, dataContext.FirstOrDefault(_ => _ == null));
        }

        [TestCase]
        public void Find()
        {
            // Arrange
            const string ITEM = "Test";
            const string ITEM2 = "Test2";
            const string ITEM3 = "Test3";
            const string ITEM4 = "Tet3";
            var dataContext = new DataContext<string>();
            dataContext.Insert(ITEM);
            dataContext.Insert(ITEM2);
            dataContext.Insert(ITEM3);
            dataContext.Insert(ITEM4);

            // Act
            var result = dataContext.Find(_ => _.Contains("Test"));

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Contains(ITEM));
            Assert.IsTrue(result.Contains(ITEM2));
            Assert.IsTrue(result.Contains(ITEM3));
            Assert.IsFalse(result.Contains(ITEM4));
        }
        
        //internal void Update(T itemToUpdate)
        //{
        //    T existingItem = GetItem(_itemList, itemToUpdate);
        //    if (existingItem == null) throw new NotFoundException(typeof(T));

        //    _itemList.Remove(existingItem);
        //    _itemList.Add(itemToUpdate);
        //}
        //internal T GetFirstOrDefault(Func<T, bool> predicate)
        //{
        //    return _itemList.FirstOrDefault(predicate);
        //}
    }
}
