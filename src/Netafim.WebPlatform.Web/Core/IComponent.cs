using EPiServer.Shell;

namespace Netafim.WebPlatform.Web.Core
{
    /// <summary>
    /// Interface to mark a block or page as a component
    /// </summary>
    public interface IComponent
    {
        string AnchorId { get; set; }
        string ComponentName { get; }
    }

    // Enable dropping content based on interface in content area
    [UIDescriptorRegistration]
    public class IComponentUIDescriptor : UIDescriptor<IComponent>
    { }
}