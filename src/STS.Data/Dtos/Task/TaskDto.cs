using System.Collections.Generic;

namespace STS.Data.Dtos.Task
{
    public class TaskDto : TaskListingDto
    {
        public string ManagerId { get; set; }

        public string EmployeeId { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ReplayTaskDto> Comments { get; set; }
    }
}
