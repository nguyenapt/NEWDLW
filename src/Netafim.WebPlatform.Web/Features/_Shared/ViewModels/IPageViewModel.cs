namespace Netafim.WebPlatform.Web.Features._Shared.ViewModels
{
    public interface IPageViewModel<out T> where T : Web.Core.Templates.PageBase
    {
        T Current { get; }
    }
}