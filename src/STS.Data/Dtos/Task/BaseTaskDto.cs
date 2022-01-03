using System;

namespace STS.Data.Dtos.Task
{
    public class BaseTaskDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Deadline { get; set; }

        public string PriorityName { get; set; }
    }
}
