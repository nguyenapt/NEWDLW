using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;

namespace Dlw.EpiBase.Content.Infrastructure.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ContentAreaMaxItems : ValidationAttribute
    {
        private int _max;

        public ContentAreaMaxItems(int max)
        {
            _max = max;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            if ((value != null) && !(value is ContentArea))
            {
                throw new ValidationException("ContentAreaMaxItems is intended only for use with ContentArea properties");
            }

            var contentArea = value as ContentArea;

            if (contentArea.Count > _max)
            {
                ErrorMessage = string.Format("ContentArea restricted to {0} content items", _max);
                return false;
            }
            return true;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = base.IsValid(value, validationContext);
            if (result != null && !string.IsNullOrEmpty(result.ErrorMessage))
            {
                result.ErrorMessage = string.Format("{0} {1}", validationContext.DisplayName, ErrorMessage);
            }
            return result;
        }
    }
}
