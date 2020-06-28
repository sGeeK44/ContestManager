using SmartWay.Orm.Attributes;

namespace Contest.Domain.Games
{
    [Entity(NameInStore = "GAME_SETTING")]
    public class GameSetting : Entity, IGameSetting
    {
        #region Constructors

        internal GameSetting()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Get minimum player register by team.
        /// </summary>
        [Field(FieldName = "MIN_PLAYER_PER_TEAM")]
        public uint MinimumPlayerByTeam { get; set; }

        /// <summary>
        ///     Get maximum player register by team.
        /// </summary>
        [Field(FieldName = "MAX_PLAYER_PER_TEAM")]
        public uint MaximumPlayerByTeam { get; set; }

        #endregion
    }
}