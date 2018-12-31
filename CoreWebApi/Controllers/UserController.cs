using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Entities;
using CoreWebApi.Repositories;
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
    public class UserController: ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        //private readonly EntranceContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(ILogger<TokenController> logger, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{username}", Name = "GetUser")]
        public IActionResult Get(string username)
        {
            var user = _userRepository.GetUserInfo(username);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(user);
            }

        }

        /// <summary>
        /// User Registration
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }
            var user = Mapper.Map<User>(userDto);
            _userRepository.AddUser(user);
            if (_unitOfWork.Save())
            {
                return CreatedAtRoute("GetUser", new { username = user.UserName }, user);
            }
            else
            {
                return StatusCode(500, "将用户信息存入数据库时出错");
            }
        }
    }
}
