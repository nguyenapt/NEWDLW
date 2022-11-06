using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Features._Shared.ViewModels
{
    public interface IBlockViewModel<out T> where T : BlockData
    {
        T CurrentBlock { get; }
    }
}