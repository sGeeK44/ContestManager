using System;
using System.Collections.Generic;

namespace Contest.Domain.Games
{
    public abstract class RelationshipFactoryBase<T, TIObj1, TIObj2> : IRelationshipFactory<TIObj1, TIObj2>
        where T : IRelationship<TIObj1, TIObj2>
        where TIObj1 : IEntity
        where TIObj2 : IEntity
    {
        public IRelationship<TIObj1, TIObj2> Create(TIObj1 first, TIObj2 second)
        {
            var result = (IRelationship<TIObj1, TIObj2>) Activator.CreateInstance<T>();
            result.FirstItemInvolve = first;
            result.SecondItemInvolve = second;
            return result;
        }

        public IList<IRelationship<TIObj1, TIObj2>> CreateFromFirstItemList(IList<TIObj1> firstItemList,
            TIObj2 secondItem)
        {
            var result = new List<IRelationship<TIObj1, TIObj2>>();
            if (firstItemList == null) return result;

            foreach (var firstItem in firstItemList) result.Add(Create(firstItem, secondItem));

            return result;
        }

        public IList<IRelationship<TIObj1, TIObj2>> CreateFromSecondItemList(TIObj1 firstItem,
            IList<TIObj2> secondItemList)
        {
            var result = new List<IRelationship<TIObj1, TIObj2>>();
            if (secondItemList == null) return result;

            foreach (var secondItem in secondItemList) result.Add(Create(firstItem, secondItem));

            return result;
        }
    }
}