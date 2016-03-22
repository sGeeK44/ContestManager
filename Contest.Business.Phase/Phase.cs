using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Serialization;
using Contest.Core.Component;
using Contest.Core.Repository;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [DataContract(Name = "PHASE")]
    public class Phase : Identifiable<Phase>, IPhase
    {
        #region Fields

        private Lazy<IList<IRelationship<ITeam, IPhase>>> _phaseTeamRelationshipList;
        private Lazy<IList<IGameStep>> _gameStepList;

        #endregion

        #region MEF Import

        [Import]
        private IRepository<IQualificationStep> QualificationStepRepository { get; set; }

        [Import]
        private IRepository<IEliminationStep> EliminationStepRepository { get; set; }

        [Import]
        private IRepository<IRelationship<ITeam, IPhase>> TeamPhaseRelationshipRepository { get; set; }

        [Import]
        private IRelationshipFactory<ITeam, IPhase> TeamPhaseRelationshipFactory { get; set; }

        #endregion

        #region Constructors

        protected Phase()
        {
            FlippingContainer.Instance.ComposeParts(this);
            _phaseTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IPhase>>>(() => TeamPhaseRelationshipRepository.Find(_ => _.SecondItemInvolveId == Id));
            _gameStepList = new Lazy<IList<IGameStep>>(() => {
                IList<IGameStep> result = EliminationStepRepository.Find(_ => _.PhaseId == Id)
                                                                   .Cast<IGameStep>()
                                                                   .Union(QualificationStepRepository.Find(_ => _.PhaseId == Id))
                                                                   .ToList();
                                                                foreach (var gameStep in result)
                                                                {
                                                                    RegisterHandler(gameStep);
                                                                }
                                                                return result;});
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get identifier of current match.
        /// </summary>
        [DataMember(Name = "CONTEST_ID")]
        public Guid ContestId { get; private set; }

        /// <summary>
        /// Get type of current game step.
        /// </summary>
        [DataMember(Name = "TYPE_OF")]
        public PhaseType Type { get; private set; }

        /// <summary>
        /// Get team involved in current game step.
        /// </summary>
        public IList<ITeam> TeamList
        {
            get { return _phaseTeamRelationshipList.Value.Select(_ => _.FirstItemInvolve).ToList(); }
            set
            {
                _phaseTeamRelationshipList = new Lazy<IList<IRelationship<ITeam, IPhase>>>(() => TeamPhaseRelationshipFactory.CreateFromFirstItemList(value, this));
            }
        }

        /// <summary>
        /// Get all game step of current phase.
        /// </summary>
        public IList<IGameStep> GameStepList
        {
            get { return _gameStepList.Value; }
            set { _gameStepList = new Lazy<IList<IGameStep>>(() => value); }
        }

        /// <summary>
        /// Get actual game step of current phase.
        /// </summary>
        public IGameStep ActualGameStep { get { return GameStepList.Last(); } }

        /// <summary>
        /// Get boolean to know if current phase is started
        /// </summary>
        public bool IsStarted { get { return ActualGameStep != null; } }

        /// <summary>
        /// Get boolean to know if current game step is finished.
        /// </summary>
        public bool IsFinished
        {
            get { return GameStepList.Count(item => !item.IsFinished) == 0; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public virtual void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.InsertOrUpdate<IPhase>(this);
            foreach (var gameStep in GameStepList)
            {
                gameStep.PrepareCommit(unitOfWorks);
            }
            if (_phaseTeamRelationshipList.Value != null)
            foreach (var relation in _phaseTeamRelationshipList.Value)
            {
                relation.PrepareCommit(unitOfWorks);
            }
        }

        /// <summary>
        /// Do all delete into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Build all game step and launch current phase
        /// </summary>
        public void LaunchNextStep()
        {
            //Ensure actual phase is Started
            if (!IsStarted) return;

            //Ensure actual game step is finished.
            if (!ActualGameStep.IsFinished) throw new NotSupportedException("Il est impossible de démarrer une nouvelle étape si l'étape actuelle n'est pas terminé.");

            if (ActualGameStep is QualificationStep) throw new NotSupportedException("La phase de qualification est terminée.");

            if (ActualGameStep is EliminationStep)
            {
                //Ensure there is a valid next step for current phase
                var actual = (EliminationStep)ActualGameStep;
                var nextStep = EliminationStep.DetermineNextEliminationStep(actual.Type);
                if (nextStep == null) throw new NotSupportedException("La phase éliminatoire est terminé.");

                //Compute team list for next step
                var qualifiedTeam = ActualGameStep.MatchList.Select(match => match.Winner).ToList();

                //Create and add new game step
                var newGameStep = EliminationStep.Create(this, qualifiedTeam, ActualGameStep.CurrentMatchSetting, nextStep.Value);
                GameStepList.Add(newGameStep);
                newGameStep.BuildMatch();
                RegisterHandler(newGameStep);
                RaiseNextStepStartedEvent(NextStepStarted, newGameStep);
            }
        }

        private void RegisterHandler(IGameStep game)
        {
            game.GameStepEnded += sender => { if (IsFinished) RaisePhaseEvent(PhaseEnded); };
        }

        private void BuildMatch()
        {
            foreach (var gameStep in GameStepList)
            {
                gameStep.BuildMatch();
            }
        }

        #endregion

        #region Event

        private void RaisePhaseEvent(PhaseEvent @event)
        {
            if (@event != null) @event(this);
        }

        private void RaiseNextStepStartedEvent(NextStepStartedEvent @event, IGameStep newGameStep)
        {
            if (@event != null) @event(this, newGameStep);
        }

        /// <summary>
        /// This event occurs when current phase is ended
        /// </summary>
        public event PhaseEvent PhaseEnded;

        /// <summary>
        /// This event occurs when current phase is ended
        /// </summary>
        public event NextStepStartedEvent NextStepStarted;

        #endregion

        #region Factory

        /// <summary>
        /// Create a new instance (in memory and database) of <see cref="T:Business.Phase"/> with specified param
        /// </summary>
        /// <param name="teamList">A dictionnary of involved team for this phase. The usigned integer is used to build group.</param>
        /// <param name="contest">Contest linked to this phase</param>
        /// <param name="type">Type of phase</param>
        /// <param name="setting">Set setting for new phase</param>
        /// <returns>Match's instance</returns>
        private static Phase Create(IContest contest, PhaseType type, IList<ITeam> teamList, IStepSetting setting)
        {
            if (contest == null) throw new ArgumentNullException("contest");
            if (setting == null) throw new ArgumentNullException("setting");
            
            var result = new Phase
            {
                Id = Guid.NewGuid(),
                ContestId = contest.Id,
                Type = type,
                GameStepList = new List<IGameStep>(),
                TeamList = teamList,
            };

            return result;
        }

        /// <summary>
        /// Create a new instance (in memory and database) of <see cref="T:Business.Phase"/> with specified param
        /// </summary>
        /// <param name="teamList">A dictionnary of involved team for this phase. The usigned integer is used to build group.</param>
        /// <param name="contest">Contest linked to this phase</param>
        /// <param name="setting">Set setting for new phase</param>
        /// <returns>Match's instance</returns>
        public static IPhase CreateMainEliminationPhase(IContest contest, IList<ITeam> teamList, IEliminationStepSetting setting)
        {
            return CreateEliminationPhase(contest, PhaseType.Main, teamList, setting);
        }

        /// <summary>
        /// Create a new instance (in memory and database) of <see cref="T:Business.Phase"/> with specified param
        /// </summary>
        /// <param name="teamList">A dictionnary of involved team for this phase. The usigned integer is used to build group.</param>
        /// <param name="contest">Contest linked to this phase</param>
        /// <param name="type">Type of phase</param>
        /// <param name="setting">Set setting for new phase</param>
        /// <returns>Match's instance</returns>
        public static IPhase CreateConsolingEliminationPhase(IContest contest, IList<ITeam> teamList, IEliminationStepSetting setting)
        {
            return CreateEliminationPhase(contest, PhaseType.Consoling, teamList, setting);
        }

        /// <summary>
        /// Create a new instance (in memory and database) of <see cref="T:Business.Phase"/> with specified param
        /// </summary>
        /// <param name="teamList">A dictionnary of involved team for this phase. The usigned integer is used to build group.</param>
        /// <param name="contest">Contest linked to this phase</param>
        /// <param name="type">Type of phase</param>
        /// <param name="setting">Set setting for new phase</param>
        /// <returns>Match's instance</returns>
        private static IPhase CreateEliminationPhase(IContest contest, PhaseType type, IList<ITeam> teamList, IEliminationStepSetting setting)
        {
            var result = Create(contest, type, teamList, setting);

            //Create and add new game step
            var newEliminationStep = EliminationStep.Create(result, result.TeamList, setting.MatchSetting, setting.FirstStep);
            result.RegisterHandler(newEliminationStep);
            result.GameStepList.Add(newEliminationStep);

            result.BuildMatch();

            return result;
        }

        /// <summary>
        /// Create a new instance (in memory and database) of <see cref="T:Business.Phase"/> with specified param
        /// </summary>
        /// <param name="teamList">A dictionnary of involved team for this phase. The usigned integer is used to build group.</param>
        /// <param name="contest">Contest linked to this phase</param>
        /// <param name="type">Type of phase</param>
        /// <param name="setting">Set setting for new phase</param>
        /// <returns>Match's instance</returns>
        public static IPhase CreateQualificationPhase(IContest contest, IList<ITeam> teamList, IQualificationStepSetting setting)
        {
            var result = Create(contest, PhaseType.Qualification, teamList, setting);
            
            var teamindex = 0;
            for (var i = 0; i < setting.CountGroup; i++)
            {
                int numberOfTeam = ComputeNumberOfTeam(setting, result, i);
                
                List<ITeam> groupTeamList = BuildTeamList(result, ref teamindex, numberOfTeam);

                CreateAndAddNewGameStep(setting, result, i, groupTeamList);
            }

            result.BuildMatch();

            return result;
        }

        private static int ComputeNumberOfTeam(IQualificationStepSetting setting, Phase result, int i)
        {
            var rest = result.TeamList.Count % setting.CountGroup;
            var numberOfTeam = result.TeamList.Count / setting.CountGroup + //Min number of team per group
                                (rest != 0 && i < rest ? 1 : 0); //Add one if has rest of division and position qualification group is lower than rest
            return numberOfTeam;
        }

        private static List<ITeam> BuildTeamList(Phase result, ref int teamindex, int numberOfTeam)
        {
            var groupTeamList = new List<ITeam>();
            for (var j = 0; j < numberOfTeam && teamindex + j < result.TeamList.Count; j++) groupTeamList.Add(result.TeamList.ElementAt(teamindex + j));
            teamindex += numberOfTeam;
            return groupTeamList;
        }

        private static void CreateAndAddNewGameStep(IQualificationStepSetting setting, Phase result, int i, List<ITeam> groupTeamList)
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
