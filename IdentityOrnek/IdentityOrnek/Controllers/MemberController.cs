using IdentityOrnek.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

namespace IdentityOrnek.Controllers
{
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IFileProvider fileProvider;
        public MemberController(SignInManager<AppUser> _signInManager,UserManager<AppUser> userManager,IFileProvider fileProvider)
        {
            signInManager = _signInManager;   
            this.userManager = userManager;
            this.fileProvider = fileProvider;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
           // var userClaims = User.Claims.ToList();
           //var user= User.Claims.First(x => x.Type == ClaimTypes.Role && x.Value == "Administrator");
          AppUser? appUser= await userManager.FindByNameAsync(User.Identity!.Name!);

          UserViewModels userViewModels= new UserViewModels() {Email=appUser!.Email, Name=appUser.UserName,Phone=appUser.PhoneNumber,Picture=appUser.Picture };
            
            return View(userViewModels);
        }
        public async Task Logout()
        {
           await signInManager.SignOutAsync();
        }
        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser? currentUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            bool checkOldPassword = await userManager.CheckPasswordAsync(currentUser!, password: passwordChangeViewModel.PasswordOld!);
            if(!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
                return View();
            }
        IdentityResult identityResult= await  userManager.ChangePasswordAsync(currentUser!,passwordChangeViewModel!.PasswordOld!,passwordChangeViewModel!.PasswordNew!);
            if(!identityResult.Succeeded)
            {
                ModelState.AddListErrors(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }

            await userManager.UpdateSecurityStampAsync(currentUser!); //şifre değiştiği için securitystamp değiştirilmeli
            await signInManager.SignOutAsync(); //çıkış yaptırır
         var results=  await signInManager.PasswordSignInAsync(currentUser!, passwordChangeViewModel.PasswordNew!, true, true);

            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirilmiştir";



            return View();
        }

        [Authorize]
        public async Task<IActionResult> UserEdit()
        {

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            AppUser? currentUser =await userManager.FindByNameAsync(User.Identity!.Name!);

            var userEditmodels = new UserEditViewModels()
            {
                Email = currentUser?.Email,
                userName = currentUser?.UserName,
                Phone = currentUser?.PhoneNumber,
                DateTime = currentUser?.DateTime,
                Cinsiyet = currentUser?.Cinsiyet,
            };
                return View(userEditmodels);

        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModels userEditViewModels)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            AppUser? currentUser =await userManager.FindByNameAsync(User.Identity!.Name!);

            if (userEditViewModels.Fotograf!=null && userEditViewModels.Fotograf.Length>0)
            {
                currentUser!.UserName = userEditViewModels.userName;
                currentUser.PhoneNumber = userEditViewModels.Phone;
                currentUser.Email = userEditViewModels.Email;
                currentUser.Cinsiyet = userEditViewModels.Cinsiyet;
                currentUser.City = userEditViewModels.City;
                currentUser.DateTime = userEditViewModels.DateTime;


                var wwwrootFolder = fileProvider.GetDirectoryContents("wwwroot");

                var resimIsmi =$"{Guid.NewGuid()}{Path.GetExtension(userEditViewModels.Fotograf.FileName)}";

                var newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userpictures").PhysicalPath!, resimIsmi);

                using (var stream=new FileStream(newPicturePath,FileMode.Create))
                {
                 await  userEditViewModels.Fotograf.CopyToAsync(stream);
                    currentUser.Picture = resimIsmi;
                }
              IdentityResult identityResult =await userManager.UpdateAsync(currentUser);

                if(!identityResult.Succeeded)
                {
                    ModelState.AddListErrors(identityResult.Errors.Select(x => x.Description));
                    return View();
                }
                await userManager.UpdateSecurityStampAsync(currentUser);
                await signInManager.SignOutAsync();
                if(userEditViewModels.DateTime.HasValue)
                {
                    await signInManager.SignInWithClaimsAsync(currentUser, true, new[] { new Claim("age", userEditViewModels.DateTime.ToString()!) });
                }
                else
                {
                    await signInManager.SignInAsync(currentUser, true);
                }
                TempData["SuccessMessage"] = "Başarıyla güncellendi";

                var userEdit = new UserEditViewModels()
                {
                    userName = currentUser.UserName,
                    Cinsiyet = currentUser.Cinsiyet,
                    DateTime = currentUser.DateTime,
                    Phone=currentUser.PhoneNumber,
                    Email = currentUser.Email,
                    City = currentUser.City,
                };
                return View(userEdit);
              

            }

            return View();
            }

        public IActionResult AccessDenied(string returnUrl)
        {
            ViewBag.message = "Admin rolünde olmadığınız için giriş yapamadınız, lütfen yetkili biriyle iletişime geçin";
            return View();
        }

        [Authorize(Policy ="ExchangePolicy")]
        public IActionResult ExchangePage()
        {
            return View();
        }
        [Authorize(Policy ="AgePolicy")]
        public IActionResult ArtiOnSekizPage()
        {
            return View();
        }
    
    }
}
