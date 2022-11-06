using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dlw.EpiBase.Content.Cms.Extensions;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Core;

namespace Netafim.WebPlatform.Web.Features.RelatedContent
{
    public class RelatedContentController : BlockController<RelatedContentBlock>
    {
        private readonly IContentRepository _contentRepository;
        private readonly LocalizationService _localizationService;

        public RelatedContentController(IContentRepository contentRepository,
            LocalizationService localizationService)
        {
            _contentRepository = contentRepository;
            _localizationService = localizationService;
        }

        public override ActionResult Index(RelatedContentBlock currentBlock)
        {
            var model = new RelatedContentViewModel
            {
                Block = currentBlock,
                Items = currentBlock.Items?.FilteredItems?.Select(item => MapToViewModel(item))
            };

            return PartialView("_relatedContent", model);
        }

        private RelatedContentItemViewModel MapToViewModel(ContentAreaItem item)
        {
            var previewable = _contentRepository.Get<IPreviewable>(item.ContentLink);

            return new RelatedContentItemViewModel()
            {
                Content = previewable,
                Tags = ExtractTags(previewable)
            };
        }

        private IEnumerable<string> ExtractTags(IContent content)
        {
            var casted = content as ICategorizable;

            if (casted == null) return new string[] { };

            var tags = new List<string>();
            foreach (var categoryId in casted.Category)
            {
                var name = _localizationService.TranslateCategory(categoryId);

                tags.Add(name);
            }

            return tags;
        }
    }

    public class RelatedContentItemViewModel
    {
        public bool IsIncomplete => string.IsNullOrEmpty(Content.Title) || Content.Thumnbail == null;

        public IPreviewable Content { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}