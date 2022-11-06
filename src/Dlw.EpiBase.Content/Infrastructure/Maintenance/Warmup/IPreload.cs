using EPiServer.Core;

namespace Dlw.EpiBase.Content.Infrastructure.Maintenance.Warmup
{
    /// <summary>
    /// Markinterface to include content during warmup stage during deployment / site restart.
    /// </summary>
    public interface IPreload : IContent
    {
        
    }
}