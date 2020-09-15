using System;
using SmartWay.Orm.Attributes;
using SmartWay.Orm.Constants;
using SmartWay.Orm.Entity;
using SmartWay.Orm.Entity.Constraints;

namespace Contest.Domain
{
    /// <summary>
    ///     Encaspulate common behavior for standard entity
    /// </summary>
    public abstract class Entity : IDistinctableEntity, IEquatable<Entity>
    {
        public const string IdColumnName = "id";

        /// <summary>
        ///     Get unique object identifier
        /// </summary>
        [PrimaryKey(KeyScheme.GUID, FieldName = IdColumnName, DefaultValue = DefaultValue.RandomGuid)]
        public Guid Id { get; set; }

        /// <inheritdoc />
        public string GetPkColumnName()
        {
            return IdColumnName;
        }

        /// <inheritdoc />
        public object GetPkValue()
        {
            return Id;
        }

        public bool Equals(Entity other)
        {
            return Equals(Id, other?.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Entity)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}