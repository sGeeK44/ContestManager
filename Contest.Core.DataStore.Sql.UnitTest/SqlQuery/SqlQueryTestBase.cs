using System;
using Contest.Core.DataStore.Sql.SqlQuery;
using Moq;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest.SqlQuery
{
    public class SqlQueryTestBase
    {
        public const string ConvertedValue = "#ConvertedValue#";
        public const string ColumnType = "#ColumnType#";

        public Mock<ISqlProviderStrategy> SqlStrategy { get; set; }
        public IEntityInfoFactory EntityInfoFactory { get; set; }

        [SetUp]
        public virtual void Init()
        {
            SqlStrategy = new Mock<ISqlProviderStrategy>();
            SqlStrategy.Setup(_ => _.ToSqlValue(It.IsAny<object>(), It.IsAny<object[]>())).Returns(ConvertedValue);
            SqlStrategy.Setup(_ => _.ToSqlType(It.IsAny<Type>())).Returns(ColumnType);
            EntityInfoFactory = new EntityInfoFactory();
        }

        public SqlQueryFactory<T, T> CreateSqlQueryFactory<T>() where T : class
        {
            return CreateSqlQueryFactory<T, T>();
        }

        public SqlQueryFactory<T, TI> CreateSqlQueryFactory<T, TI>()
            where T : class, TI
            where TI : class
        {
            return new SqlQueryFactory<T, TI>(SqlStrategy.Object, EntityInfoFactory);
        }
    }
}
