using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.ViewModels
{
    public class AddProductViewModel
    {
        [Required(ErrorMessage ="Please enter the name.")]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter the description.")]
        [StringLength(255, MinimumLength = 50)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please choose the image.")]
        public IFormFile ImageUrl { get; set; }
        [Required(ErrorMessage = "Please choose the Category.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please choose at least one tag.")]
        public int[] TagIds { get; set; }
    }
}
