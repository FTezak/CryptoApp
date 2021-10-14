using System.Threading.Tasks;
using CryptoAPI.Entities;

namespace CryptoAPI.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
