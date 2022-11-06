using System.Collections.Generic;
using System.Linq;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Forms.EditView.Models.Internal;
using EPiServer.Security;
using Netafim.WebPlatform.Web.Features.FormContainerBlock;

namespace Netafim.WebPlatform.Web.Features.JobApplicationForm
{
    [ContentGenerator(Order = 100)]
    public class JobApplicationFormContentGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;
        private ContentFolder FormFolder { get; set; }

        public JobApplicationFormContentGenerator(IContentRepository contentRepository, ContentAssetHelper contentAssetHelper)
        {
            _contentRepository = contentRepository;
            _contentAssetHelper = contentAssetHelper;
        }

        public void Generate(ContentContext context)
        {
            var folders = _contentRepository.GetDefault<ContentFolder>(ContentReference.SiteBlockFolder);
            FormFolder = _contentRepository.GetChildren<ContentFolder>(folders.ParentLink)
                                                .SingleOrDefault(t => t.Name == "Episerver Forms");

            var assetsFolder = _contentAssetHelper.GetOrCreateAssetFolder(context.Homepage);

            CreateJobApplicationFormBlock(assetsFolder);
        }

        private void CreateJobApplicationFormBlock(ContentAssetFolder assetsFolder)
        {
            var formContent = this._contentRepository.GetDefault<GeneralFormContainerBlock>(assetsFolder.ContentLink);
            ((IContent)formContent).Name = "Job Application form";
            formContent.Title = "Apply online";
            formContent.Description = "Enter some personal details and upload your full CV to apply online for a job.";
            formContent.MandatoryInformation = "All fields marked with * are mandatory";
            formContent.PrivacyStatement = "We respect your privacy and will never share your contact details with any person or organization nor spam you with unwanted emails.";

            formContent.FormContainer = formContent.FormContainer ?? new ContentArea();
            formContent.FormContainer.Items.Add(new ContentAreaItem()
            {
                ContentLink = CreateFormContainer()
            });

            Save((IContent) formContent);
        }

        private ContentReference CreateFormContainer()
        {
            var container = EpiFormGeneratorDataFactory.CreateFormContainerBlock(FormFolder, "Job Application Form");

            // selection position
            var pos = new List<OptionItem>
            {
                new OptionItem
                {
                    Value = "Sale & Marketing",
                    Caption = "Sale & Marketing"
                },
                new OptionItem
                {
                    Value = "CEO",
                    Caption = "CEO"
                },
                new OptionItem
                {
                    Value = "Farmer",
                    Caption = "Farmer"
                }
            };
            var position = EpiFormGeneratorDataFactory.CreateSelectionElementBlock(FormFolder, "position", "selected position", "select position", pos);

            container.ElementsArea.Items.Add(new ContentAreaItem()
            {
                ContentLink = Save((IContent)position)
            });

            // full name
            var tbFullname = EpiFormGeneratorDataFactory.CreateTextboxElementBlock(FormFolder, "fullname", "full name*", "Full name");
            container.ElementsArea.Items.Add(new ContentAreaItem()
            {
                ContentLink = Save((IContent)tbFullname)
            });

            // email address
            var tbEmail = EpiFormGeneratorDataFactory.CreateTextboxElementBlock(FormFolder, "email", "e-mail address*", "e-mail address");
            container.ElementsArea.Items.Add(new ContentAreaItem()
            {
                ContentLink = Save((IContent)tbEmail)
            });

            // email address
            var tbPersonStatement = EpiFormGeneratorDataFactory.CreateTextareaElementBlock(FormFolder, "statement", "person statement*", "Type here your question remark");
            container.ElementsArea.Items.Add(new ContentAreaItem()
            {
                ContentLink = Save((IContent)tbPersonStatement)
            });

            // cv upload
            var fileUpload = EpiFormGeneratorDataFactory.CreateFileUploadElementBlock(FormFolder, "cvupload", "curriculum vitae");
            container.ElementsArea.Items.Add(new ContentAreaItem()
            {
                ContentLink = Save((IContent)fileUpload)
            });

            // captcha
            var captcha = EpiFormGeneratorDataFactory.CrearteCaptchaElementBlock(FormFolder, "captcha", string.Empty);
            container.ElementsArea.Items.Add(new ContentAreaItem()
            {
                ContentLink = Save((IContent)captcha)
            });

            // submit button
            var submitButton = EpiFormGeneratorDataFactory.CreateSubmitButtonElementBlock(FormFolder, "send", "Send");

            container.ElementsArea.Items.Add(new ContentAreaItem()
            {
                ContentLink = Save((IContent)submitButton)
            });
            

            return Save((IContent)container);
        }

        private ContentReference Save(IContent content)
        {
            return this._contentRepository.Save(content, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }
}