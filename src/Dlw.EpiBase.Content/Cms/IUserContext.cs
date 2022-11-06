using System.Globalization;

namespace Dlw.EpiBase.Content.Cms
{
    public interface IUserContext
    {
        CultureInfo CurrentLanguage { get; }
    }
}