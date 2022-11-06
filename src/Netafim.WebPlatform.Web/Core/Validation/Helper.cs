using EPiServer.Validation;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Core.Validation
{
    public static class Helper
    {
        /// <summary>
        ///     This method will create a new ValidationError and add it to the validationErrors List object.
        /// </summary>
        public static void AddError(string errorMessage, ref List<ValidationError> validationErrors,
            string controlName = null, string propertyName = null)
        {
            if (string.IsNullOrEmpty(errorMessage))
            {
                return;
            }

            AddError(new ValidationError { ErrorMessage = errorMessage }, ref validationErrors, controlName ?? null,
                propertyName ?? null);
        }

        /// <summary>
        ///     If the validationError object is not null, this method will add it to the validationErrors List object.
        /// </summary>
        public static void AddError(ValidationError validationError, ref List<ValidationError> validationErrors,
            string controlName = null, string propertyName = null)
        {
            if (validationError == null)
            {
                return;
            }
            if (validationErrors == null)
            {
                validationErrors = new List<ValidationError>();
            }
            if (!string.IsNullOrEmpty(controlName))
            {
                validationError.ErrorMessage = $"{controlName}: {validationError.ErrorMessage}";
                validationError.Severity = ValidationErrorSeverity.Error;
                validationError.PropertyName = propertyName;
            }

            validationErrors.Add(validationError);
        }

        public static void AddErrors(IEnumerable<ValidationError> validationErrorsToAdd,
            ref List<ValidationError> validationErrors, string controlName = null, string propertyName = null)
        {
            validationErrors = validationErrors ?? new List<ValidationError>();
            foreach (var errorToAdd in validationErrorsToAdd)
            {
                AddError(errorToAdd, ref validationErrors, controlName, propertyName);
            }
        }
    }
}