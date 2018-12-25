using CoreWebApi.Authorization;
using CoreWebApi.Entities;
using Microsoft.AspNetCore.Authorization;
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
        private readonly EntranceContext _context;

        public TokenController(ILogger<TokenController> logger, EntranceContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET api/token
        [HttpGet]
        [Authorize(Policy="Jti")]
        public ActionResult Get()
        {
            _logger.LogDebug("Get Values From Token.");
            var jti = User.FindFirst("jti")?.Value;
            var expstring = User.FindFirst("exp")?.Value;
            var exp = TokenOperator.UnixTimeStampToDateTime(expstring);
            if (jti == null)
            {
                return NotFound();
            }

            // save jti (and username) into database

            return Ok(jti);
        }

        [HttpDelete]
        [Authorize(Policy = "Jti")]
        public IActionResult Delete()
        {
            var jti = User.FindFirst("jti")?.Value;
            var exp = TokenOperator.UnixTimeStampToDateTime(User.FindFirst("exp")?.Value);
            if (jti == null)
            {
                return NotFound();
            }
            _context.DeletedTokens.Add(new DeletedToken()
            {
                Jti = jti,
                Exp = exp
            });
            _context.SaveChanges();
            return Ok();
        }

        // POST api/token
        [HttpPost]
        public IActionResult Post(dynamic obj)
        {
            string username = Convert.ToString(obj.username);
            string password = Convert.ToString(obj.password);

            if (_context.Users.FirstOrDefault(o => o.UserName == username)?.Password == password)
            {
                return new ObjectResult(TokenOperator.GenerateToken(username));
            }
            return BadRequest();
        }

    }
}
