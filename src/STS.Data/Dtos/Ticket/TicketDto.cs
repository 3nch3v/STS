namespace STS.Data.Dtos.Ticket
{
    public class TicketDto
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int? DepartmentId { get; set; }

        public int? PriorityId { get; set; }

        public int? StatusId { get; set; }

        public string AssignedToId { get; set; }
    }
}
