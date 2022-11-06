using Dlw.EpiBase.Content.Infrastructure.TinyMce;
using System;

namespace Netafim.WebPlatform.Web.Infrastructure.Settings.TinyMce
{
    public class BoldTinyMceConfiguration : TinyMceSettingBase, ITinyMceSettings
    {
        public BoldTinyMceConfiguration() : base()
        {
            DisplayName = "Quote Tinymce settings";
            Id = new Guid("7259cb7a-9490-42b9-b4c6-2c328f70ea0e");
            NonVisualPlugins = null;

            Toolbars = new[]
            {
               new []
               {
                   ToolbarItems.BOLD,
               }
           };
        }
    }
}