using System;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sqlite.UnitTest
{
    [TestFixture]
    public class SqliteDataStoreTest
    {
        private const string INTEGER = "integer";
        private const string REAL = "real";
        private const string TEXT = "text";

        [TestCase]
        public void ToSqlType_WithUnsignedShortType_ShouldCallGetUnsignedShortColumnType()
        {
            var sqliteDataStore = new SqliteDataStore();
            Assert.AreEqual(INTEGER, sqliteDataStore.ToSqlType(typeof(ushort)));
        }

        [TestCase]
        public void ToSqlType_WithShortType_ShouldCallGetShortColumnType()
        {
            var sqliteDataStore = new SqliteDataStore();
            Assert.AreEqual(INTEGER, sqliteDataStore.ToSqlType(typeof(short)));
        }

        [TestCase]
        public void ToSqlType_WithUnsignedIntType_ShouldCallGetUnsignedIntColumnType()
        {
            var sqliteDataStore = new SqliteDataStore();
            Assert.AreEqual(INTEGER, sqliteDataStore.ToSqlType(typeof(uint)));
        }

        [TestCase]
        public void ToSqlType_WithIntType_ShouldCallGetIntColumnType()
        {
            var sqliteDataStore = new SqliteDataStore();
            Assert.AreEqual(INTEGER, sqliteDataStore.ToSqlType(typeof(int)));
        }

        [TestCase]
        public void ToSqlType_WithUnsignedLongType_ShouldCallGetUnsignedLongColumnType()
        {
            var sqliteDataStore = new SqliteDataStore();
            Assert.AreEqual(INTEGER, sqliteDataStore.ToSqlType(typeof(ulong)));
        }

        [TestCase]
        public void ToSqlType_WithLongType_ShouldCallGetLongColumnType()
        {
            var sqliteDataStore = new SqliteDataStore();
            Assert.AreEqual(INTEGER, sqliteDataStore.ToSqlType(typeof(long)));
        }

        [TestCase]
        public void ToSqlType_WithDecimalType_ShouldCallGetDecimalColumnType()
        {
            var sqliteDataStore = new SqliteDataStore();
            Assert.AreEqual(REAL, sqliteDataStore.ToSqlType(typeof(decimal)));
        }

        [TestCase]
        public void ToSqlType_WithFloatType_ShouldCallGetFloatColumnType()
        {
            var sqliteDataStore = new SqliteDataStore();
            Assert.AreEqual(REAL, sqliteDataStore.ToSqlType(typeof(float)));
        }

        [TestCase]
        public void ToSqlType_WithDoubleType_ShouldCallGetDoubleColumnType()
        {
            var sqliteDataStore = new SqliteDataStore();
            Assert.AreEqual(REAL, sqliteDataStore.ToSqlType(typeof(double)));
        }

        [TestCase]
        public void ToSqlType_WithStringType_ShouldCallGetStringColumnType()
        {
            var sqliteDataStore = new SqliteDataStore();
            Assert.AreEqual(TEXT, sqliteDataStore.ToSqlType(typeof(string)));
        }

        [TestCase]
        public void ToSqlType_WithObjectType_ShouldCallGetDefaultColumnType()
        {
            var sqliteDataStore = new SqliteDataStore();
            Assert.AreEqual(TEXT, sqliteDataStore.ToSqlType(typeof(object)));
        }

        [TestCase]
        public void ToSqlType_WithEnumType_ShouldCallGetDefaultColumnType()
        {
            var sqliteDataStore = new SqliteDataStore();
            Assert.AreEqual(TEXT, sqliteDataStore.ToSqlType(typeof(Enum)));
        }
    }
}
