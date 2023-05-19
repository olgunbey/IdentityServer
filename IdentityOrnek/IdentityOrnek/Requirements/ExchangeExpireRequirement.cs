using Microsoft.AspNetCore.Authorization;

namespace IdentityOrnek.Requirements
{
    public class ExchangeExpireRequirement:IAuthorizationRequirement
    {

    }

    public class ExchangeExpireRequirementHandler : AuthorizationHandler<ExchangeExpireRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExchangeExpireRequirement requirement)
        {//claimlerin yönetildigi businesstir 
            

             if(!context.User.HasClaim(x => x.Type == "ExchangeExpireDate")) //burada istediğim claim var mı yok mu kontrolu yapıyorum
            {
                context.Fail();
            }
            var ExchangeExpireDate = context.User.FindFirst(x => x.Type == "ExchangeExpireDate"); //burada istediğim claimi çekiyorum
             
            if (DateTime.Now >Convert.ToDateTime(ExchangeExpireDate!.Value))
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement);
            }



            return Task.CompletedTask;
        }
    }
}
