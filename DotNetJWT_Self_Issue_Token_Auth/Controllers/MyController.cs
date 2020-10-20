using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetJWT_Self_Issue_Token_Auth.Controllers
{
    [ApiController]
    [Route("api")]
    public class MyController : ControllerBase
    {
        private IHttpContextAccessor _httpContextAccessor;
        public MyController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

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
        [Authorize(AuthenticationSchemes = "MyBearer")]
        public string Get3()
        {
            var name = _httpContextAccessor.HttpContext.User.Identity.Name;
            return $"You ({name}) pass the token validation, so you can access this resource at {DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff zzz")}";
        }
    }
}
