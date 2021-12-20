using CashierDB.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashierWebApi.BL.Model
{
    public class UserProfileApiDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public UserProfileApiDto() { }

        public UserProfileApiDto(UserDbDto user)
        {
            UserName = user.UserName;
            Email = user.Email;
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            LastName = user.LastName;
        }

        public UserProfileApiDto(UserDbDto user, IEnumerable<string> roles) : this(user)
        {
            Roles = roles;
        }

        public void Update(UserDbDto user)
        {
            user.Email = Email;
            user.FirstName = FirstName;
            user.MiddleName = MiddleName;
            user.LastName = LastName;
        }

        public UserDbDto Create()
        {
            return new UserDbDto()
            {
                UserName = UserName,
                Email = Email,
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
            };
        }
    }

    public class UserProfileCreateApiDto : UserProfileApiDto
    {
        public string Password { get; set; }
    }
}
