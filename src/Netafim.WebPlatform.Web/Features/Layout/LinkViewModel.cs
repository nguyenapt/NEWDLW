namespace Netafim.WebPlatform.Web.Features.Layout
{
    public class LinkViewModel
    {
        public LinkViewModel(string text, string url, string linkUrl)
        {
            this.Text = text;
            this.Url = url;
            this.LinkUrl = linkUrl;
        }
        public string Text { get; set; }
        public string Url { get; set; }
        public string LinkUrl { get; set; }
    }
}