﻿using CoreWebApi.Authorization;
using CoreWebApi.Entities;
using CoreWebApi.Repositories;
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
        private readonly IUserRepository _userRepository;
        private readonly IDeletedTokenRepository _deletedTokenRepository;

        public TokenController(ILogger<TokenController> logger, IUserRepository userRepository, IDeletedTokenRepository deletedTokenRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _deletedTokenRepository = deletedTokenRepository;
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

        /// <summary>
        /// User Logout.
        /// </summary>
        /// <returns></returns>
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
            _deletedTokenRepository.DeleteToken(new DeletedToken
            {
                Jti = jti,
                Exp = exp
            });
            if (!_deletedTokenRepository.Save())
            {
                return StatusCode(500, "将token失效信息存入数据库时出错");
            }
            return Ok();
        }

        /// <summary>
        /// User Login.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(dynamic obj)
        {
            string username = Convert.ToString(obj.username);
            string password = Convert.ToString(obj.password);

            if (_userRepository.VerifyUser(username, password))
            {
                return new ObjectResult(TokenOperator.GenerateToken(username));
            }
            return BadRequest();
        }

    }
}
