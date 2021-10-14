using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoAPI.Entities;

namespace CryptoAPI.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<bool> DeleteUserById(int id);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);

    }
}
