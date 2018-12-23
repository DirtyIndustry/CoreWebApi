using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a secret that needs to be at least 16 characters long"));

        public TokenController(ILogger<TokenController> logger)
        {
            _logger = logger;
        }

        // GET api/token
        [HttpGet]
        public ActionResult Get(string login)
        {
            _logger.LogDebug("User Trying to Login.");
            if (string.IsNullOrEmpty(login))
            {
                return BadRequest();
            }
            try
            {
                var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(login));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
            return Ok();
        }

        // POST api/token
        [HttpPost]
        public IActionResult Create(dynamic obj)
        {
            string username = Convert.ToString(obj.username);
            string password = Convert.ToString(obj.password);

            if (username == "张三" && password == "123")
            {
                return new ObjectResult(GenerateToken(username));
            }
            return BadRequest();
        }

        private string GenerateToken(string username)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
                new Claim("department", "headquarter"),
                new Claim("title", "boss")
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the secret that needs to be at least 16 characeters long for HmacSha256")),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
