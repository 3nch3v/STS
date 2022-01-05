using System.ComponentModel.DataAnnotations;
using System.Linq;

using STS.Services.Contracts;

namespace STS.Web.Infrastructure.ValidationAttributes
{
    public class DepartmentIdAttribute : ValidationAttribute
    {
        public string GetErrorMessage => $"Department does not exist.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           var commonService = (ICommonService) validationContext.GetService(typeof(ICommonService));

            if (value != null)
            {
                int id;
                var isInt = int.TryParse(value.ToString(), out id);

                var priorities = commonService.GetDepartmentsBase()
                    .Select(x => x.Id)
                    .ToArray();

                if (!priorities.Contains(id))
                {
                    return new ValidationResult(this.GetErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
