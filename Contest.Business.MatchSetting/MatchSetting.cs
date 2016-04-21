using System;
using System.Runtime.Serialization;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    /// <summary>
    /// Represent a MatchSetting beween two team
    /// </summary>
    [DataContract(Name = "MATCH_SETTING")]
    public sealed class MatchSetting : Identifiable<MatchSetting>, IMatchSetting
    {
        #region Field

        private ushort _matchPoint;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize a new instance of setting with default param
        /// </summary>
        public MatchSetting()
            : this(false, 13, 1, 0, 0)
        { }

        /// <summary>
        /// Instance a new MatchSetting with specified param
        /// </summary>
        /// <param name="canBeDuce">Set to true if result of a match can be duce</param>
        /// <param name="matchPoint">Set match point to end game</param>
        /// <param name="pointForWin">Set number of point earn when match is winned</param>
        /// <param name="pointForLoose">Set number of point earn when match is loosed</param>
        /// <param name="pointForDuce">Set number of point earn when match haven't winner</param>
        public MatchSetting(bool canBeDuce, ushort matchPoint, ushort pointForWin, ushort pointForLoose, ushort pointForDuce)
            : this(canBeDuce, pointForWin, pointForLoose, pointForDuce)
        {
            EndBy = EndTypeConstaint.Point;
            MatchPoint = matchPoint;
        }

        private MatchSetting(bool canBeDuce, ushort pointForWin, ushort pointForLoose, ushort pointForDuce)
        {
            EndBy = EndTypeConstaint.None;
            CanBeDuce = canBeDuce;
            PointForWin = pointForWin;
            PointForLoose = pointForLoose;
            PointForDuce = pointForDuce;
        }

        #endregion

        #region Properties

        [SqlField(Name = "TYPE_OF_END")]
        public EndTypeConstaint EndBy
        {
            get { return CurrentEndType.EndBy; }
            set
            {
                switch (value)
                {
                    case EndTypeConstaint.None:
                        CurrentEndType = new EndByNoSpecifiq(this);
                        break;
                    case EndTypeConstaint.Point:
                        CurrentEndType = new EndByPoint(this);
                        break;
                    default:
                        throw new InvalidOperationException(string.Format("Unknown EndType. EndType:{0}", value));
                }
            }
        }

        internal EndByBase CurrentEndType { get; set; }

        /// <summary>
        /// Get if match can be duce
        /// </summary>
        [SqlField(Name = "CAN_BE_DUCE")]
        public bool CanBeDuce { get; set; }

        /// <summary>
        /// Get number of point need to end a match
        /// </summary>
        [SqlField(Name = "MATCH_POINT")]
        public ushort MatchPoint
        {
            get
            {
                if (!CurrentEndType.CanUseMatchPoint) throw new NotSupportedException(string.Format("Type of end doesn't support Match point. Type:{0}", CurrentEndType));
                return _matchPoint;
            }
            set
            {
                if (!CurrentEndType.CanUseMatchPoint) throw new NotSupportedException(string.Format("Type of end doesn't support Match point. Type:{0}", CurrentEndType));
                _matchPoint = value;
            }
        }

        /// <summary>
        /// Get point earn when match is win
        /// </summary>
        [SqlField(Name = "WIN_POINT")]
        public ushort PointForWin { get; set; }

        /// <summary>
        /// Get point earn when match is loosed
        /// </summary>
        [SqlField(Name = "LOOSE_POINT")]
        public ushort PointForLoose { get; set; }

        /// <summary>
        /// Get point earn when match have no winner
        /// </summary>
        [SqlField(Name = "DUCE_POINT")]
        public ushort PointForDuce { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Save current object into specfied repository
        /// </summary>
        /// <param name="repo">Target repository</param>
        public void Save(IRepository<IMatchSetting> repo)
        {
            repo.InsertOrUpdate(this);
        }

        /// <summary>
        /// Ensure match can be finished in according with current setting
        /// </summary>
        /// <param name="teamScore1">Team score 1</param>
        /// <param name="teamScore2">Team score 2</param>
        /// <param name="message">When score isnot valid, specified a message explain why</param>
        public bool IsValidToFinishedMatch(ushort teamScore1, ushort teamScore2, out string message)
        {
            if (!CurrentEndType.IsValidToFinishedMatch(teamScore1, teamScore2, out message)) return false;
            if (!CanBeDuce && teamScore1 == teamScore2)
            {
                message = string.Format("Les deux équipes ne peuvent pas être à égalité. Résultat équipe 1:{0}. Résultat équipe 2:{1}", teamScore1, teamScore2);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Ensure mscore is valid in according with current setting
        /// </summary>
        /// <param name="teamScore"></param>
        /// <param name="message">When score isnot valid, specified a message explain why</param>
        public bool IsValidScore(ushort teamScore, out string message)
        {
            return CurrentEndType.IsValidScore(teamScore, out message);
        }

        #endregion

        public void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.InsertOrUpdate<IMatchSetting>(this);
        }

        public void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.Delete<IMatchSetting>(this);

        }
    }
}
