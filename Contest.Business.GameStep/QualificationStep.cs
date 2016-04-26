using System;
using System.Collections.Generic;
using System.Linq;
using Contest.Core.DataStore.Sql.Attributes;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [SqlEntity(Name = "QUALIFICATION_STEP")]
    public class QualificationStep : GameStep, IQualificationStep
    {
        #region Constantes

        public const ushort MATCH_POINT = 13;

        #endregion

        #region Constructors

        protected QualificationStep() { }

        protected QualificationStep(IPhase phase, IList<ITeam> teamList, IMatchSetting matchSetting)
            : base(phase, teamList, matchSetting)
        { }

        #endregion

        #region Properties

        public override string Name
        {
            get { return "Groupe " + Number; }
        }

        /// <summary>
        /// Return next step, null if no futher step
        /// </summary>
        public override EliminationType? NextStep { get { return null; } }

        /// <summary>
        /// Get number of qualification group
        /// </summary>
        [SqlField(Name = "NUMBER")]
        public int Number { get; set; }

        /// <summary>
        /// Get boolean to know if match in current qualification step is played with revenge.
        /// </summary>
        [SqlField(Name = "WITH_REVENCHE")]
        public bool MatchWithRevenge { get; private set; }

        /// <summary>
        /// Get number of qualified team for current game step
        /// </summary>
        [SqlField(Name = "NUMBER_QUALIFIED")]
        public ushort NumberOfQualifiedTeam { get; private set; }

        /// <summary>
        /// Compute actual rank of current game step
        /// </summary>
        public IList<ITeam> Rank
        {
            get
            {
                return SortRank(TeamList, MatchList);
            }
        }

        #endregion

        #region Methods

        public override void BuildMatch()
        {
            IList<ITeam> list1 = new List<ITeam>(TeamList);
            IList<ITeam> list2 = new List<ITeam>(TeamList);
            MatchList.Clear();
            foreach (var team1 in list1)
            {
                foreach (var team2 in list2)
                {
                    if (team1 == team2
                     || MatchList.Count(item => item.Team1 == team1 && item.Team2 == team2
                                            || item.Team1 == team2 && item.Team2 == team1) != 0) continue;
                    
                    CreateMatch(team1, team2);

                    if (!MatchWithRevenge) continue;
                    CreateMatch(team2, team1);
                }
            }
        }

        public static IList<ITeam> SortRank(IList<ITeam> teamList, IList<IMatch> associatedMatch)
        {
            var result = new Dictionary<ITeam, int[]>();
            foreach (var team in teamList)
            {
                var currentTeam = team;
                IList<IMatch> teamMatchList =
                    associatedMatch.Where(item => item.IsFinished && (item.Team1 == currentTeam || item.Team2 == currentTeam))
                        .ToList();
                result.Add(currentTeam, new[]
                {
                    teamMatchList.Count == 0
                        ? 0
                        : teamMatchList.Count(item => item.Winner == team), //Compute number of victory
                    teamMatchList.Count == 0
                        ? 0
                        : teamMatchList.Sum(match => match.Team1 == currentTeam ? match.TeamScore1 : match.TeamScore2), //Compute marked point
                    teamMatchList.Count == 0
                        ? 0
                        : teamMatchList.Sum(match => match.Team1 == currentTeam ? match.TeamScore2 : match.TeamScore1) //Compute take point
                });
            }
            result = result.OrderByDescending(item => item.Value[0])
                           .ThenByDescending(item => item.Value[1])
                           .ThenBy(item => item.Value[2])
                           .ToDictionary(item => item.Key, item => item.Value);
            return result.Select(item => item.Key).ToList();
        }

        public override IList<ITeam> GetDirectQualifiedTeam()
        {
            return Rank.Take(NumberOfQualifiedTeam).ToList();
        }

        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public override void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.InsertOrUpdate<IQualificationStep>(this);
            base.PrepareCommit(unitOfWorks);
        }

        #endregion

        #region Factory

        /// <summary>
        /// Create a new instance (in memory and database) of <see cref="T:Business.QualificationStep"/> with specified param
        /// </summary>
        /// <param name="phase">Phase linked to this new step</param>
        /// <param name="teamList">Team involved in this new step</param>
        /// <param name="setting">Set setting for new qualification step</param>
        /// <param name="number">Number of qualification group</param>
        /// <returns>QualificationStep's instance</returns>
        public static IGameStep Create(IPhase phase, List<ITeam> teamList, IQualificationStepSetting setting, int number)
        {
            if (setting == null) throw new ArgumentNullException("setting");
            if (teamList.Count < setting.MinTeamRegister) throw new ArgumentException(string.Format("Le nombre d'équipe enregistré est insuffisant. Minimum:{0}. Fourni:{1}.", setting.MinTeamRegister, teamList.Count), "teamList");
            return new QualificationStep(phase, teamList, setting.MatchSetting)
                {
                    Number = number,
                    MatchWithRevenge = setting.MatchWithRevenche,
                    NumberOfQualifiedTeam = setting.CountTeamQualified
                };
        }

        #endregion
    }
}
