using IdentityOrnek.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace IdentityOrnek.Requirements
{
    public class OrderPermissionReadAndDeleteRequirement:IAuthorizationRequirement
    {
    }
    public class OrderPermissionReadAndDeleteRequirementHandler : AuthorizationHandler<OrderPermissionReadAndDeleteRequirement>
    {
        //Permission.Order.Read
        //     Permission.Stock.Update
        //    Permission.Order.Delete
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrderPermissionReadAndDeleteRequirement requirement)
        {
            if(!context.User.HasClaim(x=>x.Type=="Permission"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
          var PermissionClaims= context.User.Claims.Where(x => x.Type == "Permission").ToList();
            List<string> PermissionValue = new List<string>();
           PermissionClaims.ToList().ForEach(x => 
            {
                PermissionValue.Add(x.Value);
            }); 

            if(!(PermissionValue.Contains(Permission.Order.Read)&&PermissionValue.Contains(Permission.Stock.Update) && PermissionValue.Contains(Permission.Order.Delete)))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;



        }
    }
}
