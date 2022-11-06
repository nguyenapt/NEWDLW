using System;
using System.Linq;
using EPiServer;
using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Features.Settings
{
    /// <summary>
    /// Base class for components that store their settings on the settings page.
    /// </summary>
    public abstract class PageDataSettings
    {
        protected SettingsPage SettingsPage { get; }

        protected PageDataSettings(IContentRepository contentRepository)
        {
            var settingsPage = contentRepository.GetChildren<SettingsPage>(ContentReference.RootPage).SingleOrDefault();

            SettingsPage = settingsPage;// ?? throw new Exception("No settings page configured.");
        }
    }   
}