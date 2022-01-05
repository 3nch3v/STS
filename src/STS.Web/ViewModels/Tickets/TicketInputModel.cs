using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using STS.Web.Infrastructure.ValidationAttributes;
using STS.Web.ViewModels.Common;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.Tickets
{
    public class TicketInputModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; set; }

        public string AssignedToId { get; set; }

        [PriorityId]
        public int PriorityId { get; set; }

        [DepartmentId]
        public int DepartmentId { get; set; }

        public IEnumerable<PriorityViewModel> Priorities { get; set; }

        public IEnumerable<StatusViewModel> Statuses { get; set; }

        public IEnumerable<DepartmentViewModel> Departments { get; set; }
    }
}
