using System;
using System.Linq;
using System.Web.Mvc;
using Dlw.EpiBase.Content.Infrastructure.Mvc;
using EPiServer;
using EPiServer.Core;

namespace Dlw.EpiBase.Content.Infrastructure.Epi
{
    /// <summary>
    /// Default partial block controller to support feature based folder structure for block partial views.
    /// </summary>
    public class DefaultBlockPartialController : BasePartialController<BlockData>
    {
        public override ActionResult Index(BlockData currentBlock)
        {
            var fullPath = MapToFullPath(currentBlock);

            var defaultViewName = fullPath;

            return PartialView(defaultViewName, currentBlock);
        }

        private string MapToFullPath(BlockData currentBlock)
        {
            var blockType = currentBlock.GetOriginalType(); // use OriginalType of proxy

            var partialViewName = $"_{blockType.Name.Substring(0, 1).ToLowerInvariant()}{blockType.Name.Substring(1)}";

            // find feature
            var featureName = blockType.Namespace.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Last();

            return $"{FeatureRazorViewEngine.FeatureLocation}/{featureName}/Views/{partialViewName}.cshtml";
        }
    }
}