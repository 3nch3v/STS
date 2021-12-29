using System.Collections.Generic;

using STS.Web.ViewModels.Common;

namespace STS.Web.ViewModels.Tickets
{
    public class TicketViewModel : BaseTicketViewModel
    {
        public string Content { get; set; }

        public int StatusId { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string AssignedToId { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeUserName { get; set; }

        public string LoggedInUserId { get; set; }

        public bool IsOwner => EmployeeId == LoggedInUserId;

        public ICollection<CommentViewModel> Comments { get; set; }

        public ICollection<DepartmentViewModel> Departments { get; set; }

        public ICollection<StatusViewModel> Statuses { get; set; }

        public ICollection<EmployeesViewModel> Employees { get; set; }
    }
}
