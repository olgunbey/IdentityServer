using Microsoft.AspNetCore.Identity;

namespace IdentityOrnek.Localization
{
    public class LocalizationIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email) //erorların türkçesi
        {
            return new() { Code = "DuplicateEmail", Description = $"Bu {email}'e ait hesap bulunmaktadır" };
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            return new() { Code = "DuplicateUserName", Description = $"Bu {userName} kayıdı kayıtlıdır" };
        }
        public override IdentityError PasswordRequiresLower()
        {
            return new() { Code = "PasswordRequiresLower", Description = "Lütfen şifrenizde küçük harf kullanın" };
        }
        public override IdentityError PasswordRequiresUpper()
        {
            return new() { Code = "PasswordRequiresUpper", Description = "Lütfen şifrenizde büyük harf kullanın" };
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new() { Code = "PasswordToShort", Description = $"Şifreniz {length} karakterden kısa olamaz " };
        }
        public override IdentityError PasswordRequiresDigit()
        {
            return new() { Code = "PasswordRequiresDigit", Description = "Lütfen şifrenizde rakam bulundurun" };
        }
    }
}
