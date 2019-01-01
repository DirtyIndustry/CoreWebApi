using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class UserModificationDto
    {
        public string UserName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
    }
}
