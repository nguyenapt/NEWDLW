using System;
using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace Netafim.WebPlatform.Web.Core.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(DateTime?), UIHint = "UTC")]
    [EditorDescriptorRegistration(TargetType = typeof(DateTime), UIHint = "UTC")]
    public class UtcDateTimeEditorDescriptor : DateTimeEditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            ClientEditingClass = "netafim/UTCDateTimeTextBox";

            base.ModifyMetadata(metadata, attributes);
        }
    }
}