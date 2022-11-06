using System.Collections.Generic;
using EPiServer;
using EPiServer.Core;
using EPiServer.Forms.Core;
using EPiServer.Forms.EditView.Models.Internal;
using EPiServer.Forms.Implementation.Elements;
using EPiServer.Forms.Implementation.Elements.BaseClasses;
using EPiServer.ServiceLocation;

namespace Netafim.WebPlatform.Web.Features.JobApplicationForm
{
    public static class EpiFormGeneratorDataFactory
    {
        public static EPiServer.Forms.Implementation.Elements.FormContainerBlock CreateFormContainerBlock(ContentFolder contentAssetFolder, string elementName)
        {
            var formContainer = InitElement<EPiServer.Forms.Implementation.Elements.FormContainerBlock>(contentAssetFolder, elementName);
            formContainer.ElementsArea = formContainer.ElementsArea ?? new ContentArea();
            formContainer.AllowAnonymousSubmission = true;
            formContainer.AllowMultipleSubmission = true;
            formContainer.ShowNavigationBar = false;
            formContainer.AllowExposingDataFeeds = false;

            return formContainer;
        }

        public static TextboxElementBlock CreateTextboxElementBlock(ContentFolder contentAssetFolder, string name, string label, string placeHolder)
        {
            var textbox = InitElement<TextboxElementBlock>(contentAssetFolder, name)
                .SetLabel(label)
                .SetPlaceHolder(placeHolder);

            return textbox;
        }

        public static TextareaElementBlock CreateTextareaElementBlock(ContentFolder contentAssetFolder, string name, string label, string placeHolder)
        {
            var textArea = InitElement<TextareaElementBlock>(contentAssetFolder, name)
                .SetLabel(label)
                .SetPlaceHolder(placeHolder);

            return textArea;
        }

        public static SelectionElementBlock CreateSelectionElementBlock(ContentFolder contentAssetFolder, string name, string label, string placeHolder, IEnumerable<OptionItem> items)
        {
            var selection = InitElement<SelectionElementBlock>(contentAssetFolder, name)
                .SetLabel(label)
                .SetPlaceHolder(placeHolder);
            selection.Items = items;

            return selection;
        }

        public static FileUploadElementBlock CreateFileUploadElementBlock(ContentFolder contentAssetFolder, string name, string label)
        {
            var fileUpload = InitElement<FileUploadElementBlock>(contentAssetFolder, name)
                .SetLabel(label);
            fileUpload.FileSize = 10; // 10MB

            return fileUpload;
        }

        public static CaptchaElementBlock CrearteCaptchaElementBlock(ContentFolder contentAssetFolder, string name, string label)
        {
            var captcha = InitElement<CaptchaElementBlock>(contentAssetFolder, name)
                .SetLabel(label);
            captcha.ImageWidth = 250;
            captcha.ImageHeight = 60;
            captcha.TextLength = 5;

            return captcha;
        }

        public static SubmitButtonElementBlock CreateSubmitButtonElementBlock(ContentFolder contentAssetFolder, string name, string label)
        {
            var submitButton = InitElement<SubmitButtonElementBlock>(contentAssetFolder, name).SetLabel(label);

            return submitButton;
        }

        private static T InitElement<T>(ContentFolder contentAssetFolder, string elementName) where T : BlockData
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var element = contentRepository.GetDefault<T>(contentAssetFolder.ContentLink);
            ((IContent)element).Name = elementName;

            return element;
        }

        private static T SetLabel<T>(this T element, string label) where T : ElementBlockBase
        {
            element.Label = label;
            return element;
        }

        private static T SetPlaceHolder<T>(this T element, string placeholder) where T : InputElementBlockBase
        {
            element.PlaceHolder = placeholder;
            return element;
        }
    }
}