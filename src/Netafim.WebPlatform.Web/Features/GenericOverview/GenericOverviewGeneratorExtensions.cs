using EPiServer.Core;
using Netafim.WebPlatform.Web.Core.Extensions;

namespace Netafim.WebPlatform.Web.Features.GenericOverview
{
    public static class GenericOverviewGeneratorExtensions
    {
        private const string MediaFolder = "~/Features/GenericOverview/Data/Demo/{0}";

        public static GenericOverviewBlock Init(this GenericOverviewBlock overViewBlock)
        {
            overViewBlock = overViewBlock.CreateWritableClone() as GenericOverviewBlock;
            ((IContent)overViewBlock).Name = "Generic Overview";
            overViewBlock.Title = "Where does it come from?";
            overViewBlock.Description = "Our irrigation services offer you comprehensive solutions. From initial specification to a detailed design and the implementation of the system, our experts are there to assist you with your project.";
            overViewBlock.Watermark = "Challenges";

            return overViewBlock;
        }

        public static GenericOverviewBlock AddItem(this GenericOverviewBlock overviewBlock, ContentReference item)
        {
            overviewBlock.Items = overviewBlock.Items != null ? overviewBlock.Items : new ContentArea();
            overviewBlock.Items.Items.Add(new ContentAreaItem() { ContentLink = item });

            return overviewBlock;
        }
        public static T Init<T>(this T overViewItem, string title = null) where T : GenericOverviewItemBlock
        {
            overViewItem = overViewItem.CreateWritableClone() as T;
            ((IContent)overViewItem).Name = "Overview Item";
            overViewItem.Title = string.IsNullOrEmpty(title) ? "The standard Lorem Ipsum passage, used since the 1500s" : title;

            return overViewItem;
        }       

        public static GenericOverviewItemWithIconBlock AddDescription(this GenericOverviewItemWithIconBlock overViewItem, string description = null)
        {
            overViewItem = overViewItem.CreateWritableClone() as GenericOverviewItemWithIconBlock;
            ((IContent)overViewItem).Name = "Overview Item";
            overViewItem.Description = string.IsNullOrEmpty(description)
                ? "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC."
                : description;

            return overViewItem;
        }

        public static GenericOverviewItemWithIconBlock AddIcon(this GenericOverviewItemWithIconBlock overviewItem, string fileName)
        {
            overviewItem.Icon = ((IContent)overviewItem).CreateBlob(string.Format(MediaFolder, fileName));

            return overviewItem;
        }

        public static SustainableItemBlock AddIcon(this SustainableItemBlock overviewItem, string fileName)
        {
            overviewItem.Image = ((IContent)overviewItem).CreateBlob(string.Format(MediaFolder, fileName));

            return overviewItem;
        }

        public static GenericOverviewItemWithThumbnailBlock AddThumbnail(this GenericOverviewItemWithThumbnailBlock overviewItem, string fileName)
        {
            overviewItem.Thumbnail = ((IContent)overviewItem).CreateBlob(string.Format(MediaFolder, fileName));

            return overviewItem;
        }

        public static T AddLink<T>(this T overviewItem, ContentReference link) where T : GenericOverviewItemBlock
        {
            overviewItem.Link = link;
            return overviewItem;
        }
    }
}