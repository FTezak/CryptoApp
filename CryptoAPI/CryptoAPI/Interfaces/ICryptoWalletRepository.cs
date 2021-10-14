using System.Threading.Tasks;

namespace CryptoAPI.Interfaces
{
    public interface ICryptoWalletRepository
    {
        Task<bool> SaveAllAsync();
        Task<int> SaveAllAsyncint();
        //Task<List<CryptoWallet>> GetUserWalletAsync(AppUser user);
        //void AddToCryptoWalletForUser(CryptoCurrency crypto, AppUser user, decimal amount);




    }
}
