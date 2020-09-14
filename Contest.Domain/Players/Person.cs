using System;
using System.ComponentModel.Composition;
using Contest.Core.Component;
using SmartWay.Orm.Attributes;
using SmartWay.Orm.Entity.References;

namespace Contest.Domain.Players
{
    [Serializable]
    [Entity(NameInStore = "PERSON")]
    public class Person : Entity, IPerson
    {
        private ReferenceHolder<ITeam, Guid> _affectedTeam;

        public Person()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _affectedTeam = new ReferenceHolder<ITeam, Guid>(TeamRepository);
        }

        #region MEF Import

        [Import] private IRepository<Team, ITeam> TeamRepository { get; set; }

        #endregion

        [Field(FieldName = "LAST_NAME")] public string LastName { get; set; }

        [Field(FieldName = "FIRST_NAME")] public string FirstName { get; set; }

        [Field(FieldName = "ALIAS")] public string Alias { get; set; }

        [Field(FieldName = "MAIL")] public string Mail { get; set; }

        [Field(FieldName = "CAN_MAILING")] public bool CanMailing { get; set; }

        [Field(FieldName = "IS_MEMBER")] public bool IsMember { get; set; }

        [Field(FieldName = "AFFECTED_TEAM")]
        public Guid AffectedTeamId
        {
            get => _affectedTeam.Id;
            set => _affectedTeam.Id = value;
        }

        public ITeam AffectedTeam
        {
            get => _affectedTeam.Object;
            set => _affectedTeam.Object = value;
        }

        public void SetIndentity(string lastName, string firstName, string alias)
        {
            LastName = lastName;
            FirstName = firstName;
            Alias = alias;
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
    }
}