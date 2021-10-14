using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoAPI.Entities;
using Microsoft.AspNetCore.Identity;

namespace CryptoAPI.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserExists(string username);
        Task<bool> EmailExists(string email);
        Task<bool> CreateAsync(AppUser user, string password);

        Task<AppUser> FindByNameAsync(string UserName);
        Task<AppUser> FindByEmailAsync(string email);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);
        Task<List<AppUser>> GetAllUsersWithWalletNewsletterFavorite();

    }
}
