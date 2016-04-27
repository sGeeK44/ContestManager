using System;
using System.Collections.Generic;
using System.Linq;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;

namespace Contest.Core.Repository.FeatureTest
{
    [SqlEntity]
    public class ManyToManyFirstEntity : IIdentifiable, IQueryable, IEquatable<ManyToManyFirstEntity>
    {
        [SqlPrimaryKey]
        public Guid Id { get; set; }

        [SqlManyToManyReference]
        public IList<Relationship<ManyToManyFirstEntity, ManyToManySecondEntity>> SecondEntityList { get; set; }

        public bool AreSame(object other)
        {
            return AreSame(other as ManyToManyFirstEntity);
        }

        public bool AreSame(ManyToManyFirstEntity other)
        {
            if (other == null) return false;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ManyToManyFirstEntity);
        }

        public bool Equals(ManyToManyFirstEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!Id.Equals(other.Id)) return false;
            
            if (!SecondEntityList.Count.Equals(other.SecondEntityList.Count)) return false;
            return !SecondEntityList.Where((t, i) => !t.AreSame(other.SecondEntityList[i])).Any();
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode()*397) ^ (SecondEntityList != null ? SecondEntityList.GetHashCode() : 0);
            }
        }

        public static bool operator ==(ManyToManyFirstEntity left, ManyToManyFirstEntity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ManyToManyFirstEntity left, ManyToManyFirstEntity right)
        {
            return !Equals(left, right);
        }
    }
}
