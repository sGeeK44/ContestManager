using System;
using SmartWay.Orm.Attributes;
using SmartWay.Orm.Constants;
using SmartWay.Orm.Entity.Constraints;

namespace Contest.Domain
{
    /// <summary>
    ///     Encaspulate common behavior for standard entity
    /// </summary>
    public abstract class Entity
    {
        public const string IdColumnName = "id";

        /// <summary>
        ///     Get unique object identifier
        /// </summary>
        [PrimaryKey(KeyScheme.Identity, FieldName = IdColumnName, DefaultValue = DefaultValue.RandomGuid)]
        public Guid Id { get; set; }
    }
}