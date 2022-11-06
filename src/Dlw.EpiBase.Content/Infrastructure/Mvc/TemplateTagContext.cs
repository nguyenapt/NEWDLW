using System;
using System.Web.Mvc;

namespace Dlw.EpiBase.Content.Infrastructure.Mvc
{
    public class TemplateTagContext : IDisposable
    {
        private readonly HtmlHelper _helper;
        private readonly object _originalTag;

        public TemplateTagContext(HtmlHelper helper, string tag)
        {
            _helper = helper;
            _originalTag = _helper.ViewContext.ViewData["Tag"];
            _helper.ViewContext.ViewData["Tag"] = tag;
        }

        public void Dispose()
        {
            _helper.ViewContext.ViewData["Tag"] = _originalTag;
        }
    }
}
