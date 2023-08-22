using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Enter your Email address")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
