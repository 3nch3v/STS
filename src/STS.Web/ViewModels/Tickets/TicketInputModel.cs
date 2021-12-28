using System.Collections.Generic;

namespace STS.Web.ViewModels.Tickets
{
    public class TicketInputModel : BaseTicketInputModel
    {
        public IEnumerable<PriorityViewModel> Priorities { get; set; }

        public IEnumerable<StatusViewModel> Statuses { get; set; }

        public IEnumerable<DepartmentViewModel> Departments { get; set; }
    }
}
