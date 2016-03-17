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
                _phaseTeamRelationshipList =
                    new Lazy<IList<IRelationship<ITeam, IPhase>>>(
                        () =>
                            new List<IRelationship<ITeam, IPhase>>(value != null
                                ? value.Select(_ => TeamPhaseRelationshipFactory.Create(_, this))
                                : new List<IRelationship<ITeam, IPhase>>()));
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
        public static IPhase Create(IContest contest, PhaseType type, IList<ITeam> teamList, IStepSetting setting)
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

            switch (result.Type)
            {
                case PhaseType.Qualification:
                    var qualificationSetting = setting as QualificationStepSetting;
                    if (qualificationSetting == null) throw new ArgumentException("Setting can not be null or a differnet type of QualificationStepSetting", "setting");
                    var teamindex = 0;
                    for (var i = 0; i < qualificationSetting.CountGroup; i++)
                    {
                        //Compute number of team for current qualification step.
                        var rest = result.TeamList.Count%qualificationSetting.CountGroup;
                        var numberOfTeam = result.TeamList.Count / qualificationSetting.CountGroup + //Min number of team per group
                                           (rest != 0 && i < rest ? 1 : 0); //Add one if has rest of division and position qualification group is lower than rest
                        
                        //Build team list for current step.
                        var groupTeamList = new List<ITeam>();
                        for (var j = 0; j < numberOfTeam && teamindex + j < result.TeamList.Count; j++) groupTeamList.Add(result.TeamList.ElementAt(teamindex + j));
                        teamindex += numberOfTeam; 

                        //Create and add new game step.
                        var newQualificationStep = QualificationStep.Create(result, groupTeamList, qualificationSetting, i + 1);
                        result.RegisterHandler(newQualificationStep);
                        result.GameStepList.Add(newQualificationStep);
                    }
                    break;
                case PhaseType.Consoling:
                case PhaseType.Main:
                    var eliminationSetting = setting as EliminationStepSetting;
                    if (eliminationSetting == null) throw new ArgumentException("Setting can not be null or a differnet type of QualificationStepSetting", "setting");
                    //Create and add new game step
                    var newEliminationStep = EliminationStep.Create(result, result.TeamList, setting.MatchSetting, eliminationSetting.FirstStep);
                    result.RegisterHandler(newEliminationStep);
                    result.GameStepList.Add(newEliminationStep);
                    break;
                default: throw new NotSupportedException(string.Format("Le type de phase n'est pas supporté par l'application. Type:{0}.", result.Type));
            }

            foreach (var gameStep in result.GameStepList)
            {
                gameStep.BuildMatch();
            }

            return result;
        }

        #endregion
    }
}
