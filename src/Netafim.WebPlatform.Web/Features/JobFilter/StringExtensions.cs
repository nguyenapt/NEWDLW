namespace Netafim.WebPlatform.Web.Features.JobFilter
{
    public static class StringExtensions
    {
        public static string Refined(this string orginal)
        {
            orginal = string.IsNullOrWhiteSpace(orginal) ? string.Empty : orginal.Trim();
            return orginal;
        }
    }
}