using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Web.Mvc;

// source: http://www.jondjones.com/learn-episerver-cms/episerver-developers-guide/episerver-blocks/how-to-make-a-block-use-multiple-views-a-partial-view-controller-explained

namespace Dlw.EpiBase.Content.Infrastructure.Epi
{
    [TemplateDescriptor(TemplateTypeCategory = TemplateTypeCategories.MvcPartialController, Inherited = true, AvailableWithoutTag = true)]
    public class BasePartialController<T> : PartialContentController<T> where T : IContentData
    { }
}