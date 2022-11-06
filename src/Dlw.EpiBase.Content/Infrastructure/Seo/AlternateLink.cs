using System.ComponentModel.DataAnnotations;

namespace Dlw.EpiBase.Content.Infrastructure.Seo
{
    public class AlternateLink
    {
        [Required]
        public string Url { get; set; }

        [Required]
        [Display(Name = "Language")]
        public string Culture { get; set; }
    }
}
