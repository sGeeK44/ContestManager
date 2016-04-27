using System;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;

namespace Contest.Core.Repository.FeatureTest
{
    [SqlEntity]
    public class ManyToOneEntity : IIdentifiable, IQueryable, IEquatable<ManyToOneEntity>
    {
        public bool Equals(ManyToOneEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id) && OneToManyEntityId.Equals(other.OneToManyEntityId) && Equals(Entity, other.Entity);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode*397) ^ OneToManyEntityId.GetHashCode();
                hashCode = (hashCode*397) ^ (Entity != null ? Entity.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(ManyToOneEntity left, ManyToOneEntity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ManyToOneEntity left, ManyToOneEntity right)
        {
            return !Equals(left, right);
        }

        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [SqlForeignKey(typeof(OneToManyEntity))]
        public Guid OneToManyEntityId { get; set; }

        [SqlManyToOneReference]
        public OneToManyEntity Entity { get; set; }

        public bool AreSame(object other)
        {
            return AreSame(other as ManyToOneEntity);
        }

        public bool AreSame(ManyToOneEntity manyToOneEntity)
        {
            if (manyToOneEntity == null) return false;
            return Id == manyToOneEntity.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ManyToOneEntity);
        }
    }
}
