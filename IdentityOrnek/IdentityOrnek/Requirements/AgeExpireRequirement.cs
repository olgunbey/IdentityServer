using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityOrnek.Requirements
{
    public class AgeExpireRequirement:IAuthorizationRequirement
    {
        public int ThresholdAge { get; set; }
    }
    public class AgeExpiredRequirementHandler : AuthorizationHandler<AgeExpireRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeExpireRequirement requirement)
        {
          if(!context.User.HasClaim(x=>x.Type=="age"))
             {
                context.Fail();
                return Task.CompletedTask;
            }
          Claim? claim= context.User.Claims.FirstOrDefault(x => x.Type == "age");
            
            var today= DateTime.UtcNow;
            var birthDate = Convert.ToDateTime(claim!.Value);

            var age=today.Year-birthDate.Year;
            if(birthDate > today.AddYears(-age))
            {
                age--;
            }

            if(requirement.ThresholdAge>age)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
