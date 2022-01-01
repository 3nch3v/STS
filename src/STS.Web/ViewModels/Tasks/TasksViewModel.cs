using System;
using System.Collections.Generic;

using STS.Web.ViewModels.User;

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

        public string Keyword { get; set; }

        public string EmployeeId { get; set; }

        public bool OnlyActive { get; set; }

        public IEnumerable<TaskLinstingViewModel> Tasks { get; set; }

        public IEnumerable<BaseUserViewModel> Employees { get; set; }
    }
}
