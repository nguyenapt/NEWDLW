using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DbLocalizationProvider;
using EPiServer;
using EPiServer.Core;
using EPiServer.Forms.Core.Events;
using EPiServer.Forms.Core.Models;
using EPiServer.Forms.Core.Models.Internal;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Services;
using Netafim.WebPlatform.Web.Core.Templates;
using SendGrid;
using SendGrid.Helpers.Mail;
using InitializationModule = EPiServer.Web.InitializationModule;

namespace Netafim.WebPlatform.Web.Features.FormContainerBlock
{
    [InitializableModule]
    [ModuleDependency(typeof(InitializationModule))]
    public class EPiserverFormInitializationModule : IInitializableModule
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(EPiserverFormInitializationModule));
        
        private IContentLoader _contentLoader;
        private IContentRepository _contentRepository;
        private IEmailService _emailService;
        private IEmailSettings _emailSettings;

        const string ContextAwarePrefix = "__contextaware_";

        public void Initialize(InitializationEngine context)
        {
            _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            _emailService = ServiceLocator.Current.GetInstance<IEmailService>();
            _emailSettings = ServiceLocator.Current.GetInstance<IEmailSettings>();
            _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            
            var formsEvents = ServiceLocator.Current.GetInstance<FormsEvents>();
            formsEvents.FormsSubmitting += FormsEvents_FormsSubmitting;
        }

        private void FormsEvents_FormsSubmitting(object sender, FormsEventArgs e)
        {
            var submission = e as FormsSubmittingEventArgs;
            var rawdata = e.Data as NameValueCollection;
            
            var content = _contentLoader.Get<IContent>(e.FormsContent.ContentLink);
            var localizable = content as ILocalizable;
            if (content == null || localizable == null || submission == null)
                return;
            
            var formsId = new FormIdentity(e.FormsContent.ContentGuid, localizable.Language.Name);
            var friendlyNameInfos = formsId.GetFriendlyNameInfos();
            var email = submission.SubmissionData.GetElementValueFromFriendlyName(friendlyNameInfos, "email");

            if (string.IsNullOrEmpty(email))
            {
                _logger.Information("Send email after submission form: Email address is null or empty");
                return;
            }

            if (rawdata != null && rawdata.AllKeys.Any(k => k != null && k.ToLower().Trim().StartsWith(ContextAwarePrefix)))
            {
                ContentReference contentReference;
                if (!ContentReference.TryParse(rawdata[ContextAwarePrefix], out contentReference))
                    return;

                var container = _contentRepository.Get<IContent>(contentReference) as IEmailTemplateSettings;
                if(container == null)
                    return;

                var response = Task.Run(async () =>
                {
                    return await SendEmail(friendlyNameInfos, submission, container);
                }).Result;

                if (response.StatusCode != HttpStatusCode.Accepted)
                {
                    submission.CancelAction = true;
                    submission.CancelReason = LocalizationProvider.Current.GetString(() => Labels.FormSubmittedCancelReason);

                    _logger.Information($"Unable to Send Email. Status - {response.StatusCode}");
                }
            }
        }

        private async Task<Response> SendEmail(IEnumerable<FriendlyNameInfo> friendlyNameInfos, FormsSubmittingEventArgs submission, IEmailTemplateSettings container)
        {
            var email = submission.SubmissionData.GetElementValueFromFriendlyName(friendlyNameInfos, "email");

            var fileUploads = friendlyNameInfos.GetFileUploadUrls(submission.SubmissionData);

            var message = new Message()
            {
                From = new EmailAddress(_emailSettings.EmailSenderAddress),
                Tos = new List<EmailAddress> { new EmailAddress(email) },
                Subject = container.Subject,
                ContentSubstitutions = new Dictionary<string, string>
                                        {
                                            {"-heading-", container.Heading},
                                            {"-content-", friendlyNameInfos.BuildHtmlContent(submission.SubmissionData)}
                                        },
                Attachments = CreateAttachmentFromContentUrl(fileUploads)
            };

            if (!string.IsNullOrEmpty(container.CcAddresses))
            {
                message.Ccs = container.CcAddresses.ToEmailAddressesBySeparator(';');
            }
            if (!string.IsNullOrEmpty(container.BccAddresses))
            {
                message.Bccs = container.BccAddresses.ToEmailAddressesBySeparator(';');
            }

            return await _emailService.SendEmail(message);
            
        }

        private static Dictionary<string, string> CreateAttachmentFromContentUrl(IEnumerable<string> urls)
        {
            var attachments = new Dictionary<string, string>();

            if (!urls.Any())
                return attachments;

            foreach (var url in urls.Where(u => !string.IsNullOrEmpty(u)))
            {
                var data = url.GetFileUploadDataFromUrl();
                if (data == null)
                    continue;

                var fileparts = url.Split(new[] { "#@" }, StringSplitOptions.None);
                attachments.Add(fileparts.Last(), Convert.ToBase64String(data, 0, data.Length));
            }

            return attachments;
        }

        public void Uninitialize(InitializationEngine context)
        {
            var formsEvents = ServiceLocator.Current.GetInstance<FormsEvents>();
            formsEvents.FormsSubmitting -= FormsEvents_FormsSubmitting;
        }
    }
}