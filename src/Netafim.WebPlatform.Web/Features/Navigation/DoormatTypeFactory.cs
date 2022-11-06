using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Navigation
{
    public class DoormatTypeFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                    new SelectItem { Text = "", Value =DoormatType.None },
                    new SelectItem { Text = "Image Column Only", Value =DoormatType.ImageColumnOnly },
                    new SelectItem { Text = "Text Column Only", Value = DoormatType.TextColumnOnly },
                    new SelectItem { Text = "Mixed Image and Text Column", Value = DoormatType.MixedImageAndTextColumn }
            };
        }
    }
}