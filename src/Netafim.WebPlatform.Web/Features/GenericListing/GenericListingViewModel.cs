using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Templates;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.GenericListing
{
    public class GenericListingViewModel
    {
        public IEnumerable<IPreviewable> Result { get; }

        public GenericListingBlock Block { get; }

        public GenericListingViewModel(GenericListingBlock block, IEnumerable<IPreviewable> result)
        {
            this.Block = block;
            this.Result = result;
        }

        public bool HasResult() => this.Result != null && this.Result.Any();

        public int TotalItems => this.Result != null ? this.Result.Count() : 0;
    }
}