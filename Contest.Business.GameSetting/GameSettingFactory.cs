using System;
using System.ComponentModel.Composition;

namespace Contest.Business
{
    [Export(typeof(IGameSettingFactory))]
    public class GameSettingFactory : IGameSettingFactory
    {
        /// <summary>
        /// Create a new instance (in memory and database) of <see cref="T:Business.GameSetting"/> with specified param
        /// </summary>
        /// <param name="minimumPlayerByTeam">Set minimum player register by team.</param>
        /// <param name="maximumPlayerByTeam">Set maximum player register by team.</param>
        /// <returns>GameSetting's instance</returns>
        public IGameSetting Create(uint minimumPlayerByTeam, uint maximumPlayerByTeam)
        {
            if (maximumPlayerByTeam < 1) throw new ArgumentException("Une équipe doit au moins pouvoir posséder un joueur");

            var result = new GameSetting
            {
                MinimumPlayerByTeam = minimumPlayerByTeam,
                MaximumPlayerByTeam = maximumPlayerByTeam
            };

            return result;
        }
    }
}
