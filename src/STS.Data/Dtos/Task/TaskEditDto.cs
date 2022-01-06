using System;

namespace STS.Data.Dtos.Task
{
    public class TaskEditDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? Deadline { get; set; }

        public int? PriorityId { get; set; }

        public int? StatusId { get; set; }

        public string EmployeeId { get; set; }
    }
}
