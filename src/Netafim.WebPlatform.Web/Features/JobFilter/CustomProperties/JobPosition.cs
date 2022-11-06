using System;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties
{
    public class JobPosition
    {
        [Display(Name = "Job Name")]
        public string JobName { get; set; }
        
        public bool Equals(JobPosition other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return JobName.Refined().Equals(other.JobName.Refined());
        }

        public override int GetHashCode()
        {
            return JobName == null ? 0 : JobName.Refined().GetHashCode();
        }
    }
}