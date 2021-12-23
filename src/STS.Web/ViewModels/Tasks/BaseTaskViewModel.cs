using System;

namespace STS.Web.ViewModels.Tasks
{
    public class BaseTaskViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Deadline { get; set; }

        public string PriorityName { get; set; }
    }
}
