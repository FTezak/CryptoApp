using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoAPI.DTOs;
using CryptoAPI.Entities;
using CryptoAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptoAPI.Data
{
    public class CryptoDataRepository : ICryptoDataRepository
    {
        private readonly DataContext _context;

        public CryptoDataRepository(DataContext context)
        {
            _context = context;
        }

        public void Update(CryptoCurrencyData cryptoCurrencyData)
        {
            _context.Entry(cryptoCurrencyData).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> SaveAllAsyncInt()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CryptoDataDto>> GetAllCurrentCryptoDataAsync()
        {
            return new List<CryptoDataDto>();
        }

        public async Task<IEnumerable<CryptoCurrencyData>> GetCryptoDataAsyncBySymbol(string symbol)
        {
            CryptoCurrency crypto = await _context.CryptoCurrency
                .Include(r => r.CryptoCurrencyDatas)
                .Where(c => c.Symbol == symbol)
                .SingleOrDefaultAsync();
            
            List<CryptoCurrencyData> lista = new List<CryptoCurrencyData>();
            foreach (var item in crypto.CryptoCurrencyDatas)
            {
                lista.Add(new CryptoCurrencyData()
                {
                    CryptoCurrency = item.CryptoCurrency,
                    Date = item.Date.ToUniversalTime(),
                    Close = item.Close,
                    CryptoCurrencyId = item.CryptoCurrencyId,
                    Id = item.Id,
                    Open = item.Open
                });
            }

            return lista;
        }

        public Task<IEnumerable<CryptoDataDto>> GetDateCryptoDataBySymbolAsync(string symbol, DateTime? date1, DateTime date2)
        {
            throw new NotImplementedException();
        }

        public void AddToCryptoData(CryptoCurrencyData cryptoData)
        {
            _context.CryptoCurrencyData.Add(cryptoData);
        }

        public Task<bool> AnyCryptosAsync()
        {
            return _context.CryptoCurrencyData.AnyAsync();
        }

        public Task<decimal> GetAllTimeHighById(int id)
        {
            return _context.CryptoCurrencyData.Where(p=>p.CryptoCurrencyId == id).Select(p => p.Open).MaxAsync();
        }

        public Task<decimal> GetDayOldPrice(int id)
        {
            return _context.CryptoCurrencyData
                .Where(p => p.CryptoCurrencyId == id && p.Date < DateTime.Now.AddHours(-24))
                .OrderByDescending(x => x.Date)
                .Select(p => p.Open)
                .FirstAsync();
        }

        public Task<decimal> GetCurrentPriceById(int id)
        {
            return _context.CryptoCurrencyData
                .Where(p => p.CryptoCurrencyId == id)
                .OrderByDescending(x => x.Date)
                .Select(p => p.Open)
                .FirstAsync();
        }

        public Task<decimal> GetWeekOldPrice(int id)
        {
            return _context.CryptoCurrencyData
                .Where(p => p.CryptoCurrencyId == id && p.Date < DateTime.Now.AddDays(-7))
                .OrderByDescending(x => x.Date)
                .Select(p => p.Open)
                .FirstAsync();
        }

        public Task<decimal> GetMonthOldPrice(int id)
        {
            return _context.CryptoCurrencyData
                .Where(p => p.CryptoCurrencyId == id && p.Date < DateTime.Now.AddMonths(-1))
                .OrderByDescending(x => x.Date)
                .Select(p => p.Open)
                .FirstAsync();
        }
    }
}
