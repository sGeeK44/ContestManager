using System;
using System.Collections.Generic;
using System.Linq;
using Contest.Core.DataStore.Sql.Attributes;

namespace Contest.Core.Repository.FeatureTest
{
    [SqlEntity]
    public class OneToManyEntity : IQueryable, IEquatable<OneToManyEntity>
    {
        public bool Equals(OneToManyEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!Id.Equals(other.Id)) return false;
            if (!EntityList.Count.Equals(other.EntityList.Count)) return false;
            return !EntityList.Where((t, i) => !t.Id.Equals(other.EntityList[i].Id)).Any();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode()*397) ^ (EntityList != null ? EntityList.GetHashCode() : 0);
            }
        }

        public static bool operator ==(OneToManyEntity left, OneToManyEntity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(OneToManyEntity left, OneToManyEntity right)
        {
            return !Equals(left, right);
        }

        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [SqlOneToManyReference]
        public IList<ManyToOneEntity> EntityList { get; set; }

        public bool AreSame(object other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as OneToManyEntity);
        }
    }
}