using EPiServer.Core;
using EPiServer.Validation;
using Netafim.WebPlatform.Web.Core.Validation;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Events
{
    public class EventPageValidation : IValidate<EventPage>
    {
        public IEnumerable<ValidationError> Validate(EventPage instance)
        {
            // List of errors to return within the scope of the current IValidate
            var errors = new List<ValidationError>();

            // Check if the block is published first before trying to validate
            if (ContentReference.IsNullOrEmpty(instance.ContentLink))
            {
                return new ValidationError[0];
            }

            // Check the ContentArea is not null
            if (instance.From >= instance.To)
            {
                var errorMess = $"Event Page required Time From need to less than Time To.";
                Helper.AddError(errorMess, ref errors, "Event Page", "From");
            }

            return errors;
        }
    }
}