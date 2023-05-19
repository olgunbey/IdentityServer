using IdentityOrnek.Models;
using IdentityOrnek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using System.Security.Claims;

namespace IdentityOrnek.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailService emailService;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager,IEmailService emailService)
        {
            userManager = _userManager;
            _logger = logger;
            signInManager = _signInManager;
            this.emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModelView signUpModelView)
        {
            IdentityResult identityResult = await userManager.CreateAsync(new AppUser() { UserName = signUpModelView.UserName, PhoneNumber = signUpModelView.Phone, Email = signUpModelView.Email }, signUpModelView.Password);
            if (!identityResult.Succeeded)
            {
                ModelState.AddListErrors(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }
            TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarıyla gerçekleşti";

            //foreach (IdentityError item in identityResult.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, item.Description);
            //}
            return View();
        }
        public IActionResult SignIn()
        {
  
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModelView signInModelView, string returnUrl = null)
        {

            if (!ModelState.IsValid)
            {
                return View("SignIn");
            }
            AppUser? appUser = await userManager.FindByEmailAsync(signInModelView.EMail!);

            if (appUser == null)
            {
                ModelState.AddModelError("log1", "mail yok");
                return View("SignIn");
            }
            //var signInResult = await signInManager.CheckPasswordSignInAsync(appUser, signInModelView.Password, false);

            var signInResult = await signInManager.PasswordSignInAsync(appUser, signInModelView.Password, true, true);
            //isPersistent => Bu method Kullancı giriş yaptıktan sonra beni hatırla mantıgıyla çalışır, bilgileri cookie' de saklar
            await signInManager.SignInWithClaimsAsync(appUser, true, new[] { new Claim("age", appUser.DateTime!.Value.ToString()!) });
            if (signInResult.Succeeded)
            {
                return View("GirisYapildi"); 
            }
            else
            {
                if (signInResult.IsLockedOut)
                {
                    ModelState.AddModelError("LogTimeOut", "3 dakika kitlendi");
                }
                else
                {
                    int total = await userManager.GetAccessFailedCountAsync(appUser);
                    ModelState.AddModelError("log1", $"{3 - total} hakkınız kaldı");
                }
                return View("SignIn");
                
            }
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
         AppUser? appUser= await  userManager.FindByEmailAsync(resetPasswordModel.EMail!);
            if(appUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu email adresine ait kullanıcı bulunmamaktadır");
                return View();
            }
            string passwordResetToken =await userManager.GeneratePasswordResetTokenAsync(appUser);

            string? url = Url.Action("ForgetPassword", "Home", new {userID=appUser.Id,Token=passwordResetToken },HttpContext.Request.Scheme);

            await emailService.SendResetEmail(url!, resetPasswordModel.EMail!); //IEmailService IOC'ten geliyor


            TempData["SuccessMessage"] = "Şifre yenileme linki, e posta adresine gönderilmiştir";
            return RedirectToAction(nameof(ResetPassword));
        }

        public IActionResult ForgetPassword(string userID,string token)
        {
            TempData["userId"] = userID;
            TempData["token"]=token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordModel forgetPasswordModel)
        {
            string userId = TempData["userID"]!.ToString()!;
            string token = TempData["token"]!.ToString()!;

            if(userId==null || token==null)
            {
                throw new Exception("Hata meydana geldi");
            }

            AppUser? hasUser = await userManager.FindByIdAsync(userId);
            if(hasUser==null)
            {
                ModelState.AddModelError(string.Empty, "Böyle bir kullanıcı yoktur");
                return View();
            }
            IdentityResult? result= await userManager.ResetPasswordAsync(hasUser,token!,forgetPasswordModel.Password!);
            if(result.Succeeded)
            {
              //IdentityResult identityResult= await userManager.UpdateSecurityStampAsync(hasUser); //securityStamAsync => kullanıcının şifre, email, önemli bilgileri güncellendikçe değişir
              //  if(identityResult.Succeeded)
              //  {
                    TempData["SuccessMessage"] = "Şifreniz başarıyla yenilendi";
                    return View();
                //}
                //return View();
               
            }
            else
            {
                ModelState.AddListErrors(result.Errors.Select(e => e.Description).ToList());
                return View();
            }
           
        }
        public IActionResult GetUserList()
        {
            List<AppUser> AppUsers = userManager.Users.ToList();
          List<UserViewModels> userViewModels=  AppUsers.Select(x => new UserViewModels()
            {
                Phone=x.PhoneNumber,
                Email=x.Email,
                Name=x.UserName
            }).ToList();
            return View(userViewModels);
        }


    }
}