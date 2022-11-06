using EPiServer;
using EPiServer.Core;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Core.Extensions;
using System;

namespace Netafim.WebPlatform.Web.Features.Layout
{
    public interface ILinkViewModelFactory
    {
        bool IsSatisfied(LinkItem linkItem);
        LinkViewModel Create(LinkItem linkItem);
    }
    
    public class EmaiLinkViewModelFactory : ILinkViewModelFactory
    {
        public LinkViewModel Create(LinkItem linkItem)
        {
            return new LinkViewModel(linkItem.Text, linkItem.GetMappedHref(), linkItem.Href);
        }

        public bool IsSatisfied(LinkItem linkItem)
        {
            return linkItem?.Href?.StartsWith("mailto:") == true;
        }
    }

    public abstract class UrlLinkViewModelFactory : ILinkViewModelFactory
    {
        protected readonly IContentLoader _contentLoader;

        protected UrlLinkViewModelFactory(IContentLoader contentLoader)
        {
            this._contentLoader = contentLoader;
        }

        public abstract LinkViewModel Create(LinkItem linkItem);
        public abstract bool IsSatisfied(LinkItem linkItem);
        protected IContent GetContentFrom(LinkItem linkItem)
        {
            return linkItem?.Href.GetContent(_contentLoader);
        }
    }
    
    public class ExternalLinkViewModelFactory : UrlLinkViewModelFactory
    {
        public ExternalLinkViewModelFactory(IContentLoader contentLoader) : base(contentLoader)
        {
        }

        public override LinkViewModel Create(LinkItem linkItem)
        {
            return new LinkViewModel(linkItem.Text, linkItem.GetMappedHref(), linkItem.Href);
        }

        public override bool IsSatisfied(LinkItem linkItem)
        {
            return GetContentFrom(linkItem) == null;
        }
    }

    public class InternalLinkViewModelFactory : UrlLinkViewModelFactory
    {
        private readonly IUrlResolver _urlRolver;
        public InternalLinkViewModelFactory(IContentLoader contentLoader, IUrlResolver urlResolver)
            : base(contentLoader)
        {
            this._urlRolver = urlResolver;
        }

        public override LinkViewModel Create(LinkItem linkItem)
        {
            return new LinkViewModel(linkItem.Text, _urlRolver.GetUrl(linkItem.Href), linkItem.Href);
        }

        public override bool IsSatisfied(LinkItem linkItem)
        {
            return GetContentFrom(linkItem) != null;
        }
    }
}