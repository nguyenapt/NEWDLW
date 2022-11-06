using System;
using System.Linq;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.FormContainerBlock;
using Netafim.WebPlatform.Web.Features.GenericCTA;
using Netafim.WebPlatform.Web.Features.MediaCarousel;
using Netafim.WebPlatform.Web.Features.RichText.Models;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Editors;

namespace Netafim.WebPlatform.Web.Features.JobDetails
{
    [ContentGenerator(Order = 110)]
    public class JobDetailsContentGenerator : IContentGenerator
    {
        private const string Thumbnail = "~/Features/MediaCarousel/Data/Demo/testimonial_senegal.png";

        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;

        public JobDetailsContentGenerator(IContentRepository contentRepository, ContentAssetHelper contentAssetHelper)
        {
            _contentRepository = contentRepository;
            _contentAssetHelper = contentAssetHelper;
        }

        public void Generate(ContentContext context)
        {
            CreateJobDetailsPage(context);

        }

        private void CreateJobDetailsPage(ContentContext context)
        {
            var jobDetailsPage = _contentRepository.GetChildren<JobDetailsPage>(context.Homepage).SingleOrDefault();
            if (jobDetailsPage != null) return;

            jobDetailsPage = _contentRepository.GetDefault<JobDetailsPage>(context.Homepage);
            if (jobDetailsPage == null) return;

            ((IContent)jobDetailsPage).Name = "Job details page";
            jobDetailsPage.Title = "Commercial Manager JB-288";
            jobDetailsPage.Country = "TANZANIA";
            jobDetailsPage.Watermark = "Job offer";
            jobDetailsPage.OnParallaxEffect = true;
            jobDetailsPage.Introduction = new XhtmlString("<p>We are looking for an area sales manager base in Tanzania to support the business in Tanzania  to develop and drive the business in this area.</p>");
            jobDetailsPage.Location = "Tanzania";
            jobDetailsPage.Department = "Sales &amp; marketing";
            jobDetailsPage.Postingdate = new DateTime(2017, 4, 5);
            jobDetailsPage.EndDate = new DateTime(2017, 6, 17);
            jobDetailsPage.JobSchedule = "Full-time";
            jobDetailsPage.JobApplicationForm = jobDetailsPage.JobApplicationForm ?? new ContentArea();
            jobDetailsPage.Content = jobDetailsPage.Content ?? new ContentArea();
            var jobDetailsPageReference = Save(jobDetailsPage);
            var assetsFolder = _contentAssetHelper.GetOrCreateAssetFolder(jobDetailsPageReference);
            var epiFormFolder = _contentAssetHelper.GetOrCreateAssetFolder(context.Homepage);
            var applicationForm = CreateJobApplicationFormBlock(epiFormFolder.ContentLink);
            // Add Job application form overlay
            jobDetailsPage.JobApplicationForm.Items.Add(new ContentAreaItem()
            {
                ContentLink = applicationForm
            });

            // Add image component
            jobDetailsPage.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = CreateImageComponet(assetsFolder)
            });
            // Add richtext component
            jobDetailsPage.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = CreateRichTextComponent(assetsFolder)
            });
            // Add Cta component
            jobDetailsPage.Content.Items.Add(new ContentAreaItem()
            {
                ContentLink = CreateCtaComponent(assetsFolder, applicationForm)
            });
            
            Save(jobDetailsPage);
        }

        private ContentReference CreateImageComponet(ContentAssetFolder assetsFolder)
        {
            // Create carousel item
            var imageCarouselBlock = _contentRepository.GetDefault<ImageCarouselBlock>(assetsFolder.ContentLink);
            ((IContent)imageCarouselBlock).Name = "Job details (image block)";
            imageCarouselBlock.Image = ((IContent)imageCarouselBlock).CreateBlob(Thumbnail);

            // Create carousel container
            var containerBlock = _contentRepository.GetDefault<MediaCarouselContainerBlock>(assetsFolder.ContentLink);
            ((IContent)containerBlock).Name = "Job details (Image component)";
            containerBlock.Items = new ContentArea();
            containerBlock.Items.Items.Add(new ContentAreaItem()
            {
                ContentLink = Save((IContent)imageCarouselBlock)
            });

            return Save((IContent)containerBlock);
        }

        private ContentReference CreateRichTextComponent(ContentAssetFolder assetsFolder)
        {
            var richTextContainer = _contentRepository.GetDefault<RichTextContainerBlock>(assetsFolder.ContentLink);
            ((IContent)richTextContainer).Name = "Job details (Richtext component)";
            richTextContainer.Items = richTextContainer.Items ?? new ContentArea();
            GenerateTextBlock(assetsFolder, richTextContainer);

            return Save((IContent)richTextContainer);
        }

        private void GenerateTextBlock(ContentAssetFolder contentAssetFolder, RichTextContainerBlock container)
        {
            var rtTextBlockJobSummary = _contentRepository.GetDefault<RichTextTextBlock>(contentAssetFolder.ContentLink);
            ((IContent) rtTextBlockJobSummary).Name = "Job details (Job summary)";

            rtTextBlockJobSummary.Content = new XhtmlString(@"<ul class='small-bullet-list'>
					<li>
						<p>Market development - both projects and dealers' distribution network</p>
					</li>
					<li>
						<p>Increase sales in projects and dealers while maintaining and maximizing profitability by products and solutions</p>
					</li>
					<li>
						<p>Increase project lead generation;</p>
					</li>
                    <li>
						<p>Facilitate sales promotion activities including demo-fields, seminars, lectures, participation in conferences and exhibitions</p>
					</li>
                    <li>
						<p>Implement efficient communication and support interfaces for sales while collaborating with GBU and the Division</p>
					</li>
                    <li>
						<p>Lead and actively involved in achieving business objectives, from planning through execution and control & monitoring – with regards to GBU Africa's strategy</p>
					</li>
				</ul>");
            var richTextReference = Save((IContent)rtTextBlockJobSummary);

            var rtTextBlockKeyRequirements = _contentRepository.GetDefault<RichTextTextBlock>(contentAssetFolder.ContentLink);
            ((IContent)rtTextBlockKeyRequirements).Name = "Job details (Key Requirements)";

            rtTextBlockKeyRequirements.Content = new XhtmlString(@"<ul class='small-bullet-list'>
					<li>
						<p>Expert and experienced in agronomy, drip irrigation and Nutrigation applications</p>
					</li>
					<li>
						<p>Experience in Sales/Commercial/Marketing and agronomy.</p>
					</li>
					<li>
						<p>Experience in Team establishment and hands-on management</p>
					</li>
                    <li>
						<p>Being able to work alone in a very hard and challenging territory</p>
					</li>
                    <li>
						<p>Being able to manage low-skilled employees and 3rd parties</p>
					</li>
                    <li>
						<p>A strong person who can drive the business forward.  Requires attention to detail and drive to make things happen. Must be results-oriented with strong business acumen</p>
					</li>
				</ul>");
            var richTextKeyRequirementsReference = Save((IContent)rtTextBlockKeyRequirements);

            container.Items.Items.Add(new ContentAreaItem() { ContentLink = richTextReference });
            container.Items.Items.Add(new ContentAreaItem() { ContentLink = richTextKeyRequirementsReference });
        }

        private ContentReference CreateCtaComponent(ContentAssetFolder assetsFolder, ContentReference applicationForm)
        {
            var overlayCta = this._contentRepository.GetDefault<OverlayCTABlock>(assetsFolder.ContentLink);
            ((IContent)overlayCta).Name = "GENERIC CTA";
            overlayCta.Title = "become part of netafim";
            overlayCta.Watermark = "open application";
            overlayCta.OnParallaxEffect = true;
            overlayCta.BackgroundColor = BackgroundColor.Grey;
            overlayCta.LinkText = "Apply Now";
            overlayCta.Description = "Apply online and upload your CV via the online application form";
            overlayCta.OverlayContent = overlayCta.OverlayContent ?? new ContentArea();
            overlayCta.OverlayContent.Items.Add(new ContentAreaItem()
            {
                ContentLink = applicationForm
            });

            return Save((IContent) overlayCta);
        }

        private ContentReference Save(IContent content)
        {
            return this._contentRepository.Save(content, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private ContentReference CreateJobApplicationFormBlock(ContentReference parentLink)
        {
            var formBlocks = _contentRepository.GetChildren<GeneralFormContainerBlock>(parentLink);
            if(formBlocks == null || !formBlocks.Any())
                return ContentReference.EmptyReference;

            var applicationForm = formBlocks.SingleOrDefault(t => ((IContent) t).Name == "Job Application form");

            return ((IContent)applicationForm).ContentLink;
        }
    }
}