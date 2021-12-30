using System.Collections.Generic;

namespace STS.Data.Dtos.User
{
    public class UserDto : BaseUserDto
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Position { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<string> Roles { get; set; }

    }
}
