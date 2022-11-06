using EPiServer.Shell;
using EPiServer.Shell.ViewComposition;

namespace Netafim.WebPlatform.Web.Core.Bynder
{
    [Component]
    public class DamComponent : ComponentDefinitionBase
    {
        public DamComponent() : base("epi-cms/component/Media") // epi-cms/component/Media | epi-cms/widget/HierarchicalList
        {
            Categories = new string[] { "content" };
            Title = "DAM";
            Description = "Bynder assets";
            SortOrder = 1000;
            PlugInAreas = new[] { PlugInArea.AssetsDefaultGroup };
            Settings.Add(new Setting("repositoryKey", BynderProvider.Key));
        }
    }
}