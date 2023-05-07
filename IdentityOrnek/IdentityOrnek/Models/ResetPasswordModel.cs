using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace IdentityOrnek.Models
{
    public class ResetPasswordModel
    {
       [EmailAddress(ErrorMessage ="Email formatı yanlıştır")]
        [Display(Description ="EMail")]
        public string? EMail { get; set; }
    }
}
