using System.Collections.Generic;
using Contest.Core.DataStore.Sql.ReferenceManyToMany;
using Moq;

namespace Contest.UnitTest.Kit
{
    public class RelationshipFactoryBaseMock<TIObj1, TIObj2> : IRelationshipFactory<TIObj1, TIObj2>
        where TIObj1 : IIdentifiable
        where TIObj2 : IIdentifiable
    {
        public IRelationship<TIObj1, TIObj2> Create(TIObj1 first, TIObj2 second)
        {
            var result = new Mock<IRelationship<TIObj1, TIObj2>>();
            result.Setup(_ => _.FirstItemInvolve).Returns(first);
            result.Setup(_ => _.SecondItemInvolve).Returns(second);
            return result.Object;
        }

        public IList<IRelationship<TIObj1, TIObj2>> CreateFromFirstItemList(IList<TIObj1> firstItemList, TIObj2 secondItem)
        {
            var result = new List<IRelationship<TIObj1, TIObj2>>();
            if (firstItemList == null) return result;

            foreach (var firstItem in firstItemList)
            {
                result.Add(Create(firstItem, secondItem));
            }

            return result;
        }

        public IList<IRelationship<TIObj1, TIObj2>> CreateFromSecondItemList(TIObj1 firstItem, IList<TIObj2> secondItemList)
        {
            var result = new List<IRelationship<TIObj1, TIObj2>>();
            if (secondItemList == null) return result;

            foreach (var secondItem in secondItemList)
            {
                result.Add(Create(firstItem, secondItem));
            }

            return result;
        }
    }
}