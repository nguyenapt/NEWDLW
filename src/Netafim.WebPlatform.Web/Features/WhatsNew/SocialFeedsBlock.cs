using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Netafim.WebPlatform.Web.Core.Templates;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.WhatsNew
{
    [ContentType(DisplayName = "Social feed block",
        GroupName = GroupNames.Overview,
        Description = "Component that display the list of social feeds.",
        GUID = "6e6f0632-fce6-4e2e-a94e-748bbb8403e5")]
    public class SocialFeedsBlock : ItemBaseBlock
    {      
        [CultureSpecific]
        public virtual string Title { get; set; }
        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<SocialChannelSetting>))]
        [Display( Name ="Social Channel Settings")]
        public virtual IList<SocialChannelSetting> SocialChannelsSettings { get; set; }
    }
}