using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Core.Extensions
{
    public static class IComponentExtensions
    {
        public static string GetComponentName(this IComponent component, string title)
        {
            if (!string.IsNullOrEmpty(title))
                return title;

            return GetComponentName(component);
        }

        public static string GetComponentName(this IComponent component)
        {
            var pageContent = component as PageData;
            return pageContent != null ? pageContent.PageName : (component as IContent) == null ? string.Empty : ((IContent)component).Name;
        }
    }
}