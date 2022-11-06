using EPiServer.Forms.Core.PostSubmissionActor;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Core.GoogleAnalytics;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.ContactForm
{
    public class GoogleAnalyticsSubmissionActor : PostSubmissionActorBase
    {
        public override object Run(object input)
        {
            SendContactFormToGA();

            return input;
        }

        private void SendContactFormToGA()
        {
            var friendlyNameInfos = this.FormIdentity.GetFriendlyNameInfos();
            var email = this.SubmissionData.GetElementValueFromFriendlyName(friendlyNameInfos, "email");
            var subject = this.SubmissionData.GetElementValueFromFriendlyName(friendlyNameInfos, "subject");

            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(email))
                return;

            var googleAnalyticsSettings = ServiceLocator.Current.GetInstance<IGoogleAnalyticsSettings>();

            var contactDomain = string.IsNullOrEmpty(email)
                ? string.Empty
                : email.Split('@')[1];

            var eventCategory = $"{googleAnalyticsSettings.PrefixGAEventCategory} Contact form";

            var gaEventParametersModel = new GaEventParameters
            {
                ClientId = this.HttpRequestContext.GetGAClientId(),
                EventCategory = eventCategory.Trim(),
                EventAction = subject,
                EventLabel = $"@{contactDomain}"
            };

            var googleAnalytic = ServiceLocator.Current.GetInstance<IGoogleAnalytics>();
            googleAnalytic.TrackEvent(gaEventParametersModel);
        }
    }
}