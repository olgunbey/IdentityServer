using IdentityOrnek.Models;
using IdentityOrnek.Permissions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityOrnek.Seeds
{
    public  class PermissionSeed
    {
        public static async Task PermissionSeeds(RoleManager<AppRole> roleManager)
        {
            var hasBasicRole = await roleManager.RoleExistsAsync("BasicRole");
            var hasAdvancedRole = await roleManager.RoleExistsAsync("AdvancedRole");
            var hasAdminRole = await roleManager.RoleExistsAsync("AdminRole");
            var hasUpdateRole = await roleManager.RoleExistsAsync("UpdateRole");
          if (!hasBasicRole)
            {
              await  roleManager.CreateAsync(new AppRole() { Name = "BasicRole" });

               AppRole? appRole= await  roleManager.FindByNameAsync("BasicRole");

                await AddReadPermission(appRole!, roleManager);
         
            }
          if(!hasAdvancedRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "AdvancedRole" });

                AppRole? appRole = await roleManager.FindByNameAsync("AdvancedRole");

                await AddReadPermission(appRole!, roleManager);
                await AddCreatePermission(appRole!, roleManager);
            }
            if (!hasAdminRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "AdminRole" });

                AppRole? appRole = await roleManager.FindByNameAsync("AdminRole");

                await AddReadPermission(appRole!, roleManager);
                await AddCreatePermission(appRole!, roleManager);
                await AddDeletePermission(appRole!, roleManager);
                
            }
            if(!hasUpdateRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "UpdateRole" });

                AppRole? appRole = await roleManager.FindByNameAsync("UpdateRole");

                await AddUpdatePermission(appRole!, roleManager);
            }
           
        }
        public static async Task AddReadPermission(AppRole appRole, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permission.Order.Read));
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permission.Catalog.Read));
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permission.Stock.Read));
        }
        public static async Task AddUpdatePermission(AppRole appRole,RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permission.Order.Update));
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permission.Catalog.Update));
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permission.Stock.Update));
        }
        public static async Task AddDeletePermission(AppRole appRole, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permission.Order.Delete));
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permission.Catalog.Delete));
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permission.Stock.Delete));
        }
        public static async Task AddCreatePermission(AppRole appRole, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permission.Order.Create));
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permission.Catalog.Create));
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permission.Stock.Create));
        }
    }
}
