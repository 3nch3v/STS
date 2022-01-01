using System.Collections.Generic;

namespace STS.Web.ViewModels.Tasks
{
    public class TaskViewModel : TaskLinstingViewModel
    {
        public string Description { get; set; }

        public ICollection<ReplayTaskViewModel> Comments { get; set; }
    }
}
