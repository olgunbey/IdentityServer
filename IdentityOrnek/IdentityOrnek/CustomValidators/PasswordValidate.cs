using IdentityOrnek.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityOrnek.CustomValidators
{
    public class PasswordValidate : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
           if(user.UserName.ToLower().Contains(password.ToLower()) || password.ToLower().Contains(user.UserName.ToLower()))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError() { Description = "Şifre alanında kullanıcı adı içeremez", Code = "PasswordContainsUserName" }));
            }
           else
            {
                return Task.FromResult(IdentityResult.Success);
            }
        }
    }
}
