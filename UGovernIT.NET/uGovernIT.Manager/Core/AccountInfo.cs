
namespace uGovernIT.Manager
{
    public class AccountInfo
    {
        public string AccountID { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; } //added to set tenant name as defaukt user name 
    }
}