using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoAPI.Entities;
using CryptoAPI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoAPI.Services
{
    public class UserService : IUserService

    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        }


        public async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> CreateAsync(AppUser user, string password)
        {
            if (_userManager.CreateAsync(user, password).Result.Succeeded)
            {
                if (_userManager.AddToRolesAsync(user, new[] {"User"}).Result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<AppUser> FindByNameAsync(string UserName)
        {
            return await _userManager.FindByNameAsync(UserName);
        }

        public async Task<AppUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<List<AppUser>> GetAllUsersWithWalletNewsletterFavorite()
        {
            List<AppUser> users = await _userManager.Users
                .Include(r => r.CryptoWallet)
                .Include(r => r.Newsletter)
                .Include(r => r.FavoriteCrypto)
                .ToListAsync();

            return users;
        }
    }

}
