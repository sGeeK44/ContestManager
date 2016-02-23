using System;
using System.Runtime.Serialization;
using Contest.Core.Repository.Sql;

namespace Contest.Business
{
    [DataContract(Name = "GAME_SETTING")]
    public class GameSetting : Identifiable<GameSetting>, IGameSetting
    {
        #region Constructors

        internal GameSetting() { }

        #endregion

        #region Properties

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

        public override bool Equals(object other)
        {
            var castedObj = other as IGameSetting;
            if (castedObj == null) return false;

            return castedObj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return MinimumPlayerByTeam.GetHashCode() + MaximumPlayerByTeam.GetHashCode();
        }

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
    }
}
