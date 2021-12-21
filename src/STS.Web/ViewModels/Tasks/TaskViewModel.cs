using System;
using System.Collections.Generic;

namespace STS.Web.ViewModels.Tasks
{
    internal class TaskViewModel
    {
        public int id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public int PriorityId { get; set; }

        public int StatusId { get; set; }

        public string ManagerUserName { get; set; }

        public ICollection<ReplayTaskViewModel> Comments { get; set; }
    }
}
