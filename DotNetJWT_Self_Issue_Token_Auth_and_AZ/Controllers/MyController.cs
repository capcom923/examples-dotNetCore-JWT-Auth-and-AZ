using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetJWT_Self_Issue_Token_Auth_and_AZ.Controllers
{
    [ApiController]
    [Route("api")]
    public class MyController : ControllerBase
    {
        [HttpGet]
        [Route("1")]
        public string Get1()
        {
            return $"Anyone can access this resource at {DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff zzz")}";
        }

        [HttpGet]
        [Route("2")]
        [Authorize(AuthenticationSchemes = "MyBearer")]
        public string Get2()
        {
            return $"You pass the token validation, so you can access this resource at {DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff zzz")}";
        }

        [HttpGet]
        [Route("3")]
        [Authorize(AuthenticationSchemes = "MyBearer", Roles ="role3")]//Should be 403 since the token payload does not has role3 
        public string Get3()
        {
            return $"You pass the token validation and authorization, so you can access this resource at {DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff zzz")}";
        }

        [HttpGet]
        [Route("4")]
        [Authorize(AuthenticationSchemes = "MyBearer", Roles = "role1")]
        public string Get4()
        {
            return $"You pass the token validation and authorization, so you can access this resource at {DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff zzz")}";
        }

        [HttpGet]
        [Route("5")]
        [Authorize(AuthenticationSchemes = "MyBearer", Policy ="Claim1Policy")]
        public string Get5()
        {
            return $"You pass the token validation and authorization of policy, so you can access this resource at {DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff zzz")}";
        }

        [HttpGet]
        [Route("6")]
        [Authorize(AuthenticationSchemes = "MyBearer", Policy = "Role1Policy")]
        public string Get6()
        {
            return $"You pass the token validation and authorization of policy, so you can access this resource at {DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff zzz")}";
        }

        [HttpGet]
        [Route("7")]
        [Authorize(AuthenticationSchemes = "MyBearer", Policy = "IsInRolePolicy")]
        public string Get7()
        {
            return $"You pass the token validation and authorization of policy, so you can access this resource at {DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff zzz")}";
        }

        [HttpGet]
        [Route("8")]
        [Authorize(AuthenticationSchemes = "MyBearer", Policy = "CustomPolicyRequirement")]
        public string Get8()
        {
            return $"You pass the token validation and authorization of policy, so you can access this resource at {DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff zzz")}";
        }

        [HttpGet]
        [Route("9")]
        [Authorize(AuthenticationSchemes = "MyBearer", Policy = "DynamicClaimPolicy")]
        public string Get9()
        {
            return $"You pass the token validation and authorization of policy, so you can access this resource at {DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff zzz")}";
        }
    }
}
