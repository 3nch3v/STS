using System.ComponentModel.DataAnnotations;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.Tickets
{
    public class CommentInputModel
    {
        public int TicketId { get; set; }

        [StringLength(CommentMaxLength, MinimumLength = CommentMinLength)]
        public string Content { get; set; }

        public bool sendEmail { get; set; }
    }
}
