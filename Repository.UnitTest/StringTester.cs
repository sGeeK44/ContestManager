using System;

namespace Contest.Core.Repository.UnitTest
{
    internal class StringTester : IIdentifiable
    {
        public string ItemToTest { get; set; }

        public StringTester(string itemToTest)
        {
            ItemToTest = itemToTest;
        }

        public Guid Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool AreSame(object other)
        {
            var castObject = other as StringTester;
            if (castObject == null) return false;
            return ItemToTest == castObject.ItemToTest;
        }
    }
}