using System;
using System.Collections.Generic;

namespace STS.Web.ViewModels.Tasks
{
    public class TaskViewModel : BaseTaskViewModel
    {
        public string Description { get; set; }

        public string StatusName { get; set; }

        public string ManagerUserName { get; set; }

        public ICollection<ReplayTaskViewModel> Comments { get; set; }
    }
}
