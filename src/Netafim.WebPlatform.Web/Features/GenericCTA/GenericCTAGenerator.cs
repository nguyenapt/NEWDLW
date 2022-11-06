using EPiServer.Cms.Shell;
using System.Web.Http.Routing;
using EPiServer;
using EPiServer.Core;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Templates;
using System.Text;
using Netafim.WebPlatform.Web.Features.ContactForm;
using Netafim.WebPlatform.Web.Features.FormContainerBlock;

namespace Netafim.WebPlatform.Web.Features.GenericCTA
{
    public class GenericCTAGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;
        private readonly ContentAssetHelper _contentAssetHelper;

        public GenericCTAGenerator(IContentRepository contentRepository, IUrlSegmentCreator urlSegmentCreator,
            ContentAssetHelper contentAssetHelper)
        {
            _contentRepository = contentRepository;
            _urlSegmentCreator = urlSegmentCreator;
            _contentAssetHelper = contentAssetHelper;
        }

        public void Generate(ContentContext context)
        {
            EnsureComponent(context);
        }

        private void EnsureComponent(ContentContext context)
        {
            var ctaContainerPage = this._contentRepository.GetDefault<GenericContainerPage>(context.Homepage);
            ctaContainerPage.PageName = "CTA Page";
            var ctaRefenrece = Save(ctaContainerPage);
            ctaContainerPage.Content = ctaContainerPage.Content ?? new ContentArea();

            var assetsFolder = _contentAssetHelper.GetOrCreateAssetFolder(ctaRefenrece);

            // Internal CTA
            var ctaInternalLink = this._contentRepository.GetDefault<GenericCTABlock>(assetsFolder.ContentLink);
            ctaInternalLink = ctaInternalLink.Init()
                           .LinkToContent(context.Homepage);

            ctaContainerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = Save((IContent)ctaInternalLink) });


            // Internal CTA
            var ctaDownload = this._contentRepository.GetDefault<GenericCTABlock>(assetsFolder.ContentLink);
            ctaDownload = ctaDownload.Init()
                                     .LinkToDownload("pdftest.pdf");

            ctaContainerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = Save((IContent)ctaDownload) });


            // Email CTA
            var ctaEmail = this._contentRepository.GetDefault<GenericCTABlock>(assetsFolder.ContentLink);
            ctaEmail = ctaEmail.Init()
                               .LinkToEmail("cong.nguyen@niteco.se");

            ctaContainerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = Save((IContent)ctaEmail) });


            // External CTA
            var ctaExternal = this._contentRepository.GetDefault<GenericCTABlock>(assetsFolder.ContentLink);
            ctaExternal = ctaExternal.Init()
                                     .LinkToExternal("https://www.google.com");

            ctaContainerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = Save((IContent)ctaExternal) });

            // External CTA
            var overlayContent = this._contentRepository.GetDefault<GeneralFormContainerBlock>(assetsFolder.ContentLink).CreateWritableClone() as GeneralFormContainerBlock;
            overlayContent.Title = "This is the contact form";
            overlayContent.Description = "This is the contact form description";
            ((IContent)overlayContent).Name = "Contact us form";
            var overlayCta = this._contentRepository.GetDefault<OverlayCTABlock>(assetsFolder.ContentLink);
            overlayCta = overlayCta.Init()
                                .LinkToOverlay(Save((IContent)overlayContent));

            ctaContainerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = Save((IContent)overlayCta) });

            // Inline CTA
            var ctaInline = this._contentRepository.GetDefault<GenericCTABlock>(assetsFolder.ContentLink);
            ctaInline = ctaInline.Init()
                     .MarkAsInline()
                     .LinkToExternal("https://www.google.com");

            ctaContainerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = Save((IContent)ctaInline) });

            Save(ctaContainerPage);
        }

        private ContentReference Save(IContent content)
        {
            return _contentRepository.Save(content, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
        }
    }
}