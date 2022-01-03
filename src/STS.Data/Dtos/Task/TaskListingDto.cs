namespace STS.Data.Dtos.Task
{
    public class TaskListingDto : BaseTaskDto
    {
        public string EmployeeUserName { get; set; }

        public string ManagerUserName { get; set; }

        public string StatusName { get; set; }
    }
}
