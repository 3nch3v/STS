namespace STS.Data.Dtos.User
{
    public class BaseUserInputDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Position { get; set; }

        public string Role { get; set; }

        public int DepartmentId { get; set; }

        public string PhoneNumber { get; set; }
    }
}
