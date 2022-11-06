using Netafim.WebPlatform.Web.Core.Templates;
using System.Linq;

namespace Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels
{
    /// <summary>
    /// Viewmodel for Block with paginable List<IContent>.
    /// </summary>
    /// <typeparam name="TBlock"></typeparam>
    /// <typeparam name="TContent"></typeparam>
    public class PaginableBlockViewModel<TBlock, TContent> : IPaginableResult<TContent>
        where TBlock : ListingBaseBlock
        where TContent : ICanBeSearched

    {
        public IPagedList<TContent> Result { get; }

        public TBlock CurrentBlock { get; }
        
        public PaginableBlockViewModel(TBlock currentBlock, IPagedList<TContent> result)
        {
            this.CurrentBlock = currentBlock;
            this.Result = result;
        }

        public bool HasSearchResult()
        {
            return this.Result != null && this.Result.Any();
        }
    }
}