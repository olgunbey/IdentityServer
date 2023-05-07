using Microsoft.AspNetCore.Identity;

namespace IdentityOrnek.Models
{
    public class AppUser:IdentityUser
    {
        public string? City { get; set; }
    }
}
