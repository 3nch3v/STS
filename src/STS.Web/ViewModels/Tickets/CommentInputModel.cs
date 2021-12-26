using System.ComponentModel.DataAnnotations;

namespace STS.Web.ViewModels.Tickets
{
    public class CommentInputModel
    {
        public int TicketId { get; set; }

        [StringLength(2000, MinimumLength = 2)]
        public string Content { get; set; }
    }
}
