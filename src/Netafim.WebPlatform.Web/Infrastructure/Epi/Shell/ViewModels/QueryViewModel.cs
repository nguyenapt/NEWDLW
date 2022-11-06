using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels
{
    public class QueryViewModel
    {
        [Required]
        [JsonProperty("blockId")]
        public int BlockId { get; set; }
    }
}