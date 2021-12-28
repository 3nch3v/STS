using System.ComponentModel.DataAnnotations;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.Tickets
{
    public class BaseTicketInputModel
    {
        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        public string AssignedToId { get; set; }

        public int PriorityId { get; set; }

        public int DepartmentId { get; set; }
    }
}
