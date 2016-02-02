using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Business
{
    /// <summary>
    /// Provide non affiliate class methods
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Build a random list with specified list.
        /// </summary>
        /// <param name="teamList">Team list to randomize</param>
        /// <returns>Randomized list.</returns>
        public static List<ITeam> BuildRandomList(List<ITeam> teamList)
        {
            if (teamList == null) throw new ArgumentNullException("teamList");

            var randomSort = new SortedDictionary<int, ITeam>();
            var rand = new Random();
            foreach (var team in teamList)
            {
                int randomPosition;
                while (randomSort.ContainsKey(randomPosition = rand.Next())) { }
                randomSort.Add(randomPosition, team);
            }
            return randomSort.Values.ToList();
        }
    }
}
