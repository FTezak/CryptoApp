using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoAPI.DTOs;
using CryptoAPI.Entities;

namespace CryptoAPI.Interfaces
{
    public interface ICryptoDataRepository
    {
        void Update(CryptoCurrencyData cryptoCurrencyData);
        Task<bool> SaveAllAsync();
        Task<int> SaveAllAsyncInt();
        Task<IEnumerable<CryptoDataDto>> GetAllCurrentCryptoDataAsync();
        Task<IEnumerable<CryptoCurrencyData>> GetCryptoDataAsyncBySymbol(string symbol);
        Task<IEnumerable<CryptoDataDto>> GetDateCryptoDataBySymbolAsync(string symbol, DateTime? date1, DateTime date2);
        void AddToCryptoData(CryptoCurrencyData cryptoData);
        Task<bool> AnyCryptosAsync();
        Task<decimal> GetAllTimeHighById(int id);
        Task<decimal> GetDayOldPrice(int id);
        Task<decimal> GetCurrentPriceById(int id);
        Task<decimal> GetWeekOldPrice(int id);
        Task<decimal> GetMonthOldPrice(int id);
    }
}
