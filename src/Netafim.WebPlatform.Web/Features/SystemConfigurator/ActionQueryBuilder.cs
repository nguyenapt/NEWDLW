using EPiServer;
using EPiServer.Core;
using EPiServer.Logging;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator
{
    public interface IActionHandler
    {
        bool IsSatisfied(ActionContext actionContext);

        string TakeAction(ActionContext actionContext);
    }
    
    public abstract class ActionQueryBaseBuilder<TPipeline, TActionContext> : IActionHandler
        where TPipeline : PipelineBaseBlock
        where TActionContext : ActionContext
    {
        protected readonly IContentLoader ContentLoader;
        protected readonly UrlResolver UrlResolver;
        protected readonly ICipher Cipher;

        protected ActionQueryBaseBuilder(IContentLoader contentLoader,
            UrlResolver urlResolver,
            ICipher cipher)
        {
            this.ContentLoader = contentLoader;
            this.UrlResolver = urlResolver;
            this.Cipher = cipher;
        }

        public virtual string TakeAction(ActionContext actionContext)
        {
            var queryParams = new List<string>();

            foreach(var prop in actionContext.GetType().GetProperties())
            {
                if(prop.Name != nameof(ActionContext.ContentId)) // No need to add blockId to the query string
                {
                    var value = prop.GetValue(actionContext);

                    // Simple is hard-code. 
                    // TODO: add ther encryptor attribute to mark as encryptor
                    if(prop.Name == nameof(StarterActionContext.ClientEmail) || prop.Name == nameof(StarterActionContext.ClientName))
                    {                        
                        // Need to be encrypt
                        value = this.Cipher.Encipher(value.ToString(), StarterActionContext.Shift);
                    }

                    queryParams.Add($"{prop.Name.ToLower()}={value}");
                }
            }

            var pipelineBlock = this.ContentLoader.Get<IContent>(new ContentReference(actionContext.ContentId)) as TPipeline;

            var nextUrl = this.UrlResolver.GetUrl(pipelineBlock.Next);
                        
            return $"{nextUrl}?{string.Join("&", queryParams)}";
        }

        public virtual bool IsSatisfied(ActionContext actionContext)
        {
            if (actionContext == null || actionContext.ContentId <= 0)
                return false;

            return this.ContentLoader.Get<IContent>(new ContentReference(actionContext.ContentId)) is TPipeline;
        }
    }
}