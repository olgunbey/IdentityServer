using IdentityOrnek.Areas.Admin.Models;
using IdentityOrnek.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityOrnek.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private UserManager<AppUser> user;
        public HomeController(UserManager<AppUser> _user)
        {
            user = _user;

        }
        public IActionResult Index()
        {
            return View();
        }
        public async  Task<IActionResult> UserList()
        {

        List<AppUser> users=await user.Users.ToListAsync();


            List<UserListModelView> userListModelView = users.Select(x => new UserListModelView()
            {
                Id = x.Id,
                Email = x.Email,
                Name  =x.UserName
            }).ToList();

            return View(userListModelView);
        }
    }
}
