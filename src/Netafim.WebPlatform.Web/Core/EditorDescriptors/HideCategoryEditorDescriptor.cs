using System;
using System.Collections.Generic;
using EPiServer.Cms.Shell.Extensions;
using EPiServer.Core;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace Netafim.WebPlatform.Web.Core.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(CategoryList))]
    public class HideCategoryEditorDescriptor : EditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            base.ModifyMetadata(metadata, attributes);
            if (metadata.PropertyName == "icategorizable_category")
            {
                metadata.ShowForEdit = metadata.FindOwnerContent() is IEnableCategorizable;
            }
        }
    }
}