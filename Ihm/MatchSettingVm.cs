using System;
using Contest.Business;
using Contest.Core.Windows.Mvvm;

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
            get { return _byPoint; }
            set { Set(ref _byPoint, value); }
        }

        public bool ByTime
        {
            get { return _byTime; }
            set { Set(ref _byTime, value); }
        }

        public bool ByBoth
        {
            get { return _byBoth; }
            set { Set(ref _byBoth, value); }
        }

        public ushort? MatchPoint
        {
            get { return _matchPoint; }
            set { Set(ref _matchPoint, value); }
        }

        public TimeSpan? MatchElapse
        {
            get { return _matchElapse; }
            set { Set(ref _matchElapse, value); }
        }

        public bool MatchNullIsPossible
        {
            get { return _matchNullIsPossible; }
            set { Set(ref _matchNullIsPossible, value); }
        }

        public ushort MatchPointWin
        {
            get { return _matchPointWin; }
            set { Set(ref _matchPointWin, value); }
        }

        public ushort MatchPointDuce
        {
            get { return _matchPointDuce; }
            set { Set(ref _matchPointDuce, value); }
        }

        public ushort MatchPointLoose
        {
            get { return _matchPointLoose; }
            set { Set(ref _matchPointLoose, value); }
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
            if (matchSetting == null) throw new ArgumentNullException("matchSetting");
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