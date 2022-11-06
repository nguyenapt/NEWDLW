using EPiServer.Core;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.GenericOverview
{
    public class GenericOverviewBlockModel
    {
        public GenericOverviewBlockModel(GenericOverviewBlock block)
        {
            Block = block;
        }
        public List<List<IContent>> ItemRows { get; set; }
        public GenericOverviewBlock Block { get; set; }
        public bool IsListOfThumnailItems { get; set; }
    }
}