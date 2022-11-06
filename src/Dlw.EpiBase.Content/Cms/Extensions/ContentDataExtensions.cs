using EPiServer.Core;

namespace Dlw.EpiBase.Content.Cms.Extensions
{
    public static class ContentDataExtensions
    {
        public static ContentReference GetContentReference(this IContentData contentData)
        {
            // block can be used on a page, content area, and then block is wrapped in proxy class
            // -> can be casted to IContent
            // https://world.episerver.com/Blogs/Johan-Bjornfot/Dates1/2012/11/Shared-blocks--IContent/

            var content = contentData as IContent;

            return content?.ContentLink;
        }
    }
}