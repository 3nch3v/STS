using System.ComponentModel.DataAnnotations;

using STS.Web.Infrastructure.ValidationAttributes;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.Tickets
{
    public class TicketEditModel
    {
        public int Id { get; set; }

        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; }

        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; set; }

        [StatusId]
        public int? StatusId { get; set; }

        [PriorityId]
        public int? PriorityId { get; set; }

        [DepartmentId]
        public int? DepartmentId { get; set; }

        public string AssignedToId { get; set; }
    }
}
