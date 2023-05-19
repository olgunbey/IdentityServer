using System.ComponentModel.DataAnnotations;

namespace IdentityOrnek.Models
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name ="Eski Şifre: ")]
        [Required(ErrorMessage ="Eski şifrenizi giriniz")]
        [MinLength(7, ErrorMessage = "Lütfen en az 7 karakter girin")]
        public string? PasswordOld { get; set; }

        [Display(Name ="Yeni Şifre: ")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Lütfen şifrenizi giriniz")]
        [MinLength(7, ErrorMessage = "Lütfen en az 7 karakter girin")]
        public string? PasswordNew { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(PasswordNew),ErrorMessage ="Şifre aynı değildir")]
        [Display(Name ="Yeni şifre tekrar: ")]
        [Required(ErrorMessage ="Lütfen şifrenizi tekrar girin")]
        [MinLength(7,ErrorMessage ="Lütfen en az 7 karakter girin")]
        public string? PasswordConfirm { get; set; }
    }
}
