using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using STS.Web.Infrastructure.ValidationAttributes;
using STS.Web.ViewModels.Admin;
using STS.Web.ViewModels.Common;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.User
{
    public class BaseUserInputmodel
    {
        [Required]
        [MaxLength(EmployeeNameMaxLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(EmployeeNameMaxLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(EmployeeNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(EmployeeNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(EmployeePositionNameMaxLength)]
        public string Position { get; set; }

        [Required]
        public string Role { get; set; }

        [DepartmentId]
        public int DepartmentId { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<DepartmentViewModel> Departments { get; set; }

        public IEnumerable<RoleViewModel> SystemRoles { get; set; }
    }
}
