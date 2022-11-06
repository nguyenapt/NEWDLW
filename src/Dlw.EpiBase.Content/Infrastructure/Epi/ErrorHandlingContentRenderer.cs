using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Dlw.EpiBase.Content.Cms;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Logging;
using EPiServer.Web.Mvc;

namespace Dlw.EpiBase.Content.Infrastructure.Epi
{
    // source: based on AlloyDemoKit demo site

    /// <summary>
    /// Wraps an MvcContentRenderer and adds error handling to ensure that blocks and other content
    /// rendered as parts of pages won't crash the entire page if a non-critical exception occurs while rendering it.
    /// </summary>
    /// <remarks>
    /// Prints an error message for editors so that they can easily report errors to developers.
    /// </remarks>
    public class ErrorHandlingContentRenderer : IContentRenderer
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(ErrorHandlingContentRenderer));

        private readonly MvcContentRenderer _mvcRenderer;
        private readonly IShellContext _shellContext;

        public ErrorHandlingContentRenderer(MvcContentRenderer mvcRenderer, IShellContext shellContext)
        {
            _mvcRenderer = mvcRenderer;
            _shellContext = shellContext;
        }

        /// <summary>
        /// Renders the contentData using the wrapped renderer and catches common, non-critical exceptions.
        /// </summary>
        public void Render(HtmlHelper helper, PartialRequest partialRequestHandler, IContentData contentData, TemplateModel templateModel)
        {
            try
            {
                _mvcRenderer.Render(helper, partialRequestHandler, contentData, templateModel);
            }
            catch (Exception e) when (!helper.ViewContext.HttpContext.IsCustomErrorEnabled)
            {
                var errorModel = new ContentRenderingErrorModel(contentData, e);
                _logger.Error($"Could not render component '{errorModel.ContentName} / {errorModel.ContentTypeName}'.", e);

                throw;
            }
            catch (Exception ex)
            {
                HandlerError(helper, contentData, ex);
            }
        }

        private void HandlerError(HtmlHelper helper, IContentData contentData, Exception renderingException)
        {
            var errorModel = new ContentRenderingErrorModel(contentData, renderingException);

            _logger.Error($"Could not render component '{errorModel.ContentName} / {errorModel.ContentTypeName}'.", renderingException);

            if (_shellContext.PageIsInEditMode)
            {
                helper.RenderPartial("_ErrorEdit", errorModel);

                return;
            }

            helper.RenderPartial("_Error", errorModel);
        }
    }
}