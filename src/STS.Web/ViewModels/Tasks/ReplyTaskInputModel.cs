using System.ComponentModel.DataAnnotations;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.Tasks
{
    public class ReplyTaskInputModel
    {
        public int EmployeeTaskId { get; set; }

        [Required]
        [StringLength(ReplyTaskContentMaxLength, MinimumLength = ReplyTaskContentMinLength)]
        public string Content { get; set; }
    }
}
