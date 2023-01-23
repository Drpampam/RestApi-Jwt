﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RESTApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RESTApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("No User Found");
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            /*claims holds useful information about our user, without 
             * having to request details about our user */
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserName), 
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.NameIdentifier,user.LastName),
                new Claim(ClaimTypes.NameIdentifier,user.Role),

            };
            var token = new JwtSecurityToken(_config["Jwt:Key"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials:credentials);

            //returns the generated token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(UserLogin userLogin)
        {
            var currentUser = MockData.MockData.Users.FirstOrDefault(x => x.UserName.ToLower() ==
            userLogin.UserName.ToLower() && x.Password == userLogin.Passoword);

            if (currentUser != null) return currentUser;
            return null;
        }
    }
}
