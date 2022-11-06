using EPiServer.Core;
using EPiServer.Globalization;
using System;

namespace Netafim.WebPlatform.Web.Features.WhatsNew
{
    public class FeedViewModel : WhatsNewItemViewModel
    {
        public FeedViewModel(string desc, DateTime date, string postLink) : base(desc, date, postLink) { }
        public FeedViewModel() : base() { }

        public SocialChannel Channel { get; set; }
        public string ChannelLink { get; set; }
        public string Thumbnail { get; set; }
    }
    public enum SocialChannel
    {
        Facebook,
        Twitter,
        LinkedIn,
        Instagram,
        Youtube
    }
    public abstract class WhatsNewItemViewModel
    {
        public WhatsNewItemViewModel(string desc, DateTime date, string postLink)
        {
            Description = desc;
            PostDate = date;
            PostLink = postLink;
        }
        public WhatsNewItemViewModel()
        {

        }
        public string Description { get; set; }
        public DateTime PostDate { get; set; }
        public string PostLink { get; set; }

        public string PostDateString
        {
            get
            {
                return PostDate.ToString("dd MMMM yyyy", ContentLanguage.PreferredCulture);
            }
        }
    }

    public class NewsEventItemViewModel : WhatsNewItemViewModel
    {
        public NewsEventItemViewModel(string desc, DateTime date, string postLink) : base(desc, date, postLink) { }
        public NewsEventItemViewModel() : base()
        {

        }
        public ContentReference Image { get; set; }
    }
}