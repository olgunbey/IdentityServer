using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace IdentityOrnek.Models
{
    public class RoleTagHelper:TagHelper
    {
        public string? userId { get; set; }
        public UserManager<AppUser> userManager { get; set; }
        public RoleTagHelper(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
        AppUser? appUser=  await userManager.FindByIdAsync(userId!);

        var userRoles= await userManager.GetRolesAsync(appUser!);
            var stringBuilder = new StringBuilder();
            userRoles.ToList().ForEach(role =>
            {
                stringBuilder.Append(@$"<span class='badge bg-secondary' >{role.ToLower()} </span>");
            });

            output.Content.SetHtmlContent(stringBuilder.ToString());

        }
    }
}
