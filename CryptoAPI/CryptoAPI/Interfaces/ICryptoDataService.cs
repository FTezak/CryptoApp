using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoAPI.Entities;

namespace CryptoAPI.Interfaces
{
    public interface ICryptoDataService
    {
        Task<IEnumerable<CryptoCurrencyData>> GetCryptoDataAsyncBySymbol(string symbol);
    }
}
