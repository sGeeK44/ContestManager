using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Contest.Core.Component
{
    public class DirectoryComposer : IComposer
    {
        private readonly Lazy<CompositionContainer> _container;

        public DirectoryComposer()
        {
            _container = new Lazy<CompositionContainer>(() => new CompositionContainer(new DirectoryCatalog(".")));
        }

        public CompositionContainer Container { get { return _container.Value; } }

        public void ComposeParts(object obj)
        {
            Container.ComposeParts(obj);
        }
    }
}
