using System.Collections.Generic;

using STS.Web.ViewModels.Common;

namespace STS.Web.ViewModels.Tasks
{
    public class TaskViewModel : TaskLinstingViewModel
    {
        public string Description { get; set; }

        public ICollection<StatusViewModel> Statuses { get; set; }

        public ICollection<ReplayTaskViewModel> Comments { get; set; }
    }
}
