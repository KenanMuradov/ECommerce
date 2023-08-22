using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "You must enter your mail")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
