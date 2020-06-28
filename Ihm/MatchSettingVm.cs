using System;
using Contest.Core.Windows.Mvvm;
using Contest.Domain.Matchs;

namespace Contest.Ihm
{
    public class MatchSettingVm : ViewModel
    {
        private bool _byPoint;
        private bool _byTime;
        private bool _byBoth;
        private ushort? _matchPoint;
        private TimeSpan? _matchElapse;
        private bool _matchNullIsPossible;
        private ushort _matchPointWin;
        private ushort _matchPointDuce;
        private ushort _matchPointLoose;

        public bool ByPoint
        {
            get => _byPoint;
            set => Set(ref _byPoint, value);
        }

        public bool ByTime
        {
            get => _byTime;
            set => Set(ref _byTime, value);
        }

        public bool ByBoth
        {
            get => _byBoth;
            set => Set(ref _byBoth, value);
        }

        public ushort? MatchPoint
        {
            get => _matchPoint;
            set => Set(ref _matchPoint, value);
        }

        public TimeSpan? MatchElapse
        {
            get => _matchElapse;
            set => Set(ref _matchElapse, value);
        }

        public bool MatchNullIsPossible
        {
            get => _matchNullIsPossible;
            set => Set(ref _matchNullIsPossible, value);
        }

        public ushort MatchPointWin
        {
            get => _matchPointWin;
            set => Set(ref _matchPointWin, value);
        }

        public ushort MatchPointDuce
        {
            get => _matchPointDuce;
            set => Set(ref _matchPointDuce, value);
        }

        public ushort MatchPointLoose
        {
            get => _matchPointLoose;
            set => Set(ref _matchPointLoose, value);
        }

        public MatchSettingVm()
        {
            MatchPointWin = 3;
            MatchPointDuce = 1;
            MatchPointLoose = 0;
            ByPoint = true;
            MatchPoint = 13;
            MatchElapse = new TimeSpan(0, 0, 0);
        }

        public MatchSettingVm(IMatchSetting matchSetting)
        {
            if (matchSetting == null) throw new ArgumentNullException(nameof(matchSetting));
            ByPoint = matchSetting.EndBy == EndTypeConstaint.Point;
            if (ByPoint)
            {
                MatchPoint = matchSetting.MatchPoint;
            }
            MatchNullIsPossible = matchSetting.CanBeDuce;
            MatchPointWin = matchSetting.PointForWin;
            MatchPointLoose = matchSetting.PointForLoose;
            MatchPointDuce = matchSetting.PointForDuce;
        }

        public MatchSetting ToMatchSetting()
        {
            if (ByPoint)
            {
                if (MatchPoint == null) throw new InvalidProgramException("MatchPoint is null.");
                return new MatchSetting(MatchNullIsPossible, MatchPoint.Value, MatchPointWin, MatchPointLoose, MatchPointDuce);
            }

            throw new InvalidProgramException("No type of end math selected.");
        }

        public void UpdateFromModel(IMatchSetting matchToUpdate)
        {
            if (matchToUpdate == null) return;
            matchToUpdate.CanBeDuce = MatchNullIsPossible;
            matchToUpdate.PointForWin = MatchPointWin;
            matchToUpdate.PointForDuce = MatchPointDuce;
            matchToUpdate.PointForLoose = MatchPointLoose;
            if (ByPoint)
            {
                matchToUpdate.EndBy = EndTypeConstaint.Point;
                if (MatchPoint == null) throw new InvalidProgramException("MatchPoint is null.");
                matchToUpdate.MatchPoint = MatchPoint.Value;
            }
        }
    }
}