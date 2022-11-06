using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Dlw.EpiBase.Content.Infrastructure.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dlw.EpiBase.Content.Infrastructure.Web
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Creates a wrapping div element that defines a component scope.
        /// </summary>
        public static Component BeginTagComponent(this HtmlHelper htmlHelper, string tagName, string componentClassName, object initData = null, object htmlAttributes = null, string webIdName = null)
        {
            var tagBuilder = new TagBuilder(tagName);
            tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            // Explicit attributes, they take precedence over the htmlAttributes.
            tagBuilder.MergeAttribute("data-webid", webIdName ?? componentClassName);
            tagBuilder.MergeAttribute("data-component-class", componentClassName);
            tagBuilder.MergeAttribute("data-component-parm", JsonConvert.SerializeObject(initData,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));

            htmlHelper.ViewContext.Writer.Write(tagBuilder.ToString(TagRenderMode.StartTag));

            return new Component(htmlHelper.ViewContext, x => EndTagComponent(x, tagName));
        }

        internal static void EndTagComponent(ViewContext viewContext, string tagName)
        {
            viewContext.Writer.Write("</{0}>", tagName);
        }

        /// <summary>
        /// Creates a wrapping div element that defines a component scope.
        /// </summary>
        public static Component BeginComponent(this HtmlHelper htmlHelper, string componentName, object initData = null,
            object htmlAttributes = null, string webId = null)
        {
            return htmlHelper.BeginTagComponent("div", componentName, initData, htmlAttributes, webId);
        }

        /// <summary>
        /// Add a rendering tag.
        /// </summary>
        public static TemplateTagContext BeginTemplateTag(this HtmlHelper helper, string tag)
        {
            return new TemplateTagContext(helper, tag);
        }

        /// <summary>
        /// Adds an asterisk (*) after the labelText if the property is required
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString LabelForRequired<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            return LabelHelper(html,
                ModelMetadata.FromLambdaExpression(expression, html.ViewData),
                ExpressionHelper.GetExpressionText(expression), null, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        private static MvcHtmlString LabelHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, string labelText, IDictionary<string, object> htmlAttributes)
        {
            if (string.IsNullOrEmpty(labelText))
            {
                labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            }

            if (string.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            bool isRequired = false;

            if (metadata.ContainerType != null)
            {
                isRequired = metadata.ContainerType.GetProperty(metadata.PropertyName)
                                .GetCustomAttributes(typeof(RequiredAttribute), false)
                                .Length == 1;
            }

            TagBuilder tag = new TagBuilder("label");

            tag.Attributes.Add("for",TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));

            //Not technically a merge, it replaces existing attributes. 
            tag.MergeAttributes(htmlAttributes, true);

            if (isRequired)
            {
                if (tag.Attributes.ContainsKey("class"))
                {
                    tag.Attributes["class"] = $"{tag.Attributes["class"]} c-form-label-required";
                }
                else
                {
                    tag.Attributes.Add("class", "c-form-label-required");
                }
                tag.SetInnerText(labelText);
            }
            else
            {
                tag.SetInnerText(labelText);
            }
                
            var output = tag.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(output);
        }

        public static string RenderPartialToString(string viewName, object model, ControllerContext controllerContext, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            viewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var viewContext = new ViewContext(controllerContext, viewResult.View, viewData, tempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}