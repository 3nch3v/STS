using System.ComponentModel.DataAnnotations;

namespace STS.Web.ViewModels.Identity
{
    public class LoginInputModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
