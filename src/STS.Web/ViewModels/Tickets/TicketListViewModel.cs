using System;

namespace STS.Web.ViewModels.Tickets
{
    public class TicketListViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string PriorityName { get; set; }

        public string StatusName { get; set; }

        public string AssignedToUserName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
