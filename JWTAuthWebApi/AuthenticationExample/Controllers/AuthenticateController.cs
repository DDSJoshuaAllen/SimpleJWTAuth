using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenticationExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private JWTSettings _jwtSettings;

        public AuthenticateController(IOptions<JWTSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        // POST api/<AuthenticateController>
        [HttpPost]
        public IActionResult Post(AuthenticateRequest request)
        {
            //-- Basic validation.
            if (request == null)
            {
                return GetInvalidUsernamePasswordReponse();
            }


            //-- Getting the user should be inside a database.
            //-- For the purpose of the example the user model is hardcoded & password is plain text.
            UserModel user = GetUserByName(request.Username);

            if (user == null || !ValidPassword(user, request.Password))
            {
                return GetInvalidUsernamePasswordReponse();
            }

            return Ok(new AuthenticateResponse()
            {
                User = user,
                Token = GenerateJWT(user)
            });
        }

        /// <summary>
        /// Generate a JWT and add 2 claim `username` & `role`
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GenerateJWT(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("role", user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer, //-- Add the same issue that's in `Startup.cs`
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private IActionResult GetInvalidUsernamePasswordReponse()
        {
            return BadRequest("Invalid username or password.");
        }

        private UserModel GetUserByName(string username)
        {
            return new UserModel { Username = "test", Password = "test", Role = 1 };
        }

        private bool ValidPassword(UserModel user, string attemptedPassword)
        {
            return user.Password == attemptedPassword;
        }
    }

    public class UserModel
    {
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public int Role { get; set; }
    }

    public class AuthenticateRequest
    {
        public string Username { get; set; } = "test";
        public string Password { get; set; } = "test";
    }

    public class AuthenticateResponse
    {
        public UserModel User { get; internal set; }
        public string Token { get; internal set; }
    }
}
