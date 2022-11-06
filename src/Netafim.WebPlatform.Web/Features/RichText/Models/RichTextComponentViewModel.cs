using EPiServer.Core;
using System;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.RichText.Models
{
    public class RichTextComponentViewModel<T>
        where T : IRichTextColumnComponent
    {
        public T Block { get; }

        public int ComponentPosition
        {
            get
            {
                IList<Guid> components = Parent?.Block?.Items?.ReferencedPermanentLinkIds;

                var index = components != null && this.Block is IContent ? components.IndexOf(((IContent)this.Block).ContentGuid) : 0;
                return index;
            }
        }

        public RichTextContainerViewModel Parent { get; }

        public RichTextComponentViewModel(T block, RichTextContainerViewModel container)
        {
            this.Block = block;
            this.Parent = container;
        }

        public bool IsFirstComponent() => this.ComponentPosition == 0;
    }
}