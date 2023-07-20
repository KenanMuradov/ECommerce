using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.ViewModels
{
    public class AddTagViewModel
    {
        [Required(ErrorMessage = "Please enter a name.")]
        [StringLength(20,MinimumLength =3)]
        public string Name { get; set; }
    }
}
