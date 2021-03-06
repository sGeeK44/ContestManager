﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Core.FrameworkExtension
{
    public static class EnumerableExtension
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Build a random list with specified list.
        /// </summary>
        /// <param name="list">List to randomize</param>
        /// <returns>Randomized list.</returns>
        public static IList<T> BuildRandomList<T>(this IList<T> list)
        {
            if (list == null) throw new ArgumentNullException("teamList");

            var randomSort = new SortedDictionary<int, T>();
            var rand = new Random();
            foreach (var item in list)
            {
                int randomPosition;
                while (randomSort.ContainsKey(randomPosition = rand.Next())) { }
                randomSort.Add(randomPosition, item);
            }
            return randomSort.Values.ToList();
        }
    }
}
