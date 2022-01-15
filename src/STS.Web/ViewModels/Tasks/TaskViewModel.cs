using System.Collections.Generic;

using STS.Web.ViewModels.Common;
using STS.Web.ViewModels.User;

namespace STS.Web.ViewModels.Tasks
{
    public class TaskViewModel : TaskLinstingViewModel
    {
        public string Description { get; set; }

        public string ManagerId { get; set; }

        public string EmployeeId { get; set; }

        public IEnumerable<StatusViewModel> Statuses { get; set; }

        public IEnumerable<BaseUserViewModel> Employees { get; set; }

        public IEnumerable<ReplyTaskViewModel> Comments { get; set; }
    }
}
