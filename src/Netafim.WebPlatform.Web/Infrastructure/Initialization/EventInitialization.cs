using System;
using Dlw.EpiBase.Content.Cms.Extensions;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using InitializationModule = EPiServer.Web.InitializationModule;

namespace Netafim.WebPlatform.Web.Infrastructure.Initialization
{
    [InitializableModule]
    [ModuleDependency(typeof(InitializationModule))]
    public class EventInitialization : IInitializableModule
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(EventInitialization));

        private IContentRepository _contentRepository;
        private IUrlSegmentGenerator _urlSegementGenerator;

        public void Initialize(InitializationEngine context)
        {
            _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            _urlSegementGenerator = ServiceLocator.Current.GetInstance<IUrlSegmentGenerator>();

            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.SavedContent += EventInitialization_SavedContent;
        }

        private void EventInitialization_SavedContent(object sender, ContentEventArgs e)
        {
            var component = e.Content as IComponent;
            var savedContent = e.Content as ContentData;

            if (component != null && savedContent is IContent && savedContent.PropertyCanBeSaved(m => component.AnchorId))
            {
                var contentReference = savedContent.GetContentReference();

                try
                {
                    if (!ContentReference.IsNullOrEmpty(contentReference) && string.IsNullOrWhiteSpace(component.AnchorId))
                    {
                        var content = savedContent.CreateWritableClone() as IContent;

                        var anchorId = _urlSegementGenerator.Create(((IContent)savedContent).Name);
                        anchorId = !string.IsNullOrWhiteSpace(anchorId) ? anchorId : CreateDefaultAnchorId(content);

                        ((IComponent) content).AnchorId = anchorId;
                        _contentRepository.Save(content);
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error($"Could not generate anchor tag for block with content reference '{contentReference}'.", exception);
                }
            }
        }

        private string CreateDefaultAnchorId(IContent content) => $"AnchorId_{content?.ContentLink.ID}";
        
        public void Uninitialize(InitializationEngine context)
        {
            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.SavedContent -= EventInitialization_SavedContent;
        }
    }
}