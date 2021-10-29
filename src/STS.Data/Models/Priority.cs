using System.ComponentModel.DataAnnotations;

using STS.Data.Common;

using static STS.Common.GlobalConstants;

namespace STS.Data.Models
{
    public class Priority : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(PriorityNameMaxLength)]
        public string Name { get; set; }
    }
}
