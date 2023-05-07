using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace IdentityOrnek.Models
{
    public class SignInModelView
    {
        [Required(ErrorMessage = "Email boş geçilemez")]
        public string EMail { get; set; }
        [Required(ErrorMessage = "Şifre boş geçilemez")]
        public string Password { get; set; }
    }
}
