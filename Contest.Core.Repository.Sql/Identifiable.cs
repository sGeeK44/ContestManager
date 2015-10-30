using System;
using System.Runtime.Serialization;

namespace Contest.Core.Repository.Sql
{
    /// <summary>
    /// Represent a class with a unique Guid identifier
    /// </summary>
    [Serializable]
    public class Identifiable<T> : IIdentifiable
    {
        private Guid _id;

        /// <summary>
        /// Create a new instance of Identifiable class with new Guid.
        /// </summary>
        public Identifiable()
        {
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Get the unique Guid Identifier for current instance
        /// </summary>
        [SqlPrimaryKey]
        [DataMember]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(T other)
        {
            return Equals((object) other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if(!(obj is IIdentifiable)) return false;
            return obj.GetType() == GetType() && Equals((IIdentifiable)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IIdentifiable other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || Id == other.Id;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Determines whether two specified Identifiable have different value.
        /// </summary>
        /// <param name="a">The first Identifiable to compare, or null.</param>
        /// <param name="b">The second Identifiable to compare, or null.</param>
        /// <returns>true if the value of a is different from the value of b; otherwise, false.</returns>
        public static bool operator !=(Identifiable<T> a, Identifiable<T> b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Determines whether two specified Identifiable have same value.
        /// </summary>
        /// <param name="a">The first Identifiable to compare, or null.</param>
        /// <param name="b">The second Identifiable to compare, or null.</param>
        /// <returns>true if the value of a is same object from the value of b; otherwise, false.</returns>
        public static bool operator ==(Identifiable<T> a, Identifiable<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            return !ReferenceEquals(a, null) ? a.Equals((object) b) : b.Equals(null);
        }
    }
}