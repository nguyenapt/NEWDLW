using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Find.Cms;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Core.Services;
using Netafim.WebPlatform.Web.Core.Extensions;
using System.Linq.Expressions;
using System;
using EPiServer.Find;
using Netafim.WebPlatform.Web.Features.DealerLocator.Services;
using LocalizationProvider = DbLocalizationProvider.LocalizationProvider;

namespace Netafim.WebPlatform.Web.Features.DealerLocator
{
    public class DealerLocatorController : ListingBaseBlockController<DealerLocatorBlock, DealerLocatorQueryViewModel, DealerLocatorPage>
    {
        private readonly UrlResolver _urlResolver;
        private readonly IObjectSerialization _objectSerialization;
        private readonly IDealerSettings _dealerSettings;
        private readonly IDealerLocatorService _dealerLocatorService;
        private readonly LocalizationProvider _localizationProvider;
        private const int MaximumRadius = 5000;

        public DealerLocatorController(IContentLoader contentLoader, 
            IPageService pageService, 
            IFindSettings findSettings,
            UrlResolver urlResolver,
            IObjectSerialization objectSerialization,
            IDealerSettings dealerSettings,
            IDealerLocatorService dealerLocatorService, 
            LocalizationProvider localizationProvider) 
            : base(contentLoader, pageService, findSettings)
        {
            this._urlResolver = urlResolver;
            this._objectSerialization = objectSerialization;
            this._dealerSettings = dealerSettings;
            this._dealerLocatorService = dealerLocatorService;
            _localizationProvider = localizationProvider;
        }

        public override ActionResult Index(DealerLocatorBlock currentContent)
        {
            return PartialView("_dealerLocator", currentContent);
        }

        protected override ActionResult PopulateView(DealerLocatorQueryViewModel query)
        {
            var composer = this.GetQueryComposer(query);

            if (composer == null)
                throw new Exception($"Can not find the composer that's satisfied with type {nameof(DealerLocatorBlock)}");

            IContentResult<ICanBeSearched> searchResult = null;
            GeoLocation searchLocator = query.HasGeoLocation() ? new GeoLocation(query.Latitude.Value, query.Longtitude.Value) : null;

            int step = 0;
            while(!StopSearchRecursiveCondition(searchResult, step))
            {
                query.RadiusSearch = this._dealerSettings.DealerRadius * (++step);

                searchResult = this._dealerLocatorService.GetDealers(composer.Compose(query).Expression, searchLocator);
            }

            var dealerLocators = CreateDealerGroups(searchResult);

            return Json(_objectSerialization.ToJson(dealerLocators.OrderBy(m => m.Type).ToArray()));
        }

        private bool StopSearchRecursiveCondition(IContentResult<ICanBeSearched> contentResult, int step)
        {
            if (contentResult == null)
                return false;

            if (this._dealerSettings.MinimalDealersWhenSearchByLocation <= contentResult.TotalMatching)
                return true;

            if (this._dealerSettings.DealerRadius <= 0)
                return true;

            return this._dealerSettings.DealerRadius * step >= MaximumRadius;
        }

        private List<DealerGroupViewModel> CreateDealerGroups(IContentResult<ICanBeSearched> searchResult)
        {
            var dealerLocators = new List<DealerGroupViewModel>();

            var dealerPages = searchResult != null && searchResult.Items != null && searchResult.Any()
                ? searchResult?.Items?.OfType<DealerLocatorPage>()
                : Enumerable.Empty<DealerLocatorPage>();

            var dealerGroups = dealerPages.GroupBy(d => d.ParentLink.ID).ToDictionary(d => d.Key, g => g.ToList());

            foreach (var dealerGroup in dealerGroups)
            {
                var container = this.ContentLoader.Get<DealerLocatorContainerPage>(new ContentReference(dealerGroup.Key));
                if (container != null)
                {
                    var containerViewModel = MapToViewModel(container);
                    containerViewModel.Dealers.AddRange(dealerGroup.Value.Select(MapToViewModel));

                    dealerLocators.Add(containerViewModel);
                }
            }

            return dealerLocators;
        }

        private DealerGroupViewModel MapToViewModel(DealerLocatorContainerPage dealerContainer)
        {
            return new DealerGroupViewModel
            {
                Name = dealerContainer.Name,
                Type = MapDealerLevelToLabel(dealerContainer.Level),
                Color = dealerContainer.Color.GetDescription(),
                Pin = ResolveImageUrl(dealerContainer,m => dealerContainer.PinIcon),
                Logo = ResolveImageUrl(dealerContainer,m => dealerContainer.LevelIcon),
                Dealers = new List<DealerDetailViewModel>()
            };
        }

        private string MapDealerLevelToLabel(DealerLevel level)
        {
            switch (level.GetDescription())
            {
                case DealerConstants.PartnerDescription: return _localizationProvider.GetString(() => Labels.Partner);
                case DealerConstants.SecondaryDescription: return _localizationProvider.GetString(() => Labels.Secondary);
                case DealerConstants.ThirdaryDescription: return _localizationProvider.GetString(() => Labels.Thirdary);
                default: return string.Empty;
            }
        }

        private string ResolveImageUrl(IContentData contentData, Expression<Func<IContentData, ContentReference>> imageExpression)
        {
            var image = imageExpression.Compile().Invoke(contentData);
            return !ContentReference.IsNullOrEmpty(image) ? _urlResolver.GetUrl(image).ImageUrl(imageExpression) : string.Empty;
        }

        private DealerDetailViewModel MapToViewModel(DealerLocatorPage dealer)
        {
            return new DealerDetailViewModel()
            {
                Logo = _urlResolver.GetUrl(dealer.Logo),
                Address = dealer.Address,
                Direction = dealer.Direction,
                Email = dealer.Email,
                Latitude = dealer.Latitude,
                Longtitude = dealer.Longtitude,
                Name = dealer.DealerName,
                Tel = dealer.Phone,
                Website = dealer.Website
            };
        }
    }
}