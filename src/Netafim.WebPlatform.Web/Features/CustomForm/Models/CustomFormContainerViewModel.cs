using Netafim.WebPlatform.Web.Features.RichText.Models;

namespace Netafim.WebPlatform.Web.Features.CustomForm.Models
{
    public class CustomFormContainerViewModel : RichTextComponentViewModel<CustomFormContainerBlock>
    {
        public bool HasRichTextWrapper => this.Parent != null;

        public CustomFormContainerViewModel(CustomFormContainerBlock block, RichTextContainerViewModel container) 
            : base(block, container)
        {
        }
    }
}