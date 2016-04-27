using System;
using System.Collections.Generic;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.Repository.FeatureTest
{
    [SqlEntity]
    public class MainEntity : IEntity, IEquatable<MainEntity>
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
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MainEntity);
        }
        public bool Equals(MainEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Key.Equals(other.Key) && string.Equals(BasicField, other.BasicField) && string.Equals(OverrideColumnName, other.OverrideColumnName) && IntegerField == other.IntegerField;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Key.GetHashCode();
                hashCode = (hashCode*397) ^ (BasicField != null ? BasicField.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (OverrideColumnName != null ? OverrideColumnName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ IntegerField;
                return hashCode;
            }
        }

        public static bool operator ==(MainEntity left, MainEntity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MainEntity left, MainEntity right)
        {
            return !Equals(left, right);
        }
    }
}