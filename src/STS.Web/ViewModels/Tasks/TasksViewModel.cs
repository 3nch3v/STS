using System.Collections.Generic;

namespace STS.Web.ViewModels.Tasks
{
    public class TasksViewModel
    {
        public IEnumerable<BaseTaskViewModel> Tasks { get; set; }
    }
}
