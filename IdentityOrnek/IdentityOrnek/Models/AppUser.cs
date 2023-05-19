using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IdentityOrnek.Models
{
    public class AppUser:IdentityUser
    {
        public string? City { get; set; }
        public Gender? Cinsiyet { get; set; }
        [DataType(DataType.DateTime)]
        
        public DateTime? DateTime { get; set; }
        public string? Picture { get; set; }
    }
}
