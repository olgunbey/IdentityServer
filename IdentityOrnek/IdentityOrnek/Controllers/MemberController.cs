using IdentityOrnek.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityOrnek.Controllers
{
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        public MemberController(SignInManager<AppUser> _signInManager)
        {
            signInManager = _signInManager;   
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        public async Task Logout()
        {
           await signInManager.SignOutAsync();
        }
    
    }
}
