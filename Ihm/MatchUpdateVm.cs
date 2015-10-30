using System.Collections.Generic;
using System.Linq;
using Contest.Business;
using Contest.Core.Windows.Commands;

namespace Contest.Ihm
{
    public class MatchUpdateVm : MatchBaseVm
    {
        public MatchUpdateVm(IMatch match, IList<IField> fieldList)
            : base(match)
        {
            Start = new RelayCommand(
                delegate { CurrentMatch.Start(fieldList.First(item => !item.IsAllocated)); },
                delegate { return fieldList.Count(item => !item.IsAllocated) != 0
                                && CurrentMatch.Team1.CurrentMatch == null
                                && CurrentMatch.Team2.CurrentMatch == null; });
            Update = new RelayCommand(
                delegate { CurrentMatch.UpdateScore(TeamScore1.GetValueOrDefault(), TeamScore2.GetValueOrDefault()); },
                delegate { return !CurrentMatch.IsClose; });

            SetScore = new RelayCommand(
                delegate { CurrentMatch.SetResult(TeamScore1.GetValueOrDefault(), TeamScore2.GetValueOrDefault()); },
                delegate { return true; });
        }

        public RelayCommand Start { get; set; }
        public RelayCommand Update { get; set; }
        public RelayCommand SetScore { get; set; }

        public override bool ShowScoreBlock
        {
            get { return false; }
        }

        public override bool ShowUpdateScoreBox
        {
            get { return IsInProgress || IsEnded; }
        }

        public override bool ShowVs
        {
            get { return !IsStarted; }
        }
    }
}
