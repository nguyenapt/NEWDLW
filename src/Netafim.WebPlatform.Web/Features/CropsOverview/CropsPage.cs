using System.ComponentModel.DataAnnotations;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.CropsOverview
{
    [ContentType(DisplayName = "Crops Page", GUID = "830e3663-9098-4f5e-829b-2f62b45a8e33", GroupName = GroupNames.CropsOverview)]
    public class CropsPage : GenericContainerPage
    {
        [Required]
        public override string Title { get; set; }
    }
}