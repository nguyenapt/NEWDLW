using System;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Validation;
using Netafim.WebPlatform.Web.Core.Validation;
using Netafim.WebPlatform.Web.Features.SuccessStory;

namespace Netafim.WebPlatform.Web.Features.SuccessStoryOverview
{
    public class SuccessStoryPageValidation : IValidate<SuccessStoryPage>
    {
        public IEnumerable<ValidationError> Validate(SuccessStoryPage instance)
        {
            // List of errors to return within the scope of the current IValidate
            var errors = new List<ValidationError>();

            // Check if the block is published first before trying to validate
            if (ContentReference.IsNullOrEmpty(instance.ContentLink))
            {
                return new ValidationError[0];
            }

            if (instance.BoostedFrom == DateTime.MinValue && instance.BoostedTo == DateTime.MinValue)
                return errors;

            if (instance.BoostedFrom >= instance.BoostedTo)
            {
                var errorMess = "Success Story Page required Boosted From need to less than Boosted To.";
                Helper.AddError(errorMess, ref errors, "Success Story Page", "From");
            }

            return errors;
        }
    }
}