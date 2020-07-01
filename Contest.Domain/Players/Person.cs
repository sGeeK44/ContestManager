using System;
using System.ComponentModel.Composition;
using Contest.Core.Component;
using SmartWay.Orm.Attributes;

namespace Contest.Domain.Players
{
    [Serializable]
    [Entity(NameInStore = "PERSON")]
    public class Person : Entity, IPerson
    {
        [NonSerialized] private Lazy<ITeam> _affectedTeam;

        [NonSerialized] private Guid _affectedTeamId;

        private Person()
        {
            FlippingContainer.Instance.ComposeParts(this);
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
            get => _affectedTeamId;
            set => _affectedTeamId = value;
        }

        public ITeam AffectedTeam
        {
            get
            {
                if (_affectedTeam == null)
                    _affectedTeam = new Lazy<ITeam>(() =>
                        TeamRepository.FirstOrDefault(_ => _.Id == AffectedTeamId));
                return _affectedTeam.Value;
            }
            set
            {
                _affectedTeam = new Lazy<ITeam>(() => value);
                AffectedTeamId = value?.Id ?? Guid.Empty;
            }
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