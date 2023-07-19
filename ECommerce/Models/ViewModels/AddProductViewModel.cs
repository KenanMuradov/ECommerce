namespace ECommerce.Models.ViewModels
{
    public class AddProductViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public int[] TagIds { get; set; }
    }
}
