using System.ComponentModel.DataAnnotations;

namespace IdentityOrnek.Models
{
    public class SignUpModelView
    {
        [Required(ErrorMessage ="Lütfen username giriniz")]
        public string? UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Lütfen e-maili boş geçmeyin")]
        public string? Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage ="Lütfen telefon numaranızı boş geçmeyin")]
        public string? Phone { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Şifre giriniz")]
        public string? Password { get; set; }

    }
}
