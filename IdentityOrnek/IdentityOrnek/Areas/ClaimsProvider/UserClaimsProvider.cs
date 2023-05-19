using IdentityOrnek.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityOrnek.Areas.ClaimsProvider
{
    public class UserClaimsProvider : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;
        public UserClaimsProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {//ClaimsPrincipal nesnesi, uygulama tarafından kullanıcının kimlik bilgilerine 
         //kullanıcının yetkilendirilmesine erişmeyi sağlar
         //Burada uygun claimleri oluştururuz
         //her f5 atılınca bu fonksiyon otamatikmen devreye girer ve her istek için bir claim oluşturur

            var identityUser = principal.Identity as ClaimsIdentity;
            AppUser? appUser=await _userManager.FindByNameAsync(identityUser!.Name!);
            if(!principal.HasClaim(x=>x.Type=="city"))
            {
                if(appUser?.City==null)
                {
                    return principal;
                }
                var cityClaim= new Claim("city",appUser!.City!);
                identityUser.AddClaim(cityClaim);
            }
            return principal;
        }
    }
}
