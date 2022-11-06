using EPiServer.Core;
using EPiServer.Shell;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.RichText.Models
{
    public interface IRichTextColumnComponent : IContentData
    {
        string Title { get; }
    }

    [UIDescriptorRegistration]
    public class RichTextComponentDescriptor : UIDescriptor<IRichTextColumnComponent>
    {

    }
}