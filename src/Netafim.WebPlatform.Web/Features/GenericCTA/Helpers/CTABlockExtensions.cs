using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netafim.WebPlatform.Web.Features.GenericCTA.Helpers
{
    public static class CTABlockExtensions
    {
        /// <summary>
        /// Resolve the link factory 
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public static IUrlLinkFactory GetUrlLinkFactory(this GenericCTABlock block)
        {
            if (block == null)
                throw new ArgumentNullException("The Generic CTA block model is null.");

            var linkFactories = ServiceLocator.Current.GetInstance<IEnumerable<IUrlLinkFactory>>();

            if (linkFactories == null || !linkFactories.Any())
                throw new ArgumentOutOfRangeException($"None of {nameof(IUrlLinkFactory)} is registrated.");

            var linkFactory = linkFactories.FirstOrDefault(f => f.IsSatisfied(block.Link));

            if (linkFactory == null)
                throw new ArgumentException($"Can not find any satisfied link factory for url {block.Link}.");

            return linkFactory;
        }
    }
}