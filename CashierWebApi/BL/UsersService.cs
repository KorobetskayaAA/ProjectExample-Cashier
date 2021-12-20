using CashierDB.Model.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CashierWebApi.BL
{
    public partial class UsersService
    {
        private readonly UserManager<UserDbDto> userManager;

        public UsersService(UserManager<UserDbDto> userManager)
        {
            this.userManager = userManager;
        }

        async Task<Exception> ApplyToUser(string userName, Func<UserDbDto, Task<Exception>> method)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new KeyNotFoundException($"Пользователь {userName} не найден.");
            }
            return await method(user);
        }

        async Task<bool> UserExists(string userName)
        {
            return await userManager.FindByNameAsync(userName) != null;
        }
    }
}
