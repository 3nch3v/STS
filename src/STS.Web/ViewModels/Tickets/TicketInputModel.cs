using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.Tickets
{
    public class TicketInputModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(TitleMaxLength, MinimumLength = ContentMinLength)]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        public string AssignedToId { get; set; }

        public int PriorityId { get; set; }

        public int DepartmentId { get; set; }

        public IEnumerable<PriorityViewModel> Priorities { get; set; }

        public IEnumerable<StatusViewModel> Statuses { get; set; }

        public IEnumerable<DepartmentViewModel> Departments { get; set; }
    }
}
