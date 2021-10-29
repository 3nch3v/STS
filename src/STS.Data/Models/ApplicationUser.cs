using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

using STS.Data.Common;

using static STS.Common.GlobalConstants;

namespace STS.Data.Models
{

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            Tickets = new HashSet<Ticket>();
            Tasks = new HashSet<EmployeeTask>();
            Roles = new HashSet<IdentityUserRole<string>>();
            Claims = new HashSet<IdentityUserClaim<string>>();
            Logins = new HashSet<IdentityUserLogin<string>>();
        }

        [Required]
        [MaxLength(EmployeeNameMaxLength)]
        public string Vorname { get; set; }

        [Required]
        [MaxLength(EmployeeNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(EmployeePositionNameMaxLength)]
        public string Position { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        [InverseProperty("Employee")]
        public virtual ICollection<Ticket> Tickets { get; set; }

        [InverseProperty("Employee")]
        public virtual ICollection<EmployeeTask> Tasks { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    }
}
