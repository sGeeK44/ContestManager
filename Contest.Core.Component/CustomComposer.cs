using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Contest.Core.Component
{
    public class CustomComposer : IComposer
    {
        private readonly IList<Type> _catalogType = new List<Type>();
        private readonly Lazy<CompositionContainer> _container;

        public CustomComposer()
        {
            _container = new Lazy<CompositionContainer>(() => new CompositionContainer(new TypeCatalog(_catalogType)));
        }

        public CompositionContainer Container => _container.Value;

        public void AddType(Type typeToAdd)
        {
            _catalogType.Add(typeToAdd);
        }

        public void ComposeParts(object obj)
        {
            Container.ComposeParts(obj);
        }
    }
}
