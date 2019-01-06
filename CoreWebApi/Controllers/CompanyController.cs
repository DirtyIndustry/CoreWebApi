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
    public class CompanyController: ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly IUnitOfWork<EntranceContext> _unitOfWorkEntrance;
        private readonly IUnitOfWork<CompanyContext> _unitOfWorkCompany;

        public CompanyController(ILogger<CompanyController> logger,
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

        [HttpGet("exist/{company}")]
        public IActionResult GetCompany(string company)
        {
            var companyExist = _loginRepository.CompanyExists(company);
            return Ok(companyExist);
        }
    }
}
