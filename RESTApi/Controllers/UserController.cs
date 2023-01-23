using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTApi.Models;
using System.Security.Claims;

namespace RESTApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("Admins")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndPoint()
        {
            var currentUser = GetUser();
            return Ok($"Welcome {currentUser.FirstName}, You are an {currentUser.Role}");
        }

        [HttpGet("Clients")]
        [Authorize(Roles = "Client")]
        public IActionResult OtherUsersEndPoint()
        {
            var currentUser = GetUser();
            return Ok($"Welcome {currentUser.FirstName}, You are an {currentUser.Role}");
        }
        private User GetUser()
        {
            //we try to get the identity from httpcontext identity
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity != null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    UserName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email) ?.Value,
                    FirstName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName) ?.Value,
                    LastName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Surname) ?.Value,
                    Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role) ?.Value
                };
            }
            return null ?? new User();
        }
    }
}
