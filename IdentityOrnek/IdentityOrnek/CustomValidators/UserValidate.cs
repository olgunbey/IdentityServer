using IdentityOrnek.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityOrnek.CustomValidators
{
    public class UserValidate : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            if(int.TryParse(user.UserName[0].ToString(), out _)) //username'nin ilk karakteri bir int degerse yani rakamsa
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = "FirstCharacterDigit", Description = "Lütfen kullanıcı adınızın ilk karakterine rakam girmeyiniz!", }));
            }
            return Task.FromResult(IdentityResult.Success);

        }
    }
}
