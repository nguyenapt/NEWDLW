using System.Globalization;
using EPiServer.Globalization;

namespace Dlw.EpiBase.Content.Cms
{
    public class DefaultUserContext : IUserContext
    {
        public CultureInfo CurrentLanguage => ContentLanguage.PreferredCulture;
    }
}