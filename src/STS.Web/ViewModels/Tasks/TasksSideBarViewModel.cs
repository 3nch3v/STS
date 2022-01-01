using System.Collections.Generic;

namespace STS.Web.ViewModels.Tasks
{
    public class TasksSideBarViewModel
    {
        public IEnumerable<BaseTaskViewModel> Tasks { get; set; }
    }
}
