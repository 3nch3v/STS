using System.Collections.Generic;

namespace STS.Web.ViewModels.Tickets
{
    internal class TicketViewModel
    {
        public int id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string PriorityName { get; set; }

        public string StatusName { get; set; }

        public string AssignedToUserName { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }
    }
}
