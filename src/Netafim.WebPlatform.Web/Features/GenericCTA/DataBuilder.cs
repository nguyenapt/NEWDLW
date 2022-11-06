using EPiServer.Web.Routing;
using EPiServer.ServiceLocation;
using EPiServer;
using EPiServer.Core;
using Netafim.WebPlatform.Web.Core.Extensions;

namespace Netafim.WebPlatform.Web.Features.GenericCTA
{
    public static class DataBuilder
    {
        private const string DataPath = "~/Features/GenericCTA/Data/Demo/{0}";

        public static TBlock Init<TBlock>(this TBlock block) where TBlock : GenericCTABlock
        {
            var writableBlock = block.CreateWritableClone() as TBlock;
            ((IContent)writableBlock).Name = "GENERIC CTA";
            writableBlock.Watermark = "GENERIC CTA";
            writableBlock.Title = "GENERIC CTA BLOCK";
            writableBlock.Icon = ((IContent)writableBlock).CreateBlob(string.Format(DataPath, "54x54.png"));

            return writableBlock;
        }

        public static GenericCTABlock MarkAsInline(this GenericCTABlock block)
        {
            block.IsInlineMode = true;
            block.Title = string.Empty;
            block.Description = "Contact us and one of our experts will get back to you shortly.";
            return block;
        }

        public static GenericCTABlock LinkToEmail(this GenericCTABlock block, string email)
        {
            var urlResolver = ServiceLocator.Current.GetInstance<IUrlResolver>();
            block.Link = new Url($"mailto:{email}");
            block.LinkText = "CONTACT HELEN";

            return block;
        }

        public static GenericCTABlock LinkToDownload(this GenericCTABlock block, string file)
        {
            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            var pdf = ((IContent)block).CreateBlob(string.Format(DataPath, file));
            block.Link = new Url(urlResolver.GetUrl(pdf));
            block.Description = "(PDF, 1.5mb)";
            block.LinkText = "DOWNLOAD CATALOG";

            return block;
        }

        public static GenericCTABlock LinkToContent(this GenericCTABlock block, ContentReference link)
        {
            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            block.Link = new Url(urlResolver.GetUrl(link));
            block.LinkText = "VISIT TO INTERNAL";

            return block;
        }

        public static GenericCTABlock LinkToExternal(this GenericCTABlock block, string externalLink)
        {
            block.Link = new Url(externalLink);
            block.LinkText = "EXTERNAL LINK";

            return block;
        }

        public static OverlayCTABlock LinkToOverlay(this OverlayCTABlock block, ContentReference overlayContent)
        {
            block.LinkText = "CONTACT US";
            block.OverlayContent = block.OverlayContent ?? new ContentArea();
            block.OverlayContent.Items.Add(new ContentAreaItem() { ContentLink = overlayContent });

            return block;
        }
    }
}