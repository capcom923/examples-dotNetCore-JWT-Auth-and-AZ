using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DotNetJWT_Manually_Validate_Token_Auth.Controllers
{
    [ApiController]
    [Route("api")]
    public class MyController : ControllerBase
    {
        public MyController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpGet]
        [Route("1")]
        public string Get1()
        {
            return $"Anyone can access this resource at {DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff zzz")}";
        }

        [HttpGet]
        [Route("2")]
        public ActionResult<string> Get2()
        {
            //Use below token and set the ValidateLifetime = false in startup.cs if you don't know how to generate a new token
            //Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjNCQTc1RkEzOTJFNERBRkFDMjczN0I1QjQ2NDRBQTg1IiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE1OTczMDUzOTQsImV4cCI6MTU5NzMwODk5NCwiaXNzIjoiaHR0cHM6Ly9kZW1vLmlkZW50aXR5c2VydmVyLmlvIiwiY2xpZW50X2lkIjoiaW50ZXJhY3RpdmUucHVibGljIiwic3ViIjoiMTEiLCJhdXRoX3RpbWUiOjE1OTczMDI5NTMsImlkcCI6ImxvY2FsIiwianRpIjoiNjExMjRERTlCNUFENzk0MUQ2NjM2ODVDMjM4M0RDNjYiLCJzaWQiOiJGMDE1MUJCOTAzMEIwOUNFRERERTY5NkVCQTkxQzRDMiIsImlhdCI6MTU5NzMwNTM5NCwic2NvcGUiOlsib3BlbmlkIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbInB3ZCJdfQ.KhLW_3e95CwEab6i70 - b4XsqGzstTVmL3MM83k1a5cL - CF74jmBfsrjNSyID7peSeMilvTTeC2xIsd8Rv3sYZz - GIrzGtwHHWrv6ZE_yXoRPDqAUntxyEXluAd9ifeLDUMvaO90jqnVk4r4jelsqXmBEPYo9ASEcUcmvy6ZeWOVS2K5nn - vyHvfCiObJ3Yc7ZkiDgwU - C22Xz_mQ3qXotjBmW2fjqgyB2cWW6g6xGOi2TC1CtjEM0R1CyGhnEki1QGePPWTWFN9bHl2TpaLr3PVA0sQyo5wKY54WaxJJAY0xGmS9wu3RF - 6amjIrcK3DwWyzmF4bPz3NqNF9dj5YTg
            var token = HttpContext.Request.Headers["Authorization"];
            var authority = Configuration["OAuth2Config:Authority"];
            var issuer = Configuration["OAuth2Config:Authority"];
            if (TokenValidator.ValidateToken(authority, issuer, token))
            {
                return Ok($"You pass the token validation, so you can access this resource at {DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff zzz")}");
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}