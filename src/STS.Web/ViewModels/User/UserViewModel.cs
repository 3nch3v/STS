using System.Collections.Generic;

namespace STS.Web.ViewModels.User
{
    public class UserViewModel : BaseUserViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }

        public string DepartmentName { get; set; }

        public string Position { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
