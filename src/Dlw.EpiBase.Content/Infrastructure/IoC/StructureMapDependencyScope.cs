using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using StructureMap;

namespace Dlw.EpiBase.Content.Infrastructure.IoC
{
    // source: episerver Quicksilver sample
    public class StructureMapDependencyScope : IDependencyScope
    {
        private readonly IContainer _container;

        public StructureMapDependencyScope(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            if (_container == null)
            {
                throw new ObjectDisposedException("this", "This scope has already been disposed.");
            }
            return _container.TryGetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (_container == null)
            {
                throw new ObjectDisposedException("this", "This scope has already been disposed.");
            }
            return _container.GetAllInstances(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (_container != null)
            {
                _container.Dispose();
            }
        }
    }
}