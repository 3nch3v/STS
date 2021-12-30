using STS.Web.ViewModels.Common;

namespace STS.Web.ViewModels.Admin
{
    public class DepartmentStatisticViewModel : DepartmentViewModel
    {
        public int EmployeesCount { get; set; }

        public int ActiveTicketsCount { get; set; }

        public int ActiveTasksCount { get; set; }
    }
}
