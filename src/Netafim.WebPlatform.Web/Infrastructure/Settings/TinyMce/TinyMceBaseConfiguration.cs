using Dlw.EpiBase.Content.Infrastructure.TinyMce;
using System;

namespace Netafim.WebPlatform.Web.Infrastructure.Settings.TinyMce
{
    public class TinyMceBaseConfiguration : TinyMceSettingBase, ITinyMceSettings
    {
        public TinyMceBaseConfiguration() : base()
        {
            DisplayName = "Basic Tinymce settings";
            Id = new Guid("8cc4b58a-6580-45f3-ab9c-df2ed5adc82e");
            NonVisualPlugins = null;

            Toolbars = new[]
            {
               new []
               {
                   ToolbarItems.EPILINK, ToolbarItems.UNLINK, ToolbarItems.ImageItems.IMAGE, ToolbarItems.ImageItems.EPIIMAGEEIDTOR, ToolbarItems.EPISERVERPERSONALIZEDCONTENT,
                   ToolbarItems.SEPARATOR, ToolbarItems.CUT, ToolbarItems.COPY, ToolbarItems.PASTE, ToolbarItems.PASTETEXT, ToolbarItems.PASTEWORD,
                   ToolbarItems.SEPARATOR,ToolbarItems.SELECTALL, ToolbarItems.REPLACE
               },
               new []
               {
                   ToolbarItems.BOLD, ToolbarItems.ITALIC,
                   ToolbarItems.SEPARATOR, ToolbarItems.JUSTIFY_LEFT, ToolbarItems.JUSTIFY_CENTER, ToolbarItems.JUSTIFY_RIGHT,
                   ToolbarItems.SEPARATOR,ToolbarItems.BULLIST,ToolbarItems.NUMLIST,
                   ToolbarItems.SEPARATOR,ToolbarItems.STYLESELECT, ToolbarItems.UNDO, ToolbarItems.REDO, ToolbarItems.SEARCH
               }
           };
        }
    }
}