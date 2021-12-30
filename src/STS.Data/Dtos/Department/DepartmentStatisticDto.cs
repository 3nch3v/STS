namespace STS.Data.Dtos.Department
{
    public class DepartmentStatisticDto : DepartmentBaseDto
    {
        public int EmployeesCount { get; set; }

        public int ActiveTicketsCount { get; set; }

        public int ActiveTasksCount { get; set; }
    }
}
