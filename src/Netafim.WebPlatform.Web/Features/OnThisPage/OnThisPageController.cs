using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;

namespace Netafim.WebPlatform.Web.Features.OnThisPage
{
    public class OnThisPageController : BlockController<OnThisPageBlock>
    {
        public override ActionResult Index(OnThisPageBlock currentBlock)
        {
            if (currentBlock == null) throw new ArgumentNullException(nameof(currentBlock));

            var model = new OnThisPageViewModel(currentBlock)
            {
                Components = ListComponents(currentBlock)
            };
            
            return PartialView("_onThisPageBlock", model);
        }

        private IEnumerable<IComponent> ListComponents(OnThisPageBlock currentBlock)
        {
            var contentArea = ControllerContext?.ParentActionViewContext?.ViewData?.Model as ContentArea;
            
            return contentArea.GetFilteredItemsContent<IContent>()
                .OfType<IComponent>()
                .Where(t => ((IContent)t).ContentGuid != ((IContent) currentBlock).ContentGuid);
        }
    }
}
