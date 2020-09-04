using Microsoft.AspNetCore.Authorization;

namespace DotNetJWT_Self_Issue_Token_Auth_and_AZ
{
    public class MinAgePolicyRequirement : IAuthorizationRequirement
    {
        public int MinAge { get; set; }
        public MinAgePolicyRequirement(int minAge)
        {
            MinAge = minAge;
        }
    }
}