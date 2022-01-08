using System;
using System.ComponentModel.DataAnnotations;

using STS.Web.Infrastructure.ValidationAttributes;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.Tasks
{
    public class TaskEditModel
    {
        public int Id { get; set; }

        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; }

        [StringLength(TaskDescriptionMaxLength, MinimumLength = TaskDescriptionMinLength)]
        public string Description { get; set; }

        [Deadline]
        public DateTime? Deadline { get; set; }

        [PriorityId]
        public int? PriorityId { get; set; }

        [StatusId]
        public int? StatusId { get; set; }

        public string EmployeeId { get; set; }
    }
}