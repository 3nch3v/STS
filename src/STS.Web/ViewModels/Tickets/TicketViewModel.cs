using System;
using System.Collections.Generic;

namespace STS.Web.ViewModels.Tickets
{
    public class TicketViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string PriorityName { get; set; }

        public string StatusName { get; set; }

        public string AssignedToUserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }
    }
}
