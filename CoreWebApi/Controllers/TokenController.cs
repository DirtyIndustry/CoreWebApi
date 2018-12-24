using CoreWebApi.Authorization;
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

        public TokenController(ILogger<TokenController> logger)
        {
            _logger = logger;
        }

        // GET api/token
        [HttpGet]
        [Authorize(Policy="Jti")]
        public ActionResult Get()
        {
            _logger.LogDebug("Get Values From Token.");
            var jti = User.FindFirst("jti")?.Value;
            var username = User.FindFirst("sub")?.Value;
            if (jti == null)
            {
                return NotFound();
            }

            // save jti (and username) into database

            return Ok(jti);
        }

        // POST api/token
        [HttpPost]
        public IActionResult Create(dynamic obj)
        {
            string username = Convert.ToString(obj.username);
            string password = Convert.ToString(obj.password);

            if (username == "张三" && password == "123")
            {
                return new ObjectResult(TokenOperator.GenerateToken(username));
            }
            return BadRequest();
        }

    }
}
