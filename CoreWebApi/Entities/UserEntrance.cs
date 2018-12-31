namespace CoreWebApi.Entities
{
    public class UserEntrance
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int CompanyId { get; set; }
        public CompanyEntrance Company { get; set; }
        public User User { get; set; }
    }
}
