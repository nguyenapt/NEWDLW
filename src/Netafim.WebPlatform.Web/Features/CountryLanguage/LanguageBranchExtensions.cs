using EPiServer.DataAbstraction;
using System.Globalization;

namespace Netafim.WebPlatform.Web.Features.CountryLanguage
{
    public static class LanguageBranchExtensions
    {
        public static bool IsLocalSite(this LanguageBranch lang)
        {
            return lang?.Culture != null && lang.Culture.Parent != CultureInfo.InvariantCulture;
        }
    }
}