namespace Netafim.WebPlatform.Web.Business.Channels
{
    /// <summary>
    /// Defines resolution for desktop displays
    /// </summary>
    public class StandardResolution : DisplayResolutionBase
    {
        public StandardResolution() : base(Labels.SolutionStandard, 1366, 768)
        {
        }
    }

    /// <summary>
    /// Defines resolution for a horizontal iPad
    /// </summary>
    public class IpadHorizontalResolution : DisplayResolutionBase
    {
        public IpadHorizontalResolution() : base(Labels.SolutionIpadhorizontal, 1024, 768)
        {
        }
    }

    /// <summary>
    /// Defines resolution for a vertical iPhone 5s
    /// </summary>
    public class IphoneVerticalResolution : DisplayResolutionBase
    {
        public IphoneVerticalResolution() : base(Labels.SolutionIphonevertical, 320, 568)
        {
        }
    }

    /// <summary>
    /// Defines resolution for a vertical Android handheld device
    /// </summary>
    public class AndroidVerticalResolution : DisplayResolutionBase
    {
        public AndroidVerticalResolution() : base(Labels.SolutionAndroidvertical, 480, 800)
        {
        }
    }
}
