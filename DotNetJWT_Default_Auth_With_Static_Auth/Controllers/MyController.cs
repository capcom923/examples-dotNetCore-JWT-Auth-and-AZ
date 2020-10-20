using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetJWT_Default_Auth_With_Static_Auth.Controllers
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        public string Get2()
        {
            //You can use Postman OAuth2.0 function to get access_token based on https://demo.identityserver.io/
            //Suggest use client id: interactive.public
            //Or add a header ["auth_token","abc"] to pass the static auth align with the startup.cs 
            return $"You pass the token validation, so you can access this resource at {DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff zzz")}";
        }
    }
}
