using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using EPiServer.Logging;
using StructureMap;

namespace Dlw.EpiBase.Content.Infrastructure.IoC
{
    // source: episerver Quicksilver sample
    public class StructureMapResolver : StructureMapDependencyScope, IDependencyResolver, IHttpControllerActivator
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(StructureMapResolver));

        private readonly IContainer _container;

        public StructureMapResolver(IContainer container)
            : base(container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            _container = container;

            _container.Inject(typeof(IHttpControllerActivator), this);
        }

        public IDependencyScope BeginScope()
        {
            return new StructureMapDependencyScope(_container.GetNestedContainer());
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            try
            {
                return _container.GetNestedContainer().GetInstance(controllerType) as IHttpController;
            }
            catch (Exception e)
            {
                // exceptions don't bubble up
                _logger.Error($"Could not resolve controller '{controllerType}'.", e);

                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (_container != null)
            {
                _container.Dispose();
            }

            base.Dispose(true);
        }
    }
}