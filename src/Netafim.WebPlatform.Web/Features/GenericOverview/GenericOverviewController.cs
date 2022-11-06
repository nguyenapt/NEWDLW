using EPiServer.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.GenericOverview
{
    public class GenericOverviewController : BlockController<GenericOverviewBlock>
    {
        protected readonly IEnumerable<IGenericOverviewModelFactory> _genericOverviewFactories;

        public GenericOverviewController(IEnumerable<IGenericOverviewModelFactory> genericOverviewFactories)
        {
            _genericOverviewFactories = genericOverviewFactories;
        }

        public override ActionResult Index(GenericOverviewBlock currentContent)
        {
            var genericOverviewModelFactory = this._genericOverviewFactories.FirstOrDefault(f => f.IsSatisfied(currentContent));
            if (genericOverviewModelFactory == null) throw new Exception("Can not find any satisfied Generic Overview Model Factory.");
            return PartialView("GenericOverviewBlock", genericOverviewModelFactory.Create(currentContent));
        }
    }
}