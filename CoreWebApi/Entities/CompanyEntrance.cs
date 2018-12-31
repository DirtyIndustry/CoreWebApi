using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Entities
{
    public class CompanyEntrance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserEntrance> Users { get; set; }
    }
}
