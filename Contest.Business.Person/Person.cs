﻿using System;
using System.ComponentModel.Composition;
using Contest.Core.Component;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [Serializable]
    [SqlEntity(Name = "PERSON")]
    public class Person : Identifiable<Person>, IPerson
    {
        [NonSerialized]
        private Lazy<ITeam> _affectedTeam;
        [NonSerialized]
        private Guid _affectedTeamId;

        #region MEF Import

        [Import]
        private IRepository<ITeam> TeamRepository { get; set; }

        #endregion

        private Person()
        {
            FlippingContainer.Instance.ComposeParts(this);
        }

        [SqlField(Name = "LAST_NAME")]
        public string LastName { get; set; }
        [SqlField(Name = "FIRST_NAME")]
        public string FirstName { get; set; }
        [SqlField(Name = "ALIAS")]
        public string Alias { get; set; }
        [SqlField(Name = "MAIL")]
        public string Mail { get; set; }
        [SqlField(Name = "CAN_MAILING")]
        public bool CanMailing { get; set; }
        [SqlField(Name = "IS_MEMBER")]
        public bool IsMember { get; set; }
        [SqlField(Name = "AFFECTED_TEAM")]
        public Guid AffectedTeamId
        {
            get { return _affectedTeamId; }
            set { _affectedTeamId = value; }
        }

        public ITeam AffectedTeam
        {
            get
            {
                if (_affectedTeam == null) _affectedTeam = new Lazy<ITeam>(() => TeamRepository.FirstOrDefault(_ => _.Id == AffectedTeamId));
                return _affectedTeam.Value;
            }
            set
            {
                _affectedTeam = new Lazy<ITeam>(() => value);
                AffectedTeamId = value != null ? value.Id : Guid.Empty;
            }
        }

        public static IPerson Create(string lastName, string firstName, string alias)
        {
            var result = new Person
            {
                Id = Guid.NewGuid(),
                LastName = lastName,
                FirstName = firstName,
                Alias = alias
            };
            return result;
        }

        public void SetIndentity(string lastName, string firstName, string alias)
        {
            LastName = lastName;
            FirstName = firstName;
            Alias = alias;
        }

        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public virtual void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.InsertOrUpdate<IPerson>(this);
        }
        
        /// <summary>
        /// Do all delete into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.Delete<IPerson>(this);
        }
    }
}
