using System;
using System.ComponentModel.DataAnnotations;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.Tasks
{
    public class TaskInputModel
    {
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(TaskDescriptionMaxLength)]
        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public int PriorityId { get; set; }

        public int StatusId { get; set; }

        [Required]
        public string EmployeeId { get; set; }
    }
}
