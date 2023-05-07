using System.ComponentModel.DataAnnotations;

namespace IdentityOrnek.Models
{
    public class ForgetPasswordModel
    {
        [Display(Name ="Yeni Şifre")]
        [Required(ErrorMessage =" Şifre Boş bırakılamaz")]
        public string? Password { get; set; }
        [Compare(nameof(Password),ErrorMessage ="Şifre aynı değildir")]
        [Required(ErrorMessage ="Şifre tekrar alanı boş bırakılmaz")]
        [Display(Name ="Şifreyi tekrar giriniz")]
        public string? PasswordConfirm { get; set; }

    }

}
