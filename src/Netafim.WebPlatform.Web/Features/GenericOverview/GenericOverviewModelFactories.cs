using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using Netafim.WebPlatform.Web.Core.Extensions;

namespace Netafim.WebPlatform.Web.Features.GenericOverview
{
    public interface IGenericOverviewModelFactory
    {
        bool IsSatisfied(GenericOverviewBlock overviewBlock);

        GenericOverviewBlockModel Create(GenericOverviewBlock overviewBlock);
    }

    public abstract class GenericOverivewDisplayModeFactory : IGenericOverviewModelFactory
    {
        public GenericOverviewBlockModel Create(GenericOverviewBlock overviewBlock)
        {
            var model = new GenericOverviewBlockModel(overviewBlock);
            if (overviewBlock.Items.IsNullOrEmptyForViewing()) return model;

            var totalColumns = GetColumnsNumber(overviewBlock);
            model.IsListOfThumnailItems = IsListOfThumnailItems(overviewBlock);
            model.ItemRows = GetItemRows(totalColumns, overviewBlock);

            return model;
        }

        protected abstract List<List<IContent>> GetItemRows(int totalColumns, GenericOverviewBlock overviewBlock);

        protected abstract int GetColumnsNumber(GenericOverviewBlock block);

        public abstract bool IsSatisfied(GenericOverviewBlock overviewBlock);

        protected bool IsListOfThumnailItems(GenericOverviewBlock block)
        {
            var firstItem = block.Items.IsNullOrEmpty() ? null : block.Items.Items.First().GetContent();
            return firstItem is GenericOverviewItemWithThumbnailBlock;
        }

        protected enum ColumnsTypes
        {
            IconItemFourColumns = 4,
            IconItemFiveColumns = 5,
            ThumnailItemThreeColumns = 3
        }
    }

    public class GenericOverivewCarouselModeFactory : GenericOverivewDisplayModeFactory
    {
        protected override int GetColumnsNumber(GenericOverviewBlock block)
        {
            return 1;
        }

        public override bool IsSatisfied(GenericOverviewBlock overviewBlock)
        {
            return overviewBlock.DisplayingMode == GenericOverviewDisplayingMode.Carousel;
        }

        protected override List<List<IContent>> GetItemRows(int totalColumns, GenericOverviewBlock overviewBlock)
        {
            var itemsToDisplay = overviewBlock.Items.FilteredItems.Select(x => x.GetContent()).ToList();
            return new List<List<IContent>>() { itemsToDisplay };
        }
    }

    public class GenericOverivewGridModeFactory : GenericOverivewDisplayModeFactory
    {
        protected override int GetColumnsNumber(GenericOverviewBlock block)
        {
            if (block.Items.IsNullOrEmpty()) return 0;
            var columnsTypes = IsListOfThumnailItems(block) ? new List<int>() { (int)ColumnsTypes.ThumnailItemThreeColumns }
                : new List<int>() { (int)ColumnsTypes.IconItemFourColumns, (int)ColumnsTypes.IconItemFiveColumns };
            return block.Items.Count >= columnsTypes.Max() ? columnsTypes.Max() : columnsTypes.Min();
        }

        public override bool IsSatisfied(GenericOverviewBlock overviewBlock)
        {
            return overviewBlock.DisplayingMode == GenericOverviewDisplayingMode.Grid;
        }

        protected override List<List<IContent>> GetItemRows(int totalColumns, GenericOverviewBlock overviewBlock)
        {
            var itemsRows = new List<List<IContent>>();
            var rows = overviewBlock.Items.FilteredItems.AsEnumerable().Chunk(totalColumns);
            foreach (var row in rows)
            {
                itemsRows.Add(row.Select(x => x.GetContent()).ToList());
            }

            return itemsRows;
        }
    }
}