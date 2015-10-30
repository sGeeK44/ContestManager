using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Contest.Core.Component
{
    /// <summary>
    /// Compose part with all export in executing assemblies
    /// </summary>
    public class ExecutingAssemblies : IComposer
    {
        private readonly Lazy<CompositionContainer> _customContainer = new Lazy<CompositionContainer>(() =>
        {
            var list = AppDomain.CurrentDomain
                                                        .GetAssemblies()
                                                        .Select(_ => new AssemblyCatalog(_))
                                                        .Cast<ComposablePartCatalog>()
                                                        .ToList();
            var catalog = new AggregateCatalog(list);
            return new CompositionContainer(catalog);
        });

        /// <summary>
        /// Creates composable parts from an attributed objects and composes
        /// </summary>
        /// <param name="obj">attributed objects to compose.</param>
        public void ComposeParts(object obj)
        {
            _customContainer.Value.ComposeParts(obj);
        }
    }
}
