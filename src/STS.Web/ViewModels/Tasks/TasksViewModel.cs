using System;
using System.Collections.Generic;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.Tasks
{
    public class TasksViewModel
    {

        public int TasksCount { get; set; }

        public int PagesCount => (int)Math.Ceiling(TasksCount * 1.0 / TasksPerPage);

        public int Page { get; set; }

        public int PreviousPage => Page - 1;

        public int NextPage => Page + 1;

        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < PagesCount;

        public bool OnlyActive { get; set; }

        public string Keyword { get; set; }

        public string EmployeeId { get; set; }

        public string ManagerId { get; set; }
  
        public IEnumerable<TaskLinstingViewModel> Tasks { get; set; }
    }
}
