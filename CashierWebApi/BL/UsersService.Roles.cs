using CashierDB.Model.DTO;
using System;
using System.Threading.Tasks;

namespace CashierWebApi.BL
{
    public partial class UsersService
    {
        public async Task<Exception> AssignRole(UserDbDto user, string role)
        {
            var result = await userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                return new Exception($"Не удалось назначить пользователю {user.UserName} роль {role}.");
            }
            return null;
        }

        public async Task<Exception> AssignRole(string userName, string role)
        {
            return await ApplyToUser(userName, user => AssignRole(user, role));
        }

        public async Task<Exception> RemoveFromRole(UserDbDto user, string role)
        {
            var result = await userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
            {
                return new Exception($"Не удалось удалить у пользователя {user.UserName} роль {role}.");
            }
            return null;
        }

        public async Task<Exception> RemoveFromRole(string userName, string role)
        {
            return await ApplyToUser(userName, user => RemoveFromRole(user, role));
        }
    }
}
