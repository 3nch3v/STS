using System;
using System.ComponentModel.DataAnnotations;

namespace STS.Web.Infrastructure.ValidationAttributes
{
    public class DeadlineAttribute : ValidationAttribute
    {
        public string GetErrorMessage => $"Deadline should be at a future time.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime deadline;
                var successfullyParsed = DateTime.TryParse(value.ToString(), out deadline);

                if (successfullyParsed)
                {
                    int result = DateTime.Compare(DateTime.Now, deadline);

                    if (result >= 0)
                    {
                        return new ValidationResult(this.GetErrorMessage);

                    }
                } 
            }

            return ValidationResult.Success;
        }
    }
}
