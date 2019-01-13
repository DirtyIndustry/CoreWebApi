using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Entities;
using CoreWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private const string Default_Department = "headquarter";
        private const string Default_Position = "Boss";

        private readonly ILogger<TokenController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly IUnitOfWork<EntranceContext> _unitOfWorkEntrance;
        private readonly IUnitOfWork<CompanyContext> _unitOfWorkCompany;

        public UserController(ILogger<TokenController> logger,
            IUserRepository userRepository,
            ILoginRepository loginRepository,
            IUnitOfWork<EntranceContext> unitOfWorkEntrance,
            IUnitOfWork<CompanyContext> unitOfWorkCompany)
        {
            _logger = logger;
            _userRepository = userRepository;
            _loginRepository = loginRepository;
            _unitOfWorkEntrance = unitOfWorkEntrance;
            _unitOfWorkCompany = unitOfWorkCompany;
        }

        [HttpGet("{username}", Name = "GetUser")]
        [Authorize(Policy = "Jti")]
        public IActionResult Get(string username)
        {
            var login = _loginRepository.GetLoginInfo(username);
            if (login == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWorkCompany.ChangeDatabase(login.Company);
                var user = _userRepository.GetUserInfo(username);
                var userinfo = Mapper.Map<UserInfoDto>(login);
                Mapper.Map(user, userinfo);
                return Ok(userinfo);
            }

        }

        [HttpGet("all")]
        [Authorize(Policy = "Jti")]
        public IActionResult GetAll()
        {
            var userlist = _loginRepository.GetLoginList();
            if (userlist == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(userlist);
            }
        }

        [HttpGet("exist/{username}")]
        public IActionResult GetUserExist(string username)
        {
            var loginexist = _loginRepository.LoginExists(username);

            return Ok(loginexist);
        }

        /// <summary>
        /// User Registration
        /// </summary>
        /// <param name="loginCreateDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] LoginCreateDto loginCreateDto)
        {
            if (loginCreateDto == null)
            {
                return BadRequest();
            }

            var login = Mapper.Map<Login>(loginCreateDto);
            var user = Mapper.Map<User>(loginCreateDto);

            //if (!_loginRepository.CompanyExists(login.Company))
            //{
            //    user.Department = Default_Department;
            //    user.Position = Default_Position;
            //}

            _loginRepository.AddLogin(login);
            _unitOfWorkCompany.ChangeDatabase(login.Company);
            _userRepository.AddUser(user);

            var userinfo = Mapper.Map<UserInfoDto>(loginCreateDto);

            if (_unitOfWorkEntrance.Save() & _unitOfWorkCompany.Save())
            {
                return CreatedAtRoute("GetUser", new { username = user.UserName }, userinfo);
            }
            else
            {
                return StatusCode(500, "将用户信息存入数据库时出错");
            }
        }

        /// <summary>
        /// Delete a User
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpDelete("{username}")]
        [Authorize(Policy = "Jti")]
        public IActionResult Delete(string username)
        {
            var login = _loginRepository.GetLoginInfo(username);
            if (login == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWorkCompany.ChangeDatabase(login.Company);
                var user = _userRepository.GetUserInfo(username);
                _loginRepository.RemoveLogin(login);
                _userRepository.RemoveUser(user);

                if (_unitOfWorkEntrance.Save() & _unitOfWorkCompany.Save())
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(500, "从数据库删除用户信息时出错");
                }
            }
        }

        /// <summary>
        /// Modify User's Infomations.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userModificationDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = "Jti")]
        public IActionResult Put([FromBody] UserModificationDto userModificationDto)
        {
            if (userModificationDto == null)
            {
                return BadRequest();
            }
            var login = _loginRepository.GetLoginInfo(userModificationDto.UserName);
            if (login == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWorkCompany.ChangeDatabase(login.Company);
                var user = _userRepository.GetUserInfo(userModificationDto.UserName);
                Mapper.Map(userModificationDto, user);
                if (_unitOfWorkCompany.Save())
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(500, "保存用户信息到数据库时出错");
                }
            }
        }

        /// <summary>
        /// Partially Modify User's Info.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("{username}")]
        [Authorize(Policy = "Jti")]
        public IActionResult Patch(string username, [FromBody] JsonPatchDocument<UserModificationDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            var login = _loginRepository.GetLoginInfo(username);
            if (login == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWorkCompany.ChangeDatabase(login.Company);
                var user = _userRepository.GetUserInfo(username);
                var toPatch = Mapper.Map<UserModificationDto>(user);
                patchDoc.ApplyTo(toPatch, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Mapper.Map(toPatch, user);
                if (!_unitOfWorkCompany.Save())
                {
                    return StatusCode(500, "将用户信息存入数据时出错");
                }
                return NoContent();
            }
        }

        /// <summary>
        /// Modify User's Password.
        /// </summary>
        /// <param name="loginModificationDto"></param>
        /// <returns></returns>
        [HttpPatch("password")]
        [Authorize(Policy = "Jti")]
        public IActionResult PatchPassword([FromBody] LoginModificationDto loginModificationDto)
        {
            if (loginModificationDto == null)
            {
                return BadRequest();
            }
            var login = _loginRepository.GetLoginInfo(loginModificationDto.UserName);
            if (login == null)
            {
                return NotFound();
            }
            else if (login.Password != loginModificationDto.OldPassword)
            {
                ModelState.AddModelError("Password", "用户密码错误");
                return BadRequest(ModelState);
            }
            else
            {
                login.Password = loginModificationDto.Password;
                if (_unitOfWorkEntrance.Save())
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(500, "将用户信息存入数据库时出错");
                }
            }
        }
    }
}
