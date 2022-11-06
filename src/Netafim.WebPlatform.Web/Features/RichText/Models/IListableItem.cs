using EPiServer.Shell;

namespace Netafim.WebPlatform.Web.Features.RichText.Models
{
    /// <summary>
    /// Define the content that can be listed in another block (in content area of another one...)
    /// </summary>
    public interface IListableItem
    {

    }
    
    [UIDescriptorRegistration]
    public class ListableItemDescriptor : UIDescriptor<IListableItem>
    {

    }
}