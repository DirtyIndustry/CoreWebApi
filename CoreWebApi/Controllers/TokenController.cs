using AutoMapper;
using CoreWebApi.Authorization;
using CoreWebApi.Caching;
using CoreWebApi.Dtos;
using CoreWebApi.Entities;
using CoreWebApi.Helpers;
using CoreWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
        private readonly IUnitOfWork<CompanyContext> _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly IDeletedTokenRepository _deletedTokenRepository;
        private readonly IDeletedTokenCache _deletedTokenCache;

        public TokenController(ILogger<TokenController> logger,
            IUnitOfWork<CompanyContext> unitOfWork,
            IUserRepository userRepository,
            ILoginRepository loginRepository,
            IDeletedTokenRepository deletedTokenRepository,
            IDeletedTokenCache deletedTokenCache)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _loginRepository = loginRepository;
            _deletedTokenRepository = deletedTokenRepository;
            _deletedTokenCache = deletedTokenCache;
        }

        // GET api/token
        [HttpGet]
        [Authorize(Policy="Jti")]
        public ActionResult Get()
        {
            _logger.LogDebug("Get Values From Token.");
            var jti = User.FindFirst("jti")?.Value;
            var company = User.FindFirst("company")?.Value;
            var department = User.FindFirst("department")?.Value;
            var position = User.FindFirst("position")?.Value;
            var expstring = User.FindFirst("exp")?.Value;
            var exp = TokenOperator.UnixTimeStampToDateTime(expstring);
            if (jti == null)
            {
                return NotFound();
            }

            // save jti (and username) into database

            return Ok(jti + ", expire:" + exp + ", company:" + company + ", department:" + department + ", position:" + position);
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
            _deletedTokenCache.DeleteToken(new DeletedToken
            {
                Jti = jti,
                Exp = exp
            });
            //_deletedTokenRepository.DeleteToken(new DeletedToken
            //{
            //    Jti = jti,
            //    Exp = exp
            //});
            //if (!_deletedTokenRepository.Save())
            //{
            //    return StatusCode(500, "将token失效信息存入数据库时出错");
            //}
            return NoContent();
        }

        /// <summary>
        /// User Login.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] LoginDto userDto)
        {
            var login = Mapper.Map<Login>(userDto);
            if (_loginRepository.VerifyLogin(login))
            {
                var logininfo = _loginRepository.GetLoginInfo(userDto.UserName);
                _unitOfWork.ChangeDatabase(logininfo.Company);
                var userinfo = _userRepository.GetUserInfo(userDto.UserName);
                if (userinfo == null)
                {
                    return StatusCode(500, "用户信息缺失");
                }
                var tokeninfo = Mapper.Map<Login, UserInfoDto>(logininfo);
                Mapper.Map(userinfo, tokeninfo);
                try
                {
                    return new ObjectResult(TokenOperator.GenerateToken(tokeninfo));
                }
                catch(Exception e)
                {
                    return StatusCode(500, "生成令牌时出错: " + e.Message);
                }
            }
            return BadRequest();
        }

        [HttpPost("encrypt")]
        public IActionResult PostEncrypt([FromBody] EncryptedString encryptedString)
        {
            LoginDto loginDto;
            try
            {
                string origintext = Encryptor.DecryptString_Aes(encryptedString.ciphertext, Encryptor.key);
                loginDto = JsonConvert.DeserializeObject<LoginDto>(origintext);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            if (loginDto != null)
            {
                Login login = Mapper.Map<Login>(loginDto);
                if (_loginRepository.VerifyLogin(login))
                {
                    var logininfo = _loginRepository.GetLoginInfo(loginDto.UserName);
                    _unitOfWork.ChangeDatabase(logininfo.Company);
                    var userinfo = _userRepository.GetUserInfo(loginDto.UserName);
                    if (userinfo == null)
                    {
                        return StatusCode(500, "用户信息缺失");
                    }
                    var tokeninfo = Mapper.Map<Login, UserInfoDto>(logininfo);
                    Mapper.Map(userinfo, tokeninfo);
                    try
                    {
                        return new ObjectResult(TokenOperator.GenerateToken(tokeninfo));
                    }
                    catch (Exception e)
                    {
                        return StatusCode(500, "生成令牌时出错：" + e.Message);
                    }
                }
            }
            return BadRequest();
        }

    }
}
