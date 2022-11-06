using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Features.RichText.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.RichText
{
    public class RichTextController : BlockController<RichTextContainerBlock>
    {
        private readonly IContentRepository _contentRepository;

        public RichTextController(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        public override ActionResult Index(RichTextContainerBlock currentContent)
        {
            var viewModel = new RichTextContainerViewModel()
            {
                Block = currentContent,
            };

            return PartialView("_richText", viewModel);
        }
    }
}