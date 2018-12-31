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

        public UserController(ILogger<TokenController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult Post(UserDto user)
        {
            bool success = true;
            if (success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
