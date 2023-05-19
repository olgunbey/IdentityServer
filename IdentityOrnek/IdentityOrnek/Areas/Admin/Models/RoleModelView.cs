using System.ComponentModel.DataAnnotations;

namespace IdentityOrnek.Areas.Admin.Models
{
    public class RoleModelView
    {
        [Required(ErrorMessage ="Lütfen rol giriniz")]
        [Display(Name ="Rol giriniz ")]
        public string? Name { get; set; }
    }
}
