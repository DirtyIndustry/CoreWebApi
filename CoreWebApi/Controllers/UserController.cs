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
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork<EntranceContext> _unitOfWork;

        public UserController(ILogger<TokenController> logger,
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            IUnitOfWork<EntranceContext> unitOfWork)
        {
            _logger = logger;
            _companyRepository = companyRepository;
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

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var userlist = _userRepository.GetUserList();
            if (userlist == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(userlist);
            }
        }

        /// <summary>
        /// User Registration
        /// </summary>
        /// <param name="userCreateDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] UserCreateDto userCreateDto)
        {
            if (userCreateDto == null)
            {
                return BadRequest();
            }
            
            var user = Mapper.Map<UserEntrance>(userCreateDto);

            if (!_companyRepository.CompanyExists(userCreateDto.CompanyName))
            {
                _companyRepository.AddCompany(new CompanyEntrance
                {
                    CompanyName = userCreateDto.CompanyName
                });
            }

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
