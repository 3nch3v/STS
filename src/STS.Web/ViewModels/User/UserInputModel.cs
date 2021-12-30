using System.ComponentModel.DataAnnotations;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.User
{
    public class UserInputModel
    {
        [Required]
        [MaxLength(EmployeeNameMaxLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(EmployeeNameMaxLength)]
        public string Email { get; set; }

        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        public string Password { get; set; }

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
        [MaxLength(RoleNameMaxLength)]
        public string Role { get; set; }

        public int DepartmentId { get; set; }

        public string PhoneNumber { get; set; }
    }
}
