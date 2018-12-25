using CoreWebApi.Entities;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Authorization
{
    public class ValidJtiHandler: AuthorizationHandler<ValidJtiRequirement>
    {
        private readonly EntranceContext _context;

        public ValidJtiHandler(EntranceContext context)
        {
            _context = context;
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

            // 检查jti是否在失效列表中
            bool tokenExists = false; // 从数据库中查找jti是否存在
            if(_context.DeletedTokens.Any(o => o.Jti == jti))
            {
                tokenExists = true;
            }

            if (tokenExists)
            {
                context.Fail(); // jti在失效列表中 验证失败
            }
            else
            {
                context.Succeed(requirement); // 显式声明验证成功
            }
            return Task.CompletedTask;
        }
    }
}
