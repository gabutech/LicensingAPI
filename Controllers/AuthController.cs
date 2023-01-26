using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LicensingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LicensingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // authenticate the client
            var authenticated = model.Username == "user1" && model.Password == "password1"; // authenticate the client using the provided credentials

            // if the client is not authenticated
            if (!authenticated)
            {
                return Unauthorized();
            }

            // if the client is authenticated, generate a JWT token
            var token = GenerateJwtToken(model.Username);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenModel model)
        {
            // get the refresh token from the request body
            var refreshToken = model.RefreshToken;
            // validate the refresh token
            if (!IsValidRefreshToken(refreshToken))
            {
                return Unauthorized();
            }

            // invalidate the refresh token
            InvalidateRefreshToken(refreshToken);

            // create a new JWT
            var token = GenerateJwtToken(model.Username);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        private JwtSecurityToken GenerateJwtToken(string userName)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return token;
        }


        private bool IsValidRefreshToken(string token)
        {
            // retrieve the token from the database
            var tokenFromDb = "asdf"; // get the token from the database
                              
            // retrieve the token's expiration time
            var tokenExpiration = DateTime.Now; // get the token's expiration time from the database
            // check if the token has expired
            if (IsTokenExpired(tokenExpiration))
            {
                return false;
            }

            // hash the token from the user if needed
            // ...
            // check if the token from the user matches the token from the database
            return tokenFromDb == token;
        }

        private bool IsTokenExpired(DateTime tokenExpiration)
        {
            return tokenExpiration < DateTime.Now;
        }

        private void InvalidateRefreshToken(string token)
        {
            // remove the token from the database or cache
            // ...
        }

    }
}
