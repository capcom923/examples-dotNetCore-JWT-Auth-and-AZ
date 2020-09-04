using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace DotNetJWT_Self_Issue_Token_Auth_and_AZ
{
    public class MinAgeHandler : AuthorizationHandler<MinAgePolicyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinAgePolicyRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "Age"))
            {
                return Task.CompletedTask;
            }
            var age = int.Parse(context.User.FindFirst(x => x.Type == "Age").Value);

            if (age >= requirement.MinAge)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}