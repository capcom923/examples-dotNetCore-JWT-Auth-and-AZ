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
            //You can use Postman OAuth2.0 function to get access_token based on https://demo.identityserver.io/
            //Suggest use client id: interactive.public
            var token = HttpContext.Request.Headers["Authorization"];
            var authority = Configuration["OAuth2Config:Authority"];
            var issuer = Configuration["OAuth2Config:Authority"];
            if (TokenValidator.ValidateToken(authority, issuer, token))
            {
                return Ok($"You pass the token validation, so you can access this resource at {DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff zzz")}");
            }
            
            return Unauthorized();
        }
    }
}