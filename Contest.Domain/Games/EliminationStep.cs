using System;
using System.Collections.Generic;
using Contest.Domain.Matchs;
using Contest.Domain.Players;
using JetBrains.Annotations;
using SmartWay.Orm.Attributes;

namespace Contest.Domain.Games
{
    [Entity(NameInStore = "ELIMINATION_STEP")]
    public class EliminationStep : GameStep, IEliminationStep
    {
        #region Factory

        /// <summary>
        ///     Create a new instance (in memory and database) of <see cref="T:Business.EliminationStep" /> with specified param
        /// </summary>
        /// <param name="phase">Phase linked to this new step</param>
        /// <param name="teamList">Team involved in this new step</param>
        /// <param name="matchSetting">Set setting match for new Elimination phase</param>
        /// <param name="firstStep">Set first step for elimination step</param>
        /// <returns>EliminationStep's instance</returns>
        public static IEliminationStep Create(IPhase phase, IList<ITeam> teamList, IMatchSetting matchSetting,
            EliminationType firstStep)
        {
            if (phase == null) throw new ArgumentNullException(nameof(phase));
            if (teamList == null) throw new ArgumentNullException(nameof(teamList));
            if (matchSetting == null) throw new ArgumentNullException(nameof(matchSetting));

            var requiredTeam = (ushort) firstStep * 2;
            if (teamList.Count != requiredTeam)
                throw new ArgumentException(
                    $"La première étape de la phase éliminatoire ne correspond pas au nombre équipe fourni. Nombre d'équipe requise:{requiredTeam}. Nombre d'équipe:{teamList.Count}");

            var result = new EliminationStep(phase, teamList, matchSetting)
            {
                Type = firstStep
            };

            return result;
        }

        #endregion

        #region Enum

        #endregion

        #region Constructors

        [UsedImplicitly]
        protected EliminationStep()
        {
        }

        protected EliminationStep(IPhase phase, IList<ITeam> teamList, IMatchSetting currentMatchSetting)
            : base(phase, teamList, currentMatchSetting)
        {
        }

        #endregion

        #region Properties

        public override string Name => Type.ToString();

        /// <summary>
        ///     Get type of current game step.
        /// </summary>
        [Field(FieldName = "TYPE")]
        public EliminationType Type { get; private set; }

        /// <summary>
        ///     Return next step, null if no futher step
        /// </summary>
        public override EliminationType? NextStep => DetermineNextEliminationStep(Type);

        #endregion

        #region Methods

        public override void BuildMatch()
        {
            MatchList.Clear();
            for (var i = 0; i < TeamList.Count; i += 2) CreateMatch(TeamList[i], TeamList[i + 1]);
        }

        public override IList<ITeam> GetDirectQualifiedTeam()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Static method

        /// <summary>
        ///     Determine type of step is after specified type
        /// </summary>
        /// <param name="actualStep">Reference to compute next step</param>
        /// <returns>Next step if exist, else null if is no step after.</returns>
        public static EliminationType? DetermineNextEliminationStep(EliminationType actualStep)
        {
            switch (actualStep)
            {
                case EliminationType.SixtyFourthRound:
                    return EliminationType.ThirtySecondRound;
                case EliminationType.ThirtySecondRound:
                    return EliminationType.SixteenthRound;
                case EliminationType.SixteenthRound:
                    return EliminationType.QuarterFinal;
                case EliminationType.QuarterFinal:
                    return EliminationType.SemiFinal;
                case EliminationType.SemiFinal:
                    return EliminationType.Final;
                case EliminationType.Final:
                    return null;
                default:
                    throw new NotSupportedException($"Le type de l'étape n'est pas supporté. Type:{actualStep}.");
            }
        }

        public static int CountStep(EliminationType firstStep)
        {
            switch (firstStep)
            {
                case EliminationType.SixtyFourthRound:
                    return 6;
                case EliminationType.ThirtySecondRound:
                    return 5;
                case EliminationType.SixteenthRound:
                    return 4;
                case EliminationType.QuarterFinal:
                    return 3;
                case EliminationType.SemiFinal:
                    return 2;
                case EliminationType.Final:
                    return 1;
                default:
                    throw new NotSupportedException($"Le type de l'étape n'est pas supporté. Type:{firstStep}.");
            }
        }

        public static int IndexStep(EliminationType firstStep)
        {
            switch (firstStep)
            {
                case EliminationType.SixtyFourthRound:
                    return 5;
                case EliminationType.ThirtySecondRound:
                    return 4;
                case EliminationType.SixteenthRound:
                    return 3;
                case EliminationType.QuarterFinal:
                    return 2;
                case EliminationType.SemiFinal:
                    return 1;
                case EliminationType.Final:
                    return 0;
                default:
                    throw new NotSupportedException($"Le type de l'étape n'est pas supporté. Type:{firstStep}.");
            }
        }

        /// <summary>
        ///     Determine type of step in according with specified param.
        /// </summary>
        /// <param name="numberOfTeam">Number of team in specified phase</param>
        /// <returns></returns>
        public static EliminationType DetermineEliminationTypeStep(int numberOfTeam)
        {
            switch (numberOfTeam)
            {
                case 2:
                    return EliminationType.Final;
                case 4:
                    return EliminationType.SemiFinal;
                case 8:
                    return EliminationType.QuarterFinal;
                case 16:
                    return EliminationType.SixteenthRound;
                case 32:
                    return EliminationType.ThirtySecondRound;
                case 64:
                    return EliminationType.SixtyFourthRound;
                default:
                    throw new ArgumentException(
                        $"Le nombre d'équipe n'est pas valide pour procéder à une étape éliminatoire. Nombre:{numberOfTeam}.");
            }
        }

        #endregion
    }
}