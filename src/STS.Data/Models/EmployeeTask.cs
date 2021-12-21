using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using STS.Data.Common;

using static STS.Common.GlobalConstants;

namespace STS.Data.Models
{
    public class EmployeeTask : BaseDeletableModel<int>
    {
        public EmployeeTask()
        {
            this.Comments = new HashSet<ReplyTask>();
        }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(TaskDescriptionMaxLength)]
        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public int PriorityId { get; set; }

        public virtual Priority Priority { get; set; }

        public int StatusId { get; set; }

        public virtual Status Status { get; set; }

        [Required]
        public string EmployeeId { get; set; }

        public virtual ApplicationUser Employee { get; set; }

        [Required]
        public string ManagerId { get; set; }

        public virtual ApplicationUser Manager { get; set; }

        public virtual ICollection<ReplyTask> Comments { get; set; }
    }
}
