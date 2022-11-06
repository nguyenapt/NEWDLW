namespace Netafim.WebPlatform.Web.Features.OfficeLocator
{
    public class OfficeDetailViewModel
    {
        public virtual string OfficeName { get; set; }

        public virtual string Address { get; set; }

        public virtual string Phone { get; set; }

        public virtual string Fax { get; set; }

        public virtual string Email { get; set; }

        public virtual string Website { get; set; }

        public virtual string Direction { get; set; }

        public virtual double Longtitude { get; set; }

        public virtual double Latitude { get; set; }
    }
}