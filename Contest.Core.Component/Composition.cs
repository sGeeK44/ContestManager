using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Contest.Core.Component
{
    public static class Composition
    {
        public static void Compose(object objToCompose)
        {
            //Build assemblies list in wich look for export concrete object.
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                                                                            .Select(_ => new AssemblyCatalog(_))
                                                                            .Cast<ComposablePartCatalog>()
                                                                            .ToList();
            //Make Aggregate catalogue with previous list.
            var catalog = new AggregateCatalog(assemblies);
            var container = new CompositionContainer(catalog);

            //Resolve MEF import.
            container.ComposeParts(objToCompose);
        }
    }
}
