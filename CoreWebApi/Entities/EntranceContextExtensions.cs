using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Entities
{
    public static class EntranceContextExtensions
    {
        public static void EnsureSeedDataForContext(this EntranceContext context)
        {
            if (context.Users.Any())
            {
                return;
            }
            var users = new List<User>
            {
                new User
                {
                    UserName = "admin",
                    Password = "admin"
                },
                new User
                {
                    UserName = "张三",
                    Password = "123"
                },
                new User
                {
                    UserName = "李四",
                    Password = "123"
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
