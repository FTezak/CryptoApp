using System.Threading.Tasks;
using CryptoAPI.DTOs;
using CryptoAPI.Entities;

namespace CryptoAPI.Interfaces
{
    public interface IWalletService
    {
        Task<WalletDto> getUserWallet(AppUser sourceUser);
    }
}
