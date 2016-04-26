using System;
using System.Collections.Generic;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.Repository.FeatureTest
{
    [SqlEntity]
    public class MainEntity : IEntity
    {
        public struct FieldProperty
        {
            public string ColumnName { get; set; }
            public string ColumnType { get; set; }
            public bool IsPrimaryKey { get; set; }
        }

        public const string TableName = "MainEntity";
        public static List<FieldProperty> Fields = GetStructure();

        private static List<FieldProperty> GetStructure()
        {
            var result = new List<FieldProperty>
            {
                new FieldProperty {ColumnName = "Key", ColumnType = "text", IsPrimaryKey = true},
                new FieldProperty {ColumnName = "BasicField", ColumnType = "text"},
                new FieldProperty {ColumnName = "OVERRIDE_COLUMN_NAME", ColumnType = "text"},
                new FieldProperty {ColumnName = "IntegerField", ColumnType = "integer"}
            };
            return result;
        }

        [SqlPrimaryKey]
        public Guid Key { get; set; }

        [SqlField]
        public string BasicField { get; set; }

        [SqlField(Name = "OVERRIDE_COLUMN_NAME")]
        public string OverrideColumnName { get; set; }

        [SqlField]
        public int IntegerField { get; set; }

        public bool AreSame(object other)
        {
            throw new System.NotImplementedException();
        }
    }
}