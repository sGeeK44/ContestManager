﻿using System.Collections.Generic;
using Contest.Core.Repository;

namespace Contest.Business
{
    public interface IRelationshipFactory<TIObj1, TIObj2>
        where TIObj1 : IIdentifiable
        where TIObj2 : IIdentifiable
    {
        /// <summary>
        /// Create a new instance of <see cref="T:Business.IRelationship<TIObj1, TIObj2>"/> with specified param
        /// </summary>
        /// <param name="team1">First team involved in match</param>
        /// <param name="team2">Second team involved in match</param>
        /// <returns>New instance</returns>
        IRelationship<TIObj1, TIObj2> Create(TIObj1 first, TIObj2 second);

        /// <summary>
        /// Create a new Collection of relation from each first item list and second item
        /// </summary>
        /// <param name="firstItemList">First item list</param>
        /// <param name="secondItem">Second item</param>
        /// <returns>New Collection</returns>
        IList<IRelationship<TIObj1, TIObj2>> CreateFromFirstItemList(IList<TIObj1> firstItemList, TIObj2 secondItem);

        /// <summary>
        /// Create a new Collection of relation from each second item list and first item
        /// </summary>
        /// <param name="firstItem">First item</param>
        /// <param name="secondItemList">Second item list</param>
        /// <returns>New Collection</returns>
        IList<IRelationship<TIObj1, TIObj2>> CreateFromSecondItemList(TIObj1 firstItem, IList<TIObj2> secondItemList);
    }
}