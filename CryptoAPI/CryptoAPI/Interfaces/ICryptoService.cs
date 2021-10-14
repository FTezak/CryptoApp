using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoAPI.Entities;
using CryptoAPI.Helpers;

namespace CryptoAPI.Interfaces
{
    public interface ICryptoService
    {
        Task<Decimal> getCurrentCryptoPriceFromDB(CryptoCurrency crypto);
        Task<PagedList<CryptoCurrency>> GetCryptosPagedAsync(CryptoParams cryptoParams);
        Task<List<CryptoCurrency>> GetCryptoByNameLikeAsync(string nameLike);
        Task<List<CryptoCurrency>> GetAllCryptosAsync();
        Task<CryptoCurrency> GetCryptoBySymbolAsync(string symbol);
    }
}
