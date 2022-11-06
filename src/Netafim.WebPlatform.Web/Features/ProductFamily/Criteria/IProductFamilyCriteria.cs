using EPiServer.Core;
using EPiServer.Shell;

namespace Netafim.WebPlatform.Web.Features.ProductFamily.Criteria
{
    public interface IProductFamilyProperty :IContentData
    {
        string Key { get; }
        string Value { get; }        
        ContentReference Image { get; set; }
    }
    // Enable dropping content based on interface in content area
    [UIDescriptorRegistration]
    public class IPreviewableUIDescriptor : UIDescriptor<IProductFamilyProperty>
    { }
}