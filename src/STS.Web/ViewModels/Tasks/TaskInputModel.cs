using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using STS.Web.Infrastructure.ValidationAttributes;
using STS.Web.ViewModels.Common;
using STS.Web.ViewModels.User;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.Tasks
{
    public class TaskInputModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(TaskDescriptionMaxLength, MinimumLength = TaskDescriptionMinLength)]
        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        [PriorityId]
        public int PriorityId { get; set; }

        [Required]
        public string EmployeeId { get; set; }

        public IEnumerable<PriorityViewModel> Priorities { get; set; }

        public IEnumerable<BaseUserViewModel> Employees { get; set; }
    }
}
