using STS.Web.ViewModels.Common;
using System;
using System.Collections.Generic;

namespace STS.Web.ViewModels.Tickets
{
    public class TicketViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string PriorityName { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string AssignedToId { get; set; }

        public string AssignedToUserName { get; set; }

        public DateTime CreatedOn { get; set; }

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
