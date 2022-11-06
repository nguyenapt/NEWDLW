using System;
using EPiServer;
using EPiServer.Core;

namespace Dlw.EpiBase.Content.Infrastructure.Epi
{
    public class ContentRenderingErrorModel
    {
        public string ContentName { get; set; }

        public string ContentTypeName { get; set; }

        public Exception Exception { get; set; }

        public ContentRenderingErrorModel(IContentData contentData, Exception exception)
        {
            var content = contentData as IContent;

            ContentName = content != null ? content.Name : string.Empty;

            ContentTypeName = contentData.GetOriginalType().Name;

            Exception = exception;
        }
    }
}