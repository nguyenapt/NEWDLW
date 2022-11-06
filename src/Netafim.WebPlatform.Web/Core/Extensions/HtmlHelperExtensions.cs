using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using EPiServer.Core;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;
using Netafim.WebPlatform.Web.Core.Services;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.Events;
using Netafim.WebPlatform.Web.Features.FormContainerBlock;
using Netafim.WebPlatform.Web.Features._Shared.ViewModels;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Editors;

namespace Netafim.WebPlatform.Web.Core.Extensions
{
    public static class HtmlHelperExtensions
    {
        private static readonly ILogger _logger = LogManager.GetLogger(typeof(HtmlHelperExtensions));

        public static MvcHtmlString RenderAnchor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, IComponent>> expression)
            where TModel : class
        {
            var model = helper?.ViewData?.Model;

            if (model == null)
                return MvcHtmlString.Empty;

            var baseBlock = expression.Compile().Invoke(model);

            return MvcHtmlString.Create($"id={baseBlock.AnchorId}");
        }

        public static MvcHtmlString RenderContentAreaAsGrid<T>(this HtmlHelper<T> helper, Expression<Func<T, ContentArea>> contentAreaExpression,
            int itemPerRow = 3, string columnCss = "col col-md-4", string rowCss = "row", MvcHtmlString editAttributes = null)
        {
            var contentArea = contentAreaExpression?.Compile().Invoke(helper.ViewData.Model);

            if (contentArea != null)
            {
                var contentItems = contentArea?.FilteredItems?.Select(item => item.GetContent());

                var totalRows = (int)Math.Ceiling((double)contentItems.Count() / itemPerRow);

                var gridViewBuilder = new StringBuilder();

                for (int i = 0; i < totalRows; i++)
                {
                    gridViewBuilder.Append(RenderRowContent(helper, itemPerRow, columnCss, contentItems, i, rowCss, editAttributes));
                }

                return MvcHtmlString.Create(gridViewBuilder.ToString());
            }

            return MvcHtmlString.Empty;
        }

        private static StringBuilder RenderRowContent<T>(HtmlHelper<T> helper, int itemPerRow, string columnCss,
            IEnumerable<IContent> contentItems, int rowIndex, string rowCss, MvcHtmlString editAttributes)
        {
            var rowContent = new StringBuilder()
                .Append($"<div class='{rowCss}' {editAttributes.ToHtmlString()} >");

            foreach (var item in contentItems.Skip(rowIndex * itemPerRow).Take(itemPerRow))
            {
                rowContent.Append(RenderColumContent(helper, m => item, columnCss));
            }

            return rowContent.Append("</div>");
        }

        private static TagBuilder RenderColumContent<T>(HtmlHelper<T> helper, Expression<Func<T, IContent>> expression, string columnCss)
        {
            var column = new TagBuilder("div");
            column.Attributes["class"] = columnCss;

            var partial = helper.PropertyFor(expression);
            column.InnerHtml = partial != null ? partial.ToHtmlString() : string.Empty;
            return column;
        }
        public static string RenderBackgroundColor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, BaseBlock>> expression)
          where TModel : class
        {
            return GetBackgroundColor(helper, expression, false);
        }
        public static string RenderOppositeBackgroundColor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, BaseBlock>> expression)
         where TModel : class
        {
            return GetBackgroundColor(helper, expression, true);
        }
        private static string GetBackgroundColor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, BaseBlock>> expression, bool isReverseColor) where TModel : class
        {
            var model = helper?.ViewData?.Model;
            if (model == null) return string.Empty;
            var baseBlock = expression.Compile().Invoke(model);
            var bgColor = baseBlock.BackgroundColor;
            if (isReverseColor)
            {
                bgColor = bgColor.Equals(BackgroundColor.White) ? BackgroundColor.Grey : BackgroundColor.White;
            }
            return bgColor == BackgroundColor.Grey ? "gray-bg" : "white-bg";
        }

        public static string RenderBootstrapSpan(this HtmlHelper helper, ScreenSize screenMode, int columnsCount)
        {
            var bootstrapGridColumns = 12.00;
            var spanColumn = columnsCount > 0 ? (int)Math.Ceiling(bootstrapGridColumns / columnsCount) : 1;

            return string.Format("col-{0}-{1}", GetBootstrapSignature(screenMode), spanColumn);
        }

        private static string GetBootstrapSignature(ScreenSize screenMode)
        {
            switch (screenMode)
            {
                case ScreenSize.Large: return "lg";
                case ScreenSize.Medium: return "md";
                case ScreenSize.Small: return "sm";
                default: return "xs";
            }
        }

        public static MvcHtmlString RenderWatermark<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, BaseBlock>> expression, string extendCss = null) where TModel : class
        {
            var model = helper?.ViewData?.Model;
            if (model == null)
                return MvcHtmlString.Empty;

            var baseBlock = expression.Compile().Invoke(model);
            var watermark = baseBlock.Watermark;
            
            return RenderWatermark(helper, watermark, baseBlock.OnParallaxEffect, extendCss);
        }

        public static MvcHtmlString RenderWatermark(this HtmlHelper helper, string watermark, bool onParallaxEffect, string extendCss = null)
        {
            if (string.IsNullOrEmpty(watermark))
                return MvcHtmlString.Empty;

            var style = onParallaxEffect ? "bg-text has-parallax" : "bg-text";
            var divTag = new TagBuilder("div");
            divTag.Attributes["class"] = string.IsNullOrEmpty(extendCss) ? style : $"{style} {extendCss}";
            divTag.SetInnerText(watermark);

            return MvcHtmlString.Create(divTag.ToString());
        }

        public static string ToDateRangeString(this HtmlHelper helper, DateTime from, DateTime to)
        {
            var eventTimeDisplays = ServiceLocator.Current.GetAllInstances<IEventTimeRangeDisplay>();
            if (eventTimeDisplays == null) throw new Exception("Cannot find service for display time range of event.");

            var eventTimeDisplay = eventTimeDisplays.FirstOrDefault(x => x.Satified(from, to));
            if (eventTimeDisplay == null) throw new Exception("Cannot find suitable service for display time range of event.");

            return eventTimeDisplay.ToDateTimeRange(from, to);
        }

        public static string RenderPopupIdAttribute(this HtmlHelper helper, string popupId)
        {
            return $"data-popup-id={popupId}";
        }

        public static MvcHtmlString RenderDisabledReason(this HtmlHelper helper, IContent content, string reason)
        {
            return helper.Partial("_componentDisabled", new DisabledComponentViewModel
            {
                Content = content,
                Reason = reason
            });
        }

        public static MvcHtmlString RenderFloatingNavigationText<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, BaseBlock>> expression, string extendCss = null) where TModel : class
        {
            var model = helper?.ViewData?.Model;
            if (model == null)
                return MvcHtmlString.Empty;

            var baseBlock = expression.Compile().Invoke(model);
            var verticalText = baseBlock.VerticalText;
            if(string.IsNullOrEmpty(verticalText))
                return MvcHtmlString.Empty;

            var divTag = new TagBuilder("div");
            divTag.Attributes["class"] = $"vertical-text {extendCss}";
            divTag.SetInnerText(verticalText);

            return MvcHtmlString.Create(divTag.ToString());
        }

        public static MvcHtmlString RenderHiddenContainerIdentity(this HtmlHelper helper,
            IEmailTemplateSettings model)
        {
            var divTag = new StringBuilder();
            divTag.Append("<div public-identity-metadata>");
            divTag.AppendFormat("<input type='hidden' public-identity-value='{0}' />", ((IContent)model).ContentLink);
            divTag.Append("</div>");

            return MvcHtmlString.Create(divTag.ToString());
        }
    }
}