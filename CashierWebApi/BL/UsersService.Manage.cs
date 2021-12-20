using CashierDB.Model.DTO;
using CashierWebApi.BL.Exceptions;
using CashierWebApi.BL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashierWebApi.BL
{
    public partial class UsersService
    {
        public async Task<IEnumerable<UserProfileApiDto>> GetProfiles()
        {
            var users = await userManager.Users.ToListAsync();
            return users
                .Select(user => new UserProfileApiDto(user, userManager.GetRolesAsync(user).Result));
        }

        public async Task<UserProfileApiDto> GetProfile(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return null;
            }
            var profile = new UserProfileApiDto(user);
            profile.Roles = await userManager.GetRolesAsync(user);
            return profile;
        }

        public async Task<Exception> UpdateProfile(UserDbDto user, UserProfileApiDto profile)
        {
            if (user == null)
            {
                return new KeyNotFoundException("Пользователь не найден.");
            }
            profile.Update(user);
            var result = await userManager.UpdateAsync(user);
            return result.Succeeded ? null : new SaveChangesException();
        }

        public async Task<Exception> UpdateProfile(UserProfileApiDto profile)
        {
            return await ApplyToUser(profile.UserName, (user) => UpdateProfile(user, profile));
        }

        public async Task<Exception> ResetPassword(UserDbDto user, string newPassword)
        {
            if (user == null)
            {
                return new KeyNotFoundException("Пользователь не найден.");
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded ? null : new SaveChangesException();
        }

        public async Task<Exception> ResetPassword(string userName, string newPassword)
        {
            return await ApplyToUser(userName, (user) => ResetPassword(user, newPassword));
        }

        public async Task<Exception> Create(UserProfileCreateApiDto profile)
        {
            var user = profile.Create();
            var result = await userManager.CreateAsync(user, profile.Password);
            if (!result.Succeeded)
            {
                if (await UserExists(profile.UserName)) {
                    return new AlreadyExistsException($"Пользователь с именем {profile.UserName} уже существует.");
                }
                return new Exception("Не удалось создать пользователя с указанными параметрами.");
            }
            result = await userManager.AddToRolesAsync(user, profile.Roles);
            if (!result.Succeeded)
            {
                return new Exception("Не удалось назначить пользователю одну или несколько из указанных ролей.");
            }
            return null;
        }

        public async Task<Exception> Delete(UserDbDto user)
        {
            if (user == null)
            {
                return new KeyNotFoundException("Пользователь не найден.");
            }
            var result = await userManager.DeleteAsync(user);
            return result.Succeeded ? null : new SaveChangesException();
        }

        public async Task<Exception> Delete(string userName)
        {
            return await ApplyToUser(userName, Delete);
        }
    }
}
