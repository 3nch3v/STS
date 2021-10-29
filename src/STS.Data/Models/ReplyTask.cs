using System.ComponentModel.DataAnnotations;

using STS.Data.Common;

using static STS.Common.GlobalConstants;

namespace STS.Data.Models
{
    public class ReplyTask : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(ReplyTaskContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int EmployeeTaskId { get; set; }

        public virtual EmployeeTask EmployeeTask { get; set; }
    }
}
