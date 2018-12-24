using System;

namespace CoreWebApi.Entities
{
    public class DeletedToken
    {
        public int Id { get; set; }
        public string Jti { get; set; }
        public DateTime Exp { get; set; }
    }
}
