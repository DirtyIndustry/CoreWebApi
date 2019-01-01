using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class LoginModificationDto
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
    }
}
