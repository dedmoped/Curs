using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ResourceAuth.Help;
using ResourceAuth.Models;

namespace ResourceAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        ApplicationContext db;
        private readonly IOptions<AuthOptions> authOptions;
        private IServiceProvider serviceScope;
        public AuthController(IOptions<AuthOptions> options,ApplicationContext connect,IServiceProvider serviceScope)
        {
            this.db = connect;
            this.authOptions = options;
            this.serviceScope = serviceScope;
        }


        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] Login request)
        {
            var user = AuthenticatedUser(request.Email, request.Password);

            if (user != null && user.IsVerified)
            {
                var token = GenerateJWT(user);
                return Ok(new { access_token = token, userid=user.Id,username=user.Email});
            }
            return Unauthorized();
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register([FromBody]Accounts acc)
        {
            
            try
            {
                if (db.accounts.Where(x => x.Email == acc.Email).ToList().Count > 0)
                {
                    return BadRequest(new { message = $"{acc.Email} уже занят" });
                }
                ;
                var user = db.accounts.Add(new Accounts() { Email = acc.Email, Password = Crypto.HashPassword(acc.Password), RoleId = 1, Mobile = acc.Mobile, Name = acc.Name, VerificationCode = GenerateVerificationCode() });
                db.SaveChanges();
                string Url = $"http://{Request.Host}/verify?code={user.Entity.VerificationCode}&userId={user.Entity.Id}";
                var send = Email.EmailSend(user.Entity.Email, "Подтверждение почты", "<a href ='"+Url+"'> fddf </a>", serviceScope.CreateScope());
                send.Wait();
                return Ok(new {message="Решисрация прошла успешно"});
            }
            catch 
            {
                return BadRequest(new { message = "Ошибка регистрации" });
            }
        }


        private Accounts AuthenticatedUser(string email, string password)
        {
            Accounts accounts = db.accounts.SingleOrDefault(k => k.Email == email);
            if (Crypto.VerifyHashedPassword(accounts.Password, password))
                return accounts;
            return null;
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

        [HttpGet("verify")]
        public ActionResult VerifyUser(string Code, int userId)
        {

                var user = db.accounts.FirstOrDefault(x => x.Id == userId);
                if (user != null)
                {
                    if (Code == user.VerificationCode)
                    {
                        user.VerificationCode = string.Empty;
                        user.IsVerified = true;
                        db.SaveChanges();
                        return Ok();
                    }
                }
                return BadRequest();
        }

        private string GenerateVerificationCode()
        {
            var md5 = MD5.Create();
            var codeBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
            var code = Convert.ToBase64String(codeBytes).Replace("+","");
            return code;
        }
    }
}
