using System.ComponentModel.DataAnnotations;

using STS.Data.Common;

using static STS.Common.GlobalConstants;

namespace STS.Data.Models
{
    public class Status : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(StatusNameMaxLength)]
        public string Name { get; set; }
    }
}
