using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Entities;
using CoreWebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreWebApi.Authorization
{
    public class JwtReissueMiddleware
    {
        private const string VALID = "valid";
        private const string EXPIRED = "expire";
        private const string INVALID = "invalid";
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtReissueMiddleware> _logger;

        public JwtReissueMiddleware(RequestDelegate next, ILogger<JwtReissueMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IUnitOfWork<CompanyContext> unitOfWork, ILoginRepository loginRepository, IUserRepository userRepository)
        {
            string auth = "Authorization";
            string logincookie = "Login";
            if (context.Request.Headers.ContainsKey(auth))
            {
                var str = context.Request.Headers[auth];
                var token = context.Request.Headers[auth].ToString().Replace("Bearer ", "");
                if (Authenticate(token) == EXPIRED)
                {
                    _logger.LogDebug("Token is Expired.");
                    string cookie = context.Request.Cookies[logincookie];
                    if (cookie != null)
                    {
                        LoginDto loginDto = JsonConvert.DeserializeObject<LoginDto>(cookie);
                        loginDto.UserName = System.Web.HttpUtility.UrlDecode(loginDto.UserName);
                        loginDto.Password = System.Web.HttpUtility.UrlDecode(loginDto.Password);
                        Login login = Mapper.Map<Login>(loginDto);
                        if (loginRepository.VerifyLogin(login))
                        {
                            Login logininfo = loginRepository.GetLoginInfo(login.UserName);
                            unitOfWork.ChangeDatabase(logininfo.Company);
                            User userinfo = userRepository.GetUserInfo(loginDto.UserName);
                            UserInfoDto tokeninfo = Mapper.Map<Login, UserInfoDto>(logininfo);
                            Mapper.Map(userinfo, tokeninfo);
                            string newtoken = TokenOperator.GenerateToken(tokeninfo);
                            context.Request.Headers[auth] = "Bearer " + newtoken;
                            //context.Response.Headers.Add("NewToken", newtoken);
                            context.Response.OnStarting(() =>
                            {
                                context.Response.Headers["NewToken"] = newtoken;
                                return Task.CompletedTask;
                            });
                            _logger.LogDebug("Token is Reissued for " + loginDto.UserName);
                        }
                    }
                }
            }

            await _next.Invoke(context);
        }

        private string Authenticate(string token)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the secret that needs to be at least 16 characeters long for HmacSha256"));
            // SigningCredentials credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            SecurityToken validatedToken;
            JwtSecurityTokenHandler validator = new JwtSecurityTokenHandler();

            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ClockSkew = TimeSpan.FromMinutes(0);
            validationParameters.ValidateIssuerSigningKey = true;
            validationParameters.IssuerSigningKey = key;
            validationParameters.ValidateIssuer = false;
            validationParameters.ValidIssuer = "Core Web Api";
            validationParameters.ValidateAudience = false;
            validationParameters.ValidateLifetime = true;

            if (validator.CanReadToken(token))
            {
                ClaimsPrincipal principal;
                try
                {
                    principal = validator.ValidateToken(token, validationParameters, out validatedToken);
                    return VALID;
                }
                catch (Exception e)
                {
                    if (e is SecurityTokenExpiredException)
                    {
                        return EXPIRED;
                    }
                    else if (e is SecurityTokenValidationException)
                    {
                        return INVALID;
                    }
                }
            }
            return INVALID;
        }
    }
}
