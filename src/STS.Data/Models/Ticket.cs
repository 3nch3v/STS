using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using STS.Data.Common;

using static STS.Common.GlobalConstants;

namespace STS.Data.Models
{
    public class Ticket : BaseDeletableModel<int>
    {
        public Ticket()
        {
            Comments = new HashSet<Comment>();
        }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public int PriorityId { get; set; }

        public virtual Priority Priority { get; set; }

        public int StatusId { get; set; }

        public virtual Status Status { get; set; }

        [Required]
        public string EmployeeId { get; set; }

        public virtual ApplicationUser Employee { get; set; }

        public string AssignedToId { get; set; }

        public virtual ApplicationUser AssignedTo { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
