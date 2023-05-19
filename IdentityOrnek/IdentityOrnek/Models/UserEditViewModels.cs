using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace IdentityOrnek.Models
{
    public class UserEditViewModels
    {
        [Display(Name ="Kullanıcı Adı: ")]
        [Required(ErrorMessage ="Lütfen Kullanıcı adını boş bırakmayınız")]
        public string? userName { get; set; }
        [Display(Name = "Şehir: ")]
        [Required(ErrorMessage = "Lütfen şehir bilgisini giriniz")]
        public string? City { get; set; }
        [Display(Name = "EMail: ")]
        [Required(ErrorMessage = "Lütfen Email bilgisini giriniz")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Display(Name = "Cinsiyet: ")]
        [Required(ErrorMessage = "Lütfen cinsiyet seçiniz")]
        public Gender? Cinsiyet { get; set; }
        [Display(Name ="Dogum tarihi: ")]
        [Required(ErrorMessage ="Lütfen doğum tarihini giriniz")]
        [DataType(DataType.Date)]
        public DateTime? DateTime { get; set; }
        [Display(Name = "Telefon numarası: ")]
        [Required(ErrorMessage = "Lütfen telefon numarasını giriniz")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
        [Display(Name ="Fotoğraf yükleyiniz")]
        [Required(ErrorMessage ="Lütfen fotoğraf seçiniz")]
        
        public IFormFile? Fotograf { get; set; }



    }
}
