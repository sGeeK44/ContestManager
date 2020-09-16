using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Contest.Core.Component;
using Contest.Core.Windows.Commands;
using Contest.Domain.Matchs;
using Contest.Domain.Settings;
using Contest.Service;

namespace Contest.Ihm
{
    public class MatchUpdateVm : MatchBaseVm
    {
        private readonly IList<IField> _fieldList;

        [Import] private IUnitOfWorks UnitOfWorks { get; set; }

        public MatchUpdateVm(IMatch match, IList<IField> fieldList)
            : base(match)
        {
            _fieldList = fieldList;
            FlippingContainer.Instance.ComposeParts(this);
        }

        public RelayCommand Start => new RelayCommand(
            obj =>
            {
                CurrentMatch.Start(_fieldList.First(item => !item.IsAllocated));
                UnitOfWorks.Save(CurrentMatch);
            },
            obj =>
            {
                return _fieldList.Count(item => !item.IsAllocated) != 0
                       && CurrentMatch.Team1.CurrentMatch == null
                       && CurrentMatch.Team2.CurrentMatch == null;
            });

        public RelayCommand Update => new RelayCommand(
            obj =>
            {
                CurrentMatch.UpdateScore(TeamScore1.GetValueOrDefault(), TeamScore2.GetValueOrDefault());
                UnitOfWorks.Save(CurrentMatch);
            },
            obj =>
            {
                return !CurrentMatch.IsClose;
            });

        public RelayCommand SetScore => new RelayCommand(
            obj =>
            {
                CurrentMatch.SetResult(TeamScore1.GetValueOrDefault(), TeamScore2.GetValueOrDefault());
                UnitOfWorks.Save(CurrentMatch);
            });

        public override bool ShowScoreBlock => false;

        public override bool ShowUpdateScoreBox => IsInProgress || IsEnded;

        public override bool ShowVs => !IsStarted;
    }
}
