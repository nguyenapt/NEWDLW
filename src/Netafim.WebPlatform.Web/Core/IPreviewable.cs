using EPiServer.Core;
using EPiServer.Shell;

namespace Netafim.WebPlatform.Web.Core
{
    /// <summary>
    /// Enables content to be previewed through other components.
    /// Eg crop page in related content component.
    /// </summary>
    public interface IPreviewable : IContent
    {
        string Title { get; }

        XhtmlString Description { get; }

        ContentReference Thumnbail { get; }
    }

    // Enable dropping content based on interface in content area
    [UIDescriptorRegistration]
    public class IPreviewableUIDescriptor : UIDescriptor<IPreviewable>
    { }
}