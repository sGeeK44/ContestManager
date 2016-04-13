using System;
using System.Runtime.Serialization;

namespace Contest.Core.Repository.Sql.UnitTest
{
    internal class ManyToOneEntity : IQueryable
    {
        private static Guid guid = new Guid("6A4A4F81-0C29-43C4-863E-AD10398B3A8C");

        public static Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        
        [SqlPrimaryKey]
        public Guid Id { get; set; }
        
        [SqlReferenceManyToOne]
        public OneToManyEntity IdEntity1 { get; set; }

        public bool AreSame(object other)
        {
            throw new NotImplementedException();
        }
    }
}