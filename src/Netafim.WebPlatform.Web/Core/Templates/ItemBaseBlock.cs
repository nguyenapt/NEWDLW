using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Editors;

namespace Netafim.WebPlatform.Web.Core.Templates
{
    public abstract class ItemBaseBlock : BaseBlock
    {
        #region BaseBlock

        [Ignore]
        public override string AnchorId { get; set; }

        [Ignore]
        public override string Watermark { get; set; }

        [Ignore]
        public override bool OnParallaxEffect { get; set; }

        [Ignore]
        public override BackgroundColor BackgroundColor { get; set; }

        [Ignore]
        public override string VerticalText { get; set; }

        #endregion BaseBlock
    }
}