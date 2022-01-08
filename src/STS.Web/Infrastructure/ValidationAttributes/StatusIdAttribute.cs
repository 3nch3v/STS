using System.Linq;
using System.ComponentModel.DataAnnotations;

using STS.Services.Contracts;

namespace STS.Web.Infrastructure.ValidationAttributes
{
    public class StatusIdAttribute : ValidationAttribute
    {
        public string GetErrorMessage => $"Status does not exist.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var commonService = (ICommonService)validationContext.GetService(typeof(ICommonService));

                int id;
                var successfullyParsed = int.TryParse(value.ToString(), out id);

                if (successfullyParsed)
                {
                    var statuses = commonService.GetStatuses()
                        .Select(x => x.Id)
                        .ToArray();

                    if (!statuses.Contains(id))
                    {
                        return new ValidationResult(this.GetErrorMessage);
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
