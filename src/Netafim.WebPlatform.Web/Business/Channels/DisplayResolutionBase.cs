using DbLocalizationProvider;
using EPiServer.Web;

namespace Netafim.WebPlatform.Web.Business.Channels
{
    /// <summary>
    /// Base class for all resolution definitions
    /// </summary>
    public abstract class DisplayResolutionBase : IDisplayResolution
    {
        protected DisplayResolutionBase(string name, int width, int height)
        {
            Id = GetType().FullName;
            _name = name;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets the unique ID for this resolution
        /// </summary>
        public string Id { get; protected set; }

        private string _name;
        /// <summary>
        /// Gets the name of resolution
        /// </summary>
        public string Name
        {
            get { return string.Format("{0} ({1} x {2})", LocalizationProvider.Current.GetString(_name), Width, Height); }
        }

        /// <summary>
        /// Gets the resolution width in pixels
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Gets the resolution height in pixels
        /// </summary>
        public int Height { get; protected set; }
    }
}
