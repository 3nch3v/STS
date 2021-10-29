using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using STS.Data.Common;

using static STS.Common.GlobalConstants;

namespace STS.Data.Models
{
    public class Department : BaseDeletableModel<int>
    {
        public Department()
        {
            Employees = new HashSet<ApplicationUser>();
            Tickets = new HashSet<Ticket>();
        }

        [Required]
        [MaxLength(DepartmentNameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Employees { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
