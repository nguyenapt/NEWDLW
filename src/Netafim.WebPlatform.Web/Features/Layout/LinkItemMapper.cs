using System.Linq;
using Castle.Core.Internal;
using EPiServer.SpecializedProperties;
using System;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Layout
{
    public class LinkItemMapper : ILinkItemMapper
    {
        protected readonly IEnumerable<ILinkViewModelFactory> _customLinkFactories;

        public LinkItemMapper(IEnumerable<ILinkViewModelFactory> customLinkFactories)
        {
            _customLinkFactories = customLinkFactories;
        }

        public IEnumerable<LinkViewModel> GetLinkItems(LinkItemCollection linkCollection)
        {
            if (linkCollection.IsNullOrEmpty()) return Enumerable.Empty<LinkViewModel>();

            var result = new List<LinkViewModel>();

            foreach (var item in linkCollection)
            {
                var linkFactory = this._customLinkFactories.FirstOrDefault(f => f.IsSatisfied(item));

                if (linkFactory == null)
                    throw new Exception("Can not find any satisfied LinkFactory for the link item");

                result.Add(linkFactory.Create(item));
            }

            return result;
        }
    }
}