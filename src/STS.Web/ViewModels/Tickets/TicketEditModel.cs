namespace STS.Web.ViewModels.Tickets
{
    public class TicketEditModel
    {
        public int Id { get; set; }

        public int? StatusId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AssignedToId { get; set; }

        public int? PriorityId { get; set; }

        public int? DepartmentId { get; set; }
    }
}
