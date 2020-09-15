using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Contest.Core.Component;
using Contest.Domain.Matchs;
using Contest.Domain.Players;
using SmartWay.Orm.Attributes;

namespace Contest.Domain.Games
{
    [Entity(NameInStore = "PHASE")]
    public class Phase : Entity, IPhase
    {
        #region Constructors

        public Phase()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _phaseTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IPhase>>>(() =>
                TeamPhaseRelationshipRepository.Find(_ => _.SecondItemInvolveId == Id)
                    .ToList());
            _gameStepList = new Lazy<IList<IGameStep>>(() =>
            {
                IList<IGameStep> result = EliminationStepRepository.Find(_ => _.PhaseId == Id)
                    .Union<IGameStep>(QualificationStepRepository.Find(_ => _.PhaseId == Id))
                    .ToList();
                foreach (var gameStep in result) RegisterHandler(gameStep);
                return result;
            });
        }

        #endregion

        #region Fields

        private Lazy<IList<IRelationship<ITeam, IPhase>>> _phaseTeamRelationshipList;
        private Lazy<IList<IGameStep>> _gameStepList;

        #endregion

        #region MEF Import

        [Import] private IRepository<QualificationStep, IQualificationStep> QualificationStepRepository { get; set; }

        [Import] private IRepository<EliminationStep, IEliminationStep> EliminationStepRepository { get; set; }

        [Import] private IRepository<TeamPhaseRelationship, IRelationship<ITeam, IPhase>> TeamPhaseRelationshipRepository { get; set; }

        [Import] private IRelationshipFactory<ITeam, IPhase> TeamPhaseRelationshipFactory { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///     Get identifier of current match.
        /// </summary>
        [Field(FieldName = "CONTEST_ID")]
        public Guid ContestId { get; private set; }

        /// <summary>
        ///     Get type of current game step.
        /// </summary>
        [Field(FieldName = "TYPE_OF")]
        public PhaseType Type { get; private set; }

        /// <summary>
        ///     Get team involved in current game step.
        /// </summary>
        public IList<IRelationship<ITeam, IPhase>> TeamPhaseList
        {
            get => _phaseTeamRelationshipList.Value;
        }

        /// <summary>
        ///     Get team involved in current game step.
        /// </summary>
        public IList<ITeam> TeamList
        {
            get { return TeamPhaseList.Select(_ => _.FirstItemInvolve).ToList(); }
            set
            {
                _phaseTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IPhase>>>(() =>
                    TeamPhaseRelationshipFactory.CreateFromFirstItemList(value, this));
            }
        }

        /// <summary>
        ///     Get all game step of current phase.
        /// </summary>
        public IList<IGameStep> GameStepList
        {
            get => _gameStepList.Value;
            set { _gameStepList = new Lazy<IList<IGameStep>>(() => value); }
        }

        /// <summary>
        ///     Get actual game step of current phase.
        /// </summary>
        public IGameStep ActualGameStep => GameStepList.Last();

        /// <summary>
        ///     Get boolean to know if current phase is started
        /// </summary>
        public bool IsStarted => ActualGameStep != null;

        /// <summary>
        ///     Get boolean to know if current game step is finished.
        /// </summary>
        public bool IsFinished
        {
            get { return GameStepList.Count(item => !item.IsFinished) == 0; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Build all game step and launch current phase
        /// </summary>
        public void LaunchNextStep()
        {
            //Ensure actual phase is Started
            if (!IsStarted) return;

            //Ensure actual game step is finished.
            if (!ActualGameStep.IsFinished)
                throw new NotSupportedException(
                    "Il est impossible de démarrer une nouvelle étape si l'étape actuelle n'est pas terminé.");

            if (ActualGameStep is QualificationStep)
                throw new NotSupportedException("La phase de qualification est terminée.");

            if (ActualGameStep is EliminationStep actual)
            {
                var nextStep = EliminationStep.DetermineNextEliminationStep(actual.Type);
                if (nextStep == null) throw new NotSupportedException("La phase éliminatoire est terminé.");

                //Compute team list for next step
                var qualifiedTeam = ActualGameStep.MatchList.Select(match => match.Winner).ToList();

                //Create and add new game step
                var newGameStep = EliminationStep.Create(this, qualifiedTeam, ActualGameStep.CurrentMatchSetting,
                    nextStep.Value);
                GameStepList.Add(newGameStep);
                newGameStep.BuildMatch();
                RegisterHandler(newGameStep);
                RaiseNextStepStartedEvent(NextStepStarted, newGameStep);
            }
        }

        private void RegisterHandler(IGameStep game)
        {
            game.GameStepEnded += sender =>
            {
                if (IsFinished) RaisePhaseEvent(PhaseEnded);
            };
        }

        private void BuildMatch()
        {
            foreach (var gameStep in GameStepList) gameStep.BuildMatch();
        }

        #endregion

        #region Event

        private void RaisePhaseEvent(PhaseEvent @event)
        {
            @event?.Invoke(this);
        }

        private void RaiseNextStepStartedEvent(NextStepStartedEvent @event, IGameStep newGameStep)
        {
            @event?.Invoke(this, newGameStep);
        }

        /// <summary>
        ///     This event occurs when current phase is ended
        /// </summary>
        public event PhaseEvent PhaseEnded;

        /// <summary>
        ///     This event occurs when current phase is ended
        /// </summary>
        public event NextStepStartedEvent NextStepStarted;

        #endregion

        #region Factory

        /// <summary>
        ///     Create a new instance (in memory and database) of <see cref="T:Business.Phase" /> with specified param
        /// </summary>
        /// <param name="teamList">A dictionnary of involved team for this phase. The usigned integer is used to build group.</param>
        /// <param name="contest">Contest linked to this phase</param>
        /// <param name="type">Type of phase</param>
        /// <returns>Match's instance</returns>
        private static Phase Create(IContest contest, PhaseType type, IList<ITeam> teamList)
        {
            if (contest == null) throw new ArgumentNullException(nameof(contest));

            var result = new Phase
            {
                ContestId = contest.Id,
                Type = type,
                GameStepList = new List<IGameStep>(),
                TeamList = teamList
            };

            return result;
        }

        /// <summary>
        ///     Create a new instance (in memory and database) of <see cref="T:Business.Phase" /> with specified param
        /// </summary>
        /// <param name="teamList">A dictionnary of involved team for this phase. The usigned integer is used to build group.</param>
        /// <param name="contest">Contest linked to this phase</param>
        /// <param name="setting">Set setting for new phase</param>
        /// <returns>Match's instance</returns>
        public static IPhase CreateMainEliminationPhase(IContest contest, IList<ITeam> teamList,
            IEliminationStepSetting setting)
        {
            return CreateEliminationPhase(contest, PhaseType.Main, teamList, setting);
        }

        /// <summary>
        ///     Create a new instance (in memory and database) of <see cref="T:Business.Phase" /> with specified param
        /// </summary>
        /// <param name="teamList">A dictionnary of involved team for this phase. The usigned integer is used to build group.</param>
        /// <param name="contest">Contest linked to this phase</param>
        /// <param name="setting">Set setting for new phase</param>
        /// <returns>Match's instance</returns>
        public static IPhase CreateConsolingEliminationPhase(IContest contest, IList<ITeam> teamList,
            IEliminationStepSetting setting)
        {
            return CreateEliminationPhase(contest, PhaseType.Consoling, teamList, setting);
        }

        /// <summary>
        ///     Create a new instance (in memory and database) of <see cref="T:Business.Phase" /> with specified param
        /// </summary>
        /// <param name="teamList">A dictionnary of involved team for this phase. The usigned integer is used to build group.</param>
        /// <param name="contest">Contest linked to this phase</param>
        /// <param name="type">Type of phase</param>
        /// <param name="setting">Set setting for new phase</param>
        /// <returns>Match's instance</returns>
        private static IPhase CreateEliminationPhase(IContest contest, PhaseType type, IList<ITeam> teamList,
            IEliminationStepSetting setting)
        {
            var result = Create(contest, type, teamList);

            //Create and add new game step
            var newEliminationStep =
                EliminationStep.Create(result, result.TeamList, setting.MatchSetting, setting.FirstStep);
            result.RegisterHandler(newEliminationStep);
            result.GameStepList.Add(newEliminationStep);

            result.BuildMatch();

            return result;
        }

        /// <summary>
        ///     Create a new instance (in memory and database) of <see cref="T:Business.Phase" /> with specified param
        /// </summary>
        /// <param name="teamList">A dictionnary of involved team for this phase. The usigned integer is used to build group.</param>
        /// <param name="contest">Contest linked to this phase</param>
        /// <param name="setting">Set setting for new phase</param>
        /// <returns>Match's instance</returns>
        public static IPhase CreateQualificationPhase(IContest contest, IList<ITeam> teamList,
            IQualificationStepSetting setting)
        {
            var result = Create(contest, PhaseType.Qualification, teamList);

            if (setting.CountGroup == 0)
                throw new ArgumentException("La phase qualificative doit au moins contenir un groupe.");

            var teamindex = 0;
            for (var i = 0; i < setting.CountGroup; i++)
            {
                var numberOfTeam = ComputeNumberOfTeam(setting, result, i);

                var groupTeamList = BuildTeamList(result, ref teamindex, numberOfTeam);

                CreateAndAddNewGameStep(setting, result, i, groupTeamList);
            }

            result.BuildMatch();

            return result;
        }

        private static int ComputeNumberOfTeam(IQualificationStepSetting setting, Phase result, int i)
        {
            var rest = result.TeamList.Count % setting.CountGroup;
            var numberOfTeam = result.TeamList.Count / setting.CountGroup + //Min number of team per group
                               (rest != 0 && i < rest
                                   ? 1
                                   : 0); //Add one if has rest of division and position qualification group is lower than rest
            return numberOfTeam;
        }

        private static List<ITeam> BuildTeamList(Phase result, ref int teamindex, int numberOfTeam)
        {
            var groupTeamList = new List<ITeam>();
            for (var j = 0; j < numberOfTeam && teamindex + j < result.TeamList.Count; j++)
                groupTeamList.Add(result.TeamList.ElementAt(teamindex + j));
            teamindex += numberOfTeam;
            return groupTeamList;
        }

        private static void CreateAndAddNewGameStep(IQualificationStepSetting setting, Phase result, int i,
            List<ITeam> groupTeamList)
        {
            var newQualificationStep = QualificationStep.Create(result, groupTeamList, setting, i + 1);
            result.RegisterHandler(newQualificationStep);
            result.GameStepList.Add(newQualificationStep);
        }

        public IList<ITeam> GetDirectQualifiedTeam()
        {
            return GameStepList.SelectMany(_ => _.GetDirectQualifiedTeam()).ToList();
        }

        public IList<IMatch> GetAllMatch()
        {
            return GameStepList.SelectMany(_ => _.MatchList).ToList();
        }

        #endregion
    }
}