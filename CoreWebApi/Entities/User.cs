using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
    }
}
