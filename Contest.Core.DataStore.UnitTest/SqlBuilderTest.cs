using System;
using NUnit.Framework;

namespace Contest.Core.DataStore.UnitTest
{
    [TestFixture]
    public class SqlBuilderTest
    {
        [TestCase]
        public void ToSqlType_WithUnsignedShortType_ShouldCallGetUnsignedShortColumnType()
        {
            var sqlDataStore = new SqlDataStoreBaseTester();
            sqlDataStore.ToSqlType(typeof (ushort));
            Assert.IsTrue(sqlDataStore.GetUnsignedShortColumnTypeHasCalled);
        }

        [TestCase]
        public void ToSqlType_WithShortType_ShouldCallGetShortColumnType()
        {
            var sqlDataStore = new SqlDataStoreBaseTester();
            sqlDataStore.ToSqlType(typeof(short));
            Assert.IsTrue(sqlDataStore.GetShortColumnTypeHasCalled);
        }

        [TestCase]
        public void ToSqlType_WithUnsignedIntType_ShouldCallGetUnsignedIntColumnType()
        {
            var sqlDataStore = new SqlDataStoreBaseTester();
            sqlDataStore.ToSqlType(typeof(uint));
            Assert.IsTrue(sqlDataStore.GetUnsignedIntColumnTypeHasCalled);
        }

        [TestCase]
        public void ToSqlType_WithIntType_ShouldCallGetIntColumnType()
        {
            var sqlDataStore = new SqlDataStoreBaseTester();
            sqlDataStore.ToSqlType(typeof(int));
            Assert.IsTrue(sqlDataStore.GetIntColumnTypeHasCalled);
        }

        [TestCase]
        public void ToSqlType_WithUnsignedLongType_ShouldCallGetUnsignedLongColumnType()
        {
            var sqlDataStore = new SqlDataStoreBaseTester();
            sqlDataStore.ToSqlType(typeof(ulong));
            Assert.IsTrue(sqlDataStore.GetUnsignedLongColumnTypeHasCalled);
        }

        [TestCase]
        public void ToSqlType_WithLongType_ShouldCallGetLongColumnType()
        {
            var sqlDataStore = new SqlDataStoreBaseTester();
            sqlDataStore.ToSqlType(typeof(long));
            Assert.IsTrue(sqlDataStore.GetLongColumnTypeHasCalled);
        }

        [TestCase]
        public void ToSqlType_WithDecimalType_ShouldCallGetDecimalColumnType()
        {
            var sqlDataStore = new SqlDataStoreBaseTester();
            sqlDataStore.ToSqlType(typeof(decimal));
            Assert.IsTrue(sqlDataStore.GetDecimalColumnTypeHasCalled);
        }

        [TestCase]
        public void ToSqlType_WithFloatType_ShouldCallGetFloatColumnType()
        {
            var sqlDataStore = new SqlDataStoreBaseTester();
            sqlDataStore.ToSqlType(typeof(float));
            Assert.IsTrue(sqlDataStore.GetFloatColumnTypeHasCalled);
        }

        [TestCase]
        public void ToSqlType_WithDoubleType_ShouldCallGetDoubleColumnType()
        {
            var sqlDataStore = new SqlDataStoreBaseTester();
            sqlDataStore.ToSqlType(typeof(double));
            Assert.IsTrue(sqlDataStore.GetDoubleColumnTypeHasCalled);
        }

        [TestCase]
        public void ToSqlType_WithStringType_ShouldCallGetStringColumnType()
        {
            var sqlDataStore = new SqlDataStoreBaseTester();
            sqlDataStore.ToSqlType(typeof(string));
            Assert.IsTrue(sqlDataStore.GetStringColumnTypeHasCalled);
        }

        [TestCase]
        public void ToSqlType_WithObjectType_ShouldCallGetDefaultColumnType()
        {
            var sqlDataStore = new SqlDataStoreBaseTester();
            sqlDataStore.ToSqlType(typeof(object));
            Assert.IsTrue(sqlDataStore.GetDefaultColumnTypeHasCalled);
        }

        [TestCase]
        public void ToSqlType_WithEnumType_ShouldCallGetDefaultColumnType()
        {
            var sqlDataStore = new SqlDataStoreBaseTester();
            sqlDataStore.ToSqlType(typeof(Enum));
            Assert.IsTrue(sqlDataStore.GetDefaultColumnTypeHasCalled);
        }
    }
}
