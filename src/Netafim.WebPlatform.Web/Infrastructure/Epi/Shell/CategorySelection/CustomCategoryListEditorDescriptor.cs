using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.CategorySelection
{
    [EditorDescriptorRegistration(TargetType = typeof(CategoryList), EditorDescriptorBehavior = EditorDescriptorBehavior.OverrideDefault)]
    public class CustomCategoryListEditorDescriptor : CategoryListEditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            var attributeList = attributes as IList<Attribute> ?? attributes.ToList();

            base.ModifyMetadata(metadata, attributeList);
            var categorySelectionAttribute =
                attributeList.OfType<CategorySelectionAttribute>().FirstOrDefault();

            if (categorySelectionAttribute != null)
            {
                metadata.EditorConfiguration["root"] =
                    categorySelectionAttribute.GetRootCategoryId();
                return;
            }

            var contentTypeCategorySelectionAttribute =
                metadata.ContainerType.GetCustomAttribute<CategorySelectionAttribute>(true);

            if (contentTypeCategorySelectionAttribute != null)
            {
                metadata.EditorConfiguration["root"] =
                    contentTypeCategorySelectionAttribute.GetRootCategoryId();
            }
        }
    }
}