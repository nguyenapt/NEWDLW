using Netafim.WebPlatform.Web.Features.Navigation;
using EPiServer.Web.Mvc.Html;
using EPiServer;
using System.Linq;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.Layout
{
    public class LayoutController : Controller
    {
        private readonly IHeaderSettings _headerSettings;
        private readonly ILinkItemMapper _linkMapper;
        private readonly ISocialMediaSettings _socialMediaSettings;
        private readonly IFooterSettings _footerSettings;
        private readonly IContentLoader _contentLoader;
        private readonly IFooterRepository _footerRepository;

        public LayoutController(IHeaderSettings headerSettings, ILinkItemMapper linkItemRepo,
            ISocialMediaSettings socialMediaSettings, IFooterSettings footerSettings,
            IContentLoader contentLoader, IFooterRepository footerRepository)
        {
            _headerSettings = headerSettings;
            _linkMapper = linkItemRepo;
            _socialMediaSettings = socialMediaSettings;
            _footerSettings = footerSettings;
            _contentLoader = contentLoader;
            _footerRepository = footerRepository;
        }

        public ActionResult Header()
        {
            var model = new HeaderViewModel()
            {
                LogoLink = _headerSettings.LogoLink,
                HeroLogoLink=_headerSettings.HeroLogolink,
                CarreerLink = _headerSettings.CarreerLink,
                ContactUsLink = _headerSettings.ContactUsLink,
                Socials = _socialMediaSettings,
                OtherIndustries = _linkMapper.GetLinkItems(_headerSettings.OtherIndustries)
            };
            return PartialView("_header", model);
        }

        public ActionResult Footer()
        {
            var internalLeadings = _footerRepository.GetInternalLeadings();
            var subFooterLinks = _linkMapper.GetLinkItems(_footerSettings.SubFooter);
            var footerModel = new FooterViewModel()
            {
                Logo = _headerSettings.HeroLogolink,
                Carreer = _headerSettings.CarreerLink,
                ContactUs = _footerSettings.ContatUs,
                InternalLeadings = internalLeadings != null ? internalLeadings.Select(MapToViewModel) : Enumerable.Empty<LinkViewModel>(),
                Socials = _socialMediaSettings,
                SubFooter = subFooterLinks ?? Enumerable.Empty<LinkViewModel>()
            };

            return PartialView("_footer", footerModel);
        }
        
        private LinkViewModel MapToViewModel(NavigationPage navigation)
        {
            var linkText = !string.IsNullOrWhiteSpace(navigation.Title) ? navigation.Title : navigation.Name;
            return new LinkViewModel(linkText, Url.ContentUrl(navigation.Link), navigation.LinkURL);
        }
    }
}