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
            //Use below token and set the ValidateLifetime = false in startup.cs if you don't know how to generate a new token
            //Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjNCQTc1RkEzOTJFNERBRkFDMjczN0I1QjQ2NDRBQTg1IiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE1OTczMDUzOTQsImV4cCI6MTU5NzMwODk5NCwiaXNzIjoiaHR0cHM6Ly9kZW1vLmlkZW50aXR5c2VydmVyLmlvIiwiY2xpZW50X2lkIjoiaW50ZXJhY3RpdmUucHVibGljIiwic3ViIjoiMTEiLCJhdXRoX3RpbWUiOjE1OTczMDI5NTMsImlkcCI6ImxvY2FsIiwianRpIjoiNjExMjRERTlCNUFENzk0MUQ2NjM2ODVDMjM4M0RDNjYiLCJzaWQiOiJGMDE1MUJCOTAzMEIwOUNFRERERTY5NkVCQTkxQzRDMiIsImlhdCI6MTU5NzMwNTM5NCwic2NvcGUiOlsib3BlbmlkIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbInB3ZCJdfQ.KhLW_3e95CwEab6i70 - b4XsqGzstTVmL3MM83k1a5cL - CF74jmBfsrjNSyID7peSeMilvTTeC2xIsd8Rv3sYZz - GIrzGtwHHWrv6ZE_yXoRPDqAUntxyEXluAd9ifeLDUMvaO90jqnVk4r4jelsqXmBEPYo9ASEcUcmvy6ZeWOVS2K5nn - vyHvfCiObJ3Yc7ZkiDgwU - C22Xz_mQ3qXotjBmW2fjqgyB2cWW6g6xGOi2TC1CtjEM0R1CyGhnEki1QGePPWTWFN9bHl2TpaLr3PVA0sQyo5wKY54WaxJJAY0xGmS9wu3RF - 6amjIrcK3DwWyzmF4bPz3NqNF9dj5YTg

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
    }
}
