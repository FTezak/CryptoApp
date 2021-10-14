using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoAPI.Entities;
using CryptoAPI.Helpers;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CryptoAPI.Interfaces
{
    public interface ICryptoRepository
    {
        void Update(CryptoCurrency cryptoCurrency);
        Task<bool> SaveAllAsync();
        Task<bool> DeleteCryptoBySymbol(string symbol);
        Task<PagedList<CryptoCurrency>> GetCryptosPagedAsync(CryptoParams cryptoParams);

        Task<List<CryptoCurrency>> GetAllCryptosAsync();

        Task<CryptoCurrency> GetCryptoBySymbolAsync(string symbol);

        Task<List<CryptoCurrency>> GetCryptoByNameLikeAsync(string nameLike);
        Task<CryptoCurrency> GetCryptoByIdAsync(int id);
        Task<bool> AnyCryptosAsync();
        void AddToCrypto(CryptoCurrency crypto);

        Task<int> SaveAllAsyncint();

        Task<int> GetNumberOfCryptos();

        Task<EntityEntry<CryptoCurrency>> DeleteCrypto(CryptoCurrency cryptoCurrency);

    }
}
