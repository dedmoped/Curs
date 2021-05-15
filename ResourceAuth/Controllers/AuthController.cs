using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ResourceAuth.Models;

namespace ResourceAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        ApplicationContext db;
        private readonly IOptions<AuthOptions> authOptions;
        public AuthController(IOptions<AuthOptions> options,ApplicationContext connect)
        {
            this.db = connect;
            this.authOptions = options;
        }


        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] Login request)
        {
            var user = AuthenticatedUser(request.Email, request.Password);

            if (user != null)
            {
                var token = GenerateJWT(user);
                return Ok(new { access_token = token, userid=user.Id,username=user.Email});
            }
            return Unauthorized();
        }

        [Route("register")]
        [HttpPost]
        public string Register([FromBody]Accounts acc)
        {
            try
            {
                db.accounts.Add(new Accounts() { Email = acc.Email, Password = acc.Password, RoleId = 1,Mobile= acc.Mobile,Name=acc.Name});
                db.SaveChanges();
                return "Решисрация прошла успешно";
            }
            catch 
            {
                return "Ощибка регистрации";
            }
        }


        private Accounts AuthenticatedUser(string email, string password)
        {
            return db.accounts.SingleOrDefault(k => k.Email == email && k.Password == password);
        }

        private string GenerateJWT(Accounts user)
        {
            var authParams = authOptions.Value;
            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString())
            };
            
                claims.Add(new Claim("role", user.RoleId == 1?"user":"admin"));
            
            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifeTime),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
