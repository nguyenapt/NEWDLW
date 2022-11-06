using System.Collections;
using EPiServer.Logging;
using EPiServer.Cms.Shell;
using EPiServer.Core;
using EPiServer.Core.PropertySettings;
using EPiServer.DataAbstraction;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using EPiServer.Web.Hosting;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Collections.Generic;

namespace Dlw.EpiBase.Content.Infrastructure.TinyMce
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    [InitializableModule]
    public class TinyMceSettingsInitialization : IInitializableModule
    {
        private Injected<IPropertySettingsRepository> _propertySettingsRepository;
        private Injected<IContentTypeRepository> _contentTypeRepository;
        private Injected<IPropertyDefinitionRepository> _propertyDefinitionRepository;

        private ILogger _logger = LogManager.GetLogger(typeof(TinyMceSettingsInitialization));

        public void Initialize(InitializationEngine context)
        {
            AddVppForTinyMcePluginFolder();

            SyncTinyMceSettings();
        }

        public void Uninitialize(InitializationEngine context) { }

        #region Private & Sub methods/members

        private void AddVppForTinyMcePluginFolder()
        {
            var nameValueCollection = new NameValueCollection();
            nameValueCollection.Add("virtualPath", string.Format("~{0}", Paths.ToClientResource(typeof(CmsModule), "ClientResources/Editor/tiny_mce/plugins")));
            nameValueCollection.Add("physicalPath", Path.Combine(HttpRuntime.AppDomainAppPath, @"Util\Editor\tinymce\plugins"));
            nameValueCollection.Add("bypassAccessCheck", "true");
            HostingEnvironment.RegisterVirtualPathProvider(new VirtualPathNonUnifiedProvider("Bisnode.TinyMCE.Plugins", nameValueCollection));
        }

        private void SyncTinyMceSettings()
        {
            foreach(var xhtmlProperty in ScanAllXhtmlProperties())
            {
                ITinyMceSettings settings = GetSettingsFromAttribute(xhtmlProperty.Property);
                if (settings == null)
                {
                    continue;
                }

                PropertySettingsContainer container = settings.GetOrCreateSettingContainer(_propertySettingsRepository.Service);

                // if property was removed, then we cannot use First().
                var property = xhtmlProperty.ContentType.PropertyDefinitions.FirstOrDefault(x => x.Name == xhtmlProperty.Property.Name);
                if (property != null)
                    SaveSettingsToProperty(property, settings);
            }
        }

        private IEnumerable<XhtmlPropertyReference> ScanAllXhtmlProperties()
        {
            foreach (var contentType in _contentTypeRepository.Service.List())
            {
                if (contentType == null || contentType.ModelType == null)
                {
                    continue;
                }

                var xhtmlStringType = typeof(XhtmlString);
                foreach (var propertyInfo in contentType.ModelType.GetProperties())
                {
                    if (propertyInfo.PropertyType == xhtmlStringType)
                    {
                        yield return new XhtmlPropertyReference(contentType, propertyInfo);
                    }
                }
            }
        }

        private ITinyMceSettings GetSettingsFromAttribute(PropertyInfo propertyInfo)
        {
            var settingsAttribute = propertyInfo.GetCustomAttribute<TinyMceSettingsAttribute>();

            if (settingsAttribute == null) return null;

            var settings = Activator.CreateInstance(settingsAttribute.SettingsType) as ITinyMceSettings;
            if (settings == null)
                _logger.Error($"Defined TinyMceSettings type of {propertyInfo.Name} of type {propertyInfo.Module?.Name} is not implementing ITinyMceSettings");

            return settings;
        }

        private void SaveSettingsToProperty(PropertyDefinition property, ITinyMceSettings settings)
        {
            var writableProperty = property.CreateWritableClone();
            writableProperty.SettingsID = settings.Id;
            _propertyDefinitionRepository.Service.Save(writableProperty);
        }

        private class XhtmlPropertyReference
        {
            public XhtmlPropertyReference(ContentType contentType, PropertyInfo property)
            {
                this.ContentType = contentType;
                this.Property = property;
            }

            public ContentType ContentType { get; }
            public PropertyInfo Property { get; }
        }

        #endregion
    }
}