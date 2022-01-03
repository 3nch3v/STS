namespace STS.Common
{
    public static class GlobalConstants
    {
        public const int PasswordMinLength = 6;

        public const int PasswordMaxLength = 128;

        public const int EmployeeNameMaxLength = 50;

        public const int EmployeePositionNameMaxLength = 50;

        public const int TitleMinLength = 2;

        public const int TitleMaxLength = 100;

        public const int ContentMinLength = 5;

        public const int ContentMaxLength = 2000;

        public const int CommentMinLength = 2;

        public const int CommentMaxLength = 2000;

        public const int TaskDescriptionMinLength = 5;

        public const int TaskDescriptionMaxLength = 1000;

        public const int ReplyTaskContentMaxLength = 1000;

        public const int StatusNameMaxLength = 20;

        public const int PriorityNameMaxLength = 20;

        public const int DepartmentNameMinLength = 2;

        public const int DepartmentNameMaxLength = 50;

        public const int RoleNameMinLength = 2;

        public const int RoleNameMaxLength = 50;

        public const int TicketsPerPage = 15;

        public const int UsersPerPage = 12;

        public const int DefaultPageNumber = 1;

        public const int TasksSideBarCount = 12;

        public const int TasksPerPage = 12;

        public const string SystemName = "STS";

        public const string AdministratorRoleName = "Administrator";

        public const string EmployeeRoleName = "Employee";

        public const string ManagerRoleName = "Manager";

        public static class ErrorMessage
        {
            public const string BadRequest = "The server cannot or will not process the request due to a client error";

            public const string NotFound = "Sorry, we can not find the page you are looking for.";

            public const string Unauthorised = "You are not authorised for this action";

            public const string Forbidden = "You are not authorised for this action";

            public const string ServerError = "Internal Server Error";

            public const string BadGateway = "The server was acting as a gateway or proxy and received an invalid response from the upstream server.";

            public const string HttpNotSupported = "HTTP Version Not Supported";
        }
    }
}
