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
            var commonService = (ICommonService)validationContext.GetService(typeof(ICommonService));

            if (value != null)
            {
                int id;
                var isInt = int.TryParse(value.ToString(), out id);

                var statuses = commonService.GetStatuses()
                    .Select(x => x.Id)
                    .ToArray();

                if (!statuses.Contains(id))
                {
                    return new ValidationResult(this.GetErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
