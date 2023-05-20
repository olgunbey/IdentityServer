using IdentityOrnek.Areas.Admin.Models;
using IdentityOrnek.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace IdentityOrnek.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        public RoleController(RoleManager<AppRole>  _roleManager,UserManager<AppUser> userManager)
        {
            roleManager = _roleManager;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleCreate()
        {
            return View();
        }
        [Authorize(Roles = "Administrator,Yönetici")]
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModelView roleModelView)
        {
          IdentityResult identityResult=await  roleManager.CreateAsync(new AppRole() { Name= roleModelView.Name });
            if(!identityResult.Succeeded)
            {
                ModelState.AddListErrors(identityResult.Errors.Select(e => e.Description).ToList());
                return View();
            }
            return RedirectToAction(nameof(RoleController.Index));
        }


        public IActionResult RoleList()
        {
          List<AppRole> appRoles= roleManager.Roles.ToList();
          List<RoleList> roleLists=  appRoles.Select(appRole => new RoleList()
            {
                ID=appRole.Id,
                Name=appRole.Name
            }).ToList();
            return View(roleLists);
        }
        public async Task<IActionResult> RoleUpdate(string id)
        {
          AppRole? appRole= await roleManager.FindByIdAsync(id);
            RoleUpdateModelView roleUpdateModelView = new RoleUpdateModelView();
            roleUpdateModelView.Name = appRole!.Name;
            return View(roleUpdateModelView);
        }
        //[Authorize(Roles = "Hademe,Yönetici")]
        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleUpdateModelView roleUpdateModelView,string id)
        {
         AppRole appRole= await roleManager.FindByIdAsync(id); //değişecek approle
            appRole!.Name=roleUpdateModelView.Name;
         IdentityResult identityResult=  await roleManager.UpdateAsync(appRole);
            if(!identityResult.Succeeded)
            {
                ModelState.AddListErrors(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }
            TempData["SuccessMessage"] = "Rol başarıyla güncellendi";
            return View();
        }
        public async Task<IActionResult> RoleDelete(string id)
        {
            AppRole? roleDelete = await roleManager.FindByIdAsync(id);

            IdentityResult identityResult=await roleManager.DeleteAsync(roleDelete!);

            if (!identityResult.Succeeded)
            {
                ModelState.AddListErrors(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }
            return RedirectToAction(nameof(RoleController.RoleList));
        }
        //[Authorize(Roles = "Hademe")]
        [HttpGet]
        public async Task<IActionResult> AssignRoleToUser(string id)
        {
         AppUser? appUser=await userManager.FindByIdAsync(id);
            ViewBag.userId = id;

            List<AppRole> roles = await roleManager.Roles.ToListAsync();

            var roleViewModel = new List<AssignRoleToUserModelView>();

            var userRoles =await userManager.GetRolesAsync(appUser!);

            foreach (var role in roles)
            {
                var AssignRoleTouserModelView = new AssignRoleToUserModelView() { Id = role.Id, RoleName = role.Name! };
                if(userRoles.Contains(role.Name!))
                {
                    AssignRoleTouserModelView.Exist = true;
                }
                roleViewModel.Add(AssignRoleTouserModelView);
            }
            return View(roleViewModel);

        }


        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(List<AssignRoleToUserModelView> requestList,string userId)
        {
            var userToAssignRole=await userManager.FindByIdAsync(userId);

            foreach (var item in requestList)
            {
                if(item.Exist)
                {
                   await userManager.AddToRoleAsync(userToAssignRole, item.RoleName);
                }
                else
                {
                    await userManager.RemoveFromRoleAsync(userToAssignRole, item.RoleName);
                }
            }
            return View(requestList);
        }

        [Authorize(Policy = "AnkaraPolicy")]
        public IActionResult GetClaimList()
        {
            var user = User.Claims.Select(x => new ClaimViewModel()
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value
            }).ToList();

            return View(user);
        }


    }
}
