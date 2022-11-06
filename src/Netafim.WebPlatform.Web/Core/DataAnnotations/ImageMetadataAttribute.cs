using System;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Core.DataAnnotations
{
    /// <summary>
    /// Metadata of recommended size of a image when it's displayed
    /// </summary>
    public sealed class ImageMetadataAttribute : Attribute
    {
        public ImageMetadataAttribute(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Mode = ResizeMode.Crop;
        }

        /// <summary>
        /// Recommended height
        /// </summary>
        [Required]
        public int Height { get; set; }

        /// <summary>
        /// Recommended width
        /// </summary>
        [Required]
        public int Width { get; set; }

        /// <summary>
        /// Resize mode
        /// </summary>
        public ResizeMode Mode { get; set; }
    }

    public enum ResizeMode
    {
        Crop,
    }
}