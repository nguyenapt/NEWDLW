using System;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Core.Bynder
{
    public class AssetData
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public IEnumerable<DerivativeData> Derivatives { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }
    }
}