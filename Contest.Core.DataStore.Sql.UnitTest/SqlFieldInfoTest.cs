﻿using Contest.Core.DataStore.Sql.UnitTest.Entities;
using NUnit.Framework;

namespace Contest.Core.DataStore.Sql.UnitTest
{
    [TestFixture]
    public class SqlFieldInfoTest
    {
        [TestCase]
        public void GetSqlField_BasicEntity_AllMarkedProperty()
        {
            var fields = EntityInfoFactory.GetSqlField<BasicEntity>();
            Assert.AreEqual(3, fields.Count);
        }
    }
}
