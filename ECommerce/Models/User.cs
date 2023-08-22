using Microsoft.AspNetCore.Identity;

namespace ECommerce.Models
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }
        public int Year { get; set; }
    }
}
