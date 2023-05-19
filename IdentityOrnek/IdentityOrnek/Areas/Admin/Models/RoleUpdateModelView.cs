using System.ComponentModel.DataAnnotations;

namespace IdentityOrnek.Areas.Admin.Models
{
    public class RoleUpdateModelView
    {
        [Required(ErrorMessage ="Lütfen bir rol giriniz")]
        [Display(Name="Rol")]
        public string? Name { get; set; }
    }
}
