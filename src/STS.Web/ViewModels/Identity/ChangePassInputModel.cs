using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.Identity
{
    public class ChangePassInputModel
    {
        [Required]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        [DisplayName("Old password")]
        public string OldPass { get; set; }

        [Required]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        [DisplayName("New password")]
        public string NewPass { get; set; }

        [Required]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength)]
        [DisplayName("Repeat password")]
        public string RepeatPass { get; set; }
    }
}
