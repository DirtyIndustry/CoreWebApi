using CoreWebApi.Entities;
using CoreWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Authorization
{
    public class ValidJtiHandler: AuthorizationHandler<ValidJtiRequirement>
    {
        // private readonly EntranceContext _context;
        private readonly IDeletedTokenRepository _deletedTokenRepository;

        public ValidJtiHandler(IDeletedTokenRepository deletedTokenRepository)
        {
            _deletedTokenRepository = deletedTokenRepository;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidJtiRequirement requirement)
        {
            // 检查jti是否存在
            var jti = context.User.FindFirst("jti")?.Value;
            if (jti == null)
            {
                context.Fail(); // 显式声明验证失败
                return Task.CompletedTask;
            }

            if (_deletedTokenRepository.VerifyToken(jti))
            {
                context.Succeed(requirement); // 显式声明验证成功
            }
            else
            {
                context.Fail(); // jti在失效列表中 验证失败
            }
            return Task.CompletedTask;
        }
    }
}
