using CoreWebApi.Dtos;
using CoreWebApi.Entities;
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
        private readonly EntranceContext _context;

        public UserController(ILogger<TokenController> logger, EntranceContext context)
        {
            _logger = logger;
            _context = context;
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
