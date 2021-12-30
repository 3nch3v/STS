using System;

namespace STS.Data.Dtos.Ticket
{
    public class TicketListingDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string PriorityName { get; set; }

        public string StatusName { get; set; }

        public string AssignedToUserName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
