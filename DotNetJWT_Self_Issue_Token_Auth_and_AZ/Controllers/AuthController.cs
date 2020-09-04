using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DotNetJWT_Self_Issue_Token_Auth_and_AZ.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        public AuthController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpGet]
        [Route("auth")]
        public string Get1()
        {
            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, "subid-1"),
                    new Claim(JwtRegisteredClaimNames.Email, "a@b.com"),
                    new Claim(ClaimTypes.Role, "role1"),
                    new Claim(ClaimTypes.Role, "role2"),
                    new Claim("Claim1", "anything"),
                    new Claim("Age", "28")
                };

            string pem = Configuration["InternalToken:RsaPrivateKeyInPem"];
            var privateKey = RsaUtils.CreateRsaPrivateKeyByPem(pem);
            SigningCredentials creds = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);

            double expireInMin;
            if (!double.TryParse(Configuration["InternalToken:ExpireInMin"], out expireInMin))
            {
                expireInMin = 45 * 24 * 60;
            }

            var token = new JwtSecurityToken(null,
              null,
              claims,
              expires: DateTime.Now.AddMinutes(expireInMin),
              signingCredentials: creds);

            var authToken = new JwtSecurityTokenHandler().WriteToken(token);

            return authToken;
        }
    }
}
