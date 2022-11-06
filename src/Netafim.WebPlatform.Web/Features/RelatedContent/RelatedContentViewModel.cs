using System.Collections.Generic;
using Netafim.WebPlatform.Web.Core;

namespace Netafim.WebPlatform.Web.Features.RelatedContent
{
    public class RelatedContentViewModel
    {
        public RelatedContentBlock Block { get; set; }

        public IEnumerable<RelatedContentItemViewModel> Items { get; set; }
    }
}