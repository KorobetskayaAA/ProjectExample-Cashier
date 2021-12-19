using Microsoft.AspNetCore.Identity;

namespace CashierDB.Model.DTO
{
    public class UserDbDto : IdentityUser
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
