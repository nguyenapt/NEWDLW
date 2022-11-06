namespace Netafim.WebPlatform.Web.Core.Services
{
    public interface IEmailSettings
    {
        string EmailSenderAddress { get; }
        string SendGridApiKey { get; }
        string SendGridTemplateId { get; }
    }

    public interface IEmailTemplateSettings
    {
        string Subject { get; set; }
        string Heading { get; set; }
        string CcAddresses { get; set; }
        string BccAddresses { get; set; }
    }
}