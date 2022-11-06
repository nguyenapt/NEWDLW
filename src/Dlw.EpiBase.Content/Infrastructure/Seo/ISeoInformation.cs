using System.Collections.Generic;

namespace Dlw.EpiBase.Content.Infrastructure.Seo
{
    public interface ISeoInformation
    {
        string SeoDescription { get; set; }

        string SeoKeywords { get; set; }

        string SeoPageTitle { get; set; }

        IList<AlternateLink> AlternateLinks { get; set; }
    }
}