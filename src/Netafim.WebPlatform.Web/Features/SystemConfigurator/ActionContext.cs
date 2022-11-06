using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator
{
    public class ActionContext
    {
        public int ContentId { get; set; }
        
        public static TActionContext Create<TActionContext>(NameValueCollection values) where TActionContext : ActionContext
        {
            var instance = Activator.CreateInstance<TActionContext>();
            var cipher = ServiceLocator.Current.GetInstance<ICipher>();

            foreach (var property in typeof(TActionContext).GetProperties())
            {
                foreach (string key in values)
                {
                    if (property.Name.ToLower() == key.ToLower())
                    {

                        if(property.PropertyType == typeof(int))
                        {
                            int queryValue = 0;

                            if (int.TryParse(values[key], out queryValue))
                            {
                                property.SetValue(instance, queryValue);
                            }
                            
                            break;
                        }
                       
                        if(property.PropertyType == typeof(string))
                        {
                            var value = values[key];

                            if (property.Name.ToLower() == nameof(StarterActionContext.ClientEmail).ToLower() ||
                                property.Name.ToLower() == nameof(StarterActionContext.ClientName).ToLower())
                            {
                                // Decrypt the value
                                value = cipher.Decipher(value, StarterActionContext.Shift);
                            }

                            property.SetValue(instance, value);
                        }
                    }
                }
            }

            return instance;
        }
    }

    public class StarterActionContext : ActionContext
    {
        public string ClientName { get; set; }

        public string ClientEmail { get; set; }

        public const int Shift = 18;
    }

    public class PipelineActionResult
    {
        public string RedirectUrl { get; set; }
    }

    public class ParametersActionContext : StarterActionContext
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Region { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Crop { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PlotSize { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int RowSpacing { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int WaterSource { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MaxIrrigation { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int IrrigationCycle { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Filtration { get; set; }
        
        public string SubmissionId { get; set; }
    }
}