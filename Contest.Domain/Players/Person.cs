using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Contest.Core.Component;
using Contest.Domain.Games;
using SmartWay.Orm.Attributes;

namespace Contest.Domain.Players
{
    [Serializable]
    [Entity(NameInStore = "PERSON")]
    public class Person : Entity, IPerson
    {
        private Lazy<IList<IRelationship<ITeam, IPerson>>> _teamList;

        public Person()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _teamList = new Lazy<IList<IRelationship<ITeam, IPerson>>>(() => TeamPersonRelationshipRepository.Find(_ => _.SecondItemInvolveId == Id).ToList());
        }

        #region MEF Import

        [Import] private IRepository<TeamPersonRelationship, IRelationship<ITeam, IPerson>> TeamPersonRelationshipRepository { get; set; }

        #endregion

        [Field(FieldName = "LAST_NAME")] public string LastName { get; set; }

        [Field(FieldName = "FIRST_NAME")] public string FirstName { get; set; }

        [Field(FieldName = "ALIAS")] public string Alias { get; set; }

        [Field(FieldName = "MAIL")] public string Mail { get; set; }

        [Field(FieldName = "CAN_MAILING")] public bool CanMailing { get; set; }

        [Field(FieldName = "IS_MEMBER")] public bool IsMember { get; set; }

        public IList<IRelationship<ITeam, IPerson>> TeamList
        {
            get => _teamList.Value;
            set { _teamList = new Lazy<IList<IRelationship<ITeam, IPerson>>>(() => value); }
        }

        public void SetIndentity(string lastName, string firstName, string alias)
        {
            LastName = lastName;
            FirstName = firstName;
            Alias = alias;
        }

        public ITeam GetTeam(IContest contest)
        {
            var association = TeamList.FirstOrDefault(_ => _.FirstItemInvolve.Contest.Id == contest.Id);
            return association?.FirstItemInvolve;
        }

        public static IPerson Create(string lastName, string firstName, string alias)
        {
            var result = new Person
            {
                LastName = lastName,
                FirstName = firstName,
                Alias = alias
            };
            return result;
        }
    }
}