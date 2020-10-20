using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace DotNetJWT_Self_Issue_Token_Auth_and_AZ.AuthEvents
{
    public class MyAuthenticateEvents : JwtBearerEvents
    {
        /// <summary>
        /// Executed after the user JWT token is validated
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenValidated(TokenValidatedContext context)
        {
            //if goes here, the identity.IsAuthenticated will be definitely true
            var identity = context.Principal.Identities.FirstOrDefault();
            if (identity != null &&
                string.Equals(identity.Name, "myName1", StringComparison.InvariantCultureIgnoreCase))
            {
                identity.AddClaims(new List<Claim>()
                {
                    new Claim("ClaimAddDynamically", "anything")
                });
            }

            return Task.CompletedTask;
        }

    }
}
