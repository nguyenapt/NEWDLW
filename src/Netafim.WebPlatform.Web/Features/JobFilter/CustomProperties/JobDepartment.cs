using System;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties
{
    public class JobDepartment
    {
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        
        public bool Equals(JobDepartment other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return DepartmentName.Refined().Equals(other.DepartmentName.Refined());
        }

        public override int GetHashCode()
        {
            return DepartmentName == null ? 0 : DepartmentName.Refined().GetHashCode();
        }
    }
}