using System;
using System.ComponentModel.Composition;

namespace Contest.Business
{
    [Export(typeof(IFieldFactory))]
    public class FieldFactory : IFieldFactory
    {
        /// <summary>
        /// Create a new instance of Field with specified param
        /// </summary>
        /// <param name="current">Current contest</param>
        /// <param name="name">Name of new field</param>
        /// <returns>Field's instance</returns>
        public IField Create(IContest current, string name)
        {
            return new Field(current, name);
        }
    }
}
