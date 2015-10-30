using System;
using System.Runtime.Serialization;
using Contest.Core.Repository.Sql;
using Contest.Core.Windows;

namespace Contest.Business
{
    [DataContract(Name = "GAME_SETTING")]
    public class GameSetting : Identifiable<GameSetting>, IGameSetting
    {
        #region Constantes

        public const double LEAD_BOARD_WITH_BRASS_PUCK = 2.8;
        public const double LEAD_BOARD_WITH_FONTE_PUCK = 3.8;
        public const double WOOD_BOARD_WITH_FONTE_PUCK = 5;

        #endregion

        #region Enum

        public enum TypeOfPuck
        {
            [Display("Non renseigné")]
            Unknown,

            [Display("Laiton")]
            Brass,

            [Display("Fonte")]
            Fonte
        }

        public enum TypeOfPlayGround
        {
            [Display("Non renseigné")]
            Unknown,

            [Display("Bois")]
            WoodBoard,

            [Display("Plomb")]
            LeadBoard
        }

        #endregion

        #region Constructors

        private GameSetting() { }

        #endregion

        #region Properties

        /// <summary>
        /// Get type of puck
        /// </summary>
        [DataMember(Name = "PUCK")]
        public TypeOfPuck Puck { get; private set; }

        /// <summary>
        /// Get type of play ground
        /// </summary>
        [DataMember(Name = "PLAY_GROUND")]
        public TypeOfPlayGround PlayGround { get; private set; }

        /// <summary>
        /// Get distance between player and board
        /// </summary>
        public double? Distance
        {
            get { return GetDistance(PlayGround, Puck); }
        }

        /// <summary>
        /// Return distance conditionned by type of board and puck
        /// </summary>
        /// <param name="playGround">Type of board</param>
        /// <param name="puck">Type of puck</param>
        /// <returns></returns>
        public static double? GetDistance(TypeOfPlayGround playGround, TypeOfPuck puck)
        {
            switch (playGround)
            {
                case TypeOfPlayGround.LeadBoard:
                    switch (puck)
                    {
                        case TypeOfPuck.Brass:
                            return LEAD_BOARD_WITH_BRASS_PUCK;
                        case TypeOfPuck.Fonte:
                            return LEAD_BOARD_WITH_FONTE_PUCK;
                    }
                    break;
                case TypeOfPlayGround.WoodBoard:
                    switch (puck)
                    {
                        case TypeOfPuck.Fonte:
                            return WOOD_BOARD_WITH_FONTE_PUCK;
                    }
                    break;
            }
            return null;
        }

        /// <summary>
        /// Get minimum player register by team.
        /// </summary>
        [DataMember(Name = "MIN_PLAYER_PER_TEAM")]
        public uint MinimumPlayerByTeam { get; set; }

        /// <summary>
        /// Get maximum player register by team.
        /// </summary>
        [DataMember(Name = "MAX_PLAYER_PER_TEAM")]
        public uint MaximumPlayerByTeam { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Do all insert or update into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public virtual void PrepareCommit(ISqlUnitOfWorks unitOfWorks)
        {
            unitOfWorks.InsertOrUpdate<IGameSetting>(this);
        }

        /// <summary>
        /// Do all delete into repository for all of object composed.
        /// </summary>
        /// <param name="unitOfWorks">Unit of work for action</param>
        public void PrepareDelete(ISqlUnitOfWorks unitOfWorks)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Factory

        /// <summary>
        /// Create a new instance (in memory and database) of <see cref="T:Business.GameSetting"/> with specified param
        /// </summary>
        /// <param name="playGround">Set type of PlayGround</param>
        /// <param name="puck">Set type of Puck</param>
        /// <param name="minimumPlayerByTeam">Set minimum player register by team.</param>
        /// <param name="maximumPlayerByTeam">Set maximum player register by team.</param>
        /// <returns>GameSetting's instance</returns>
        public static IGameSetting Create(TypeOfPlayGround playGround, TypeOfPuck puck, uint minimumPlayerByTeam, uint maximumPlayerByTeam)
        {
            if (maximumPlayerByTeam < 1) throw new ArgumentException("Une équipe doit au moins pouvoir posséder un joueur");
            var result = new GameSetting
            {
                Id = Guid.NewGuid(),
                PlayGround = playGround,
                Puck = puck,
                MinimumPlayerByTeam = minimumPlayerByTeam,
                MaximumPlayerByTeam = maximumPlayerByTeam
            };

            return result;
        }

        #endregion
    }
}
