using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoAPI.Entities;
using CryptoAPI.Helpers;
using CryptoAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CryptoAPI.Data
{
    public class CryptoRepository : ICryptoRepository
    {
        private readonly DataContext _context;



        public CryptoRepository(DataContext context)
        {
            _context = context;
        }
        

        public void Update(CryptoCurrency cryptoCurrency)
        {
            _context.Entry(cryptoCurrency).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> SaveAllAsyncint()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> GetNumberOfCryptos()
        {
            return _context.CryptoCurrency.Count();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> DeleteCryptoBySymbol(string symbol)
        {
            _context.CryptoCurrency.Remove(new CryptoCurrency() {Symbol = symbol});
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<EntityEntry<CryptoCurrency>> DeleteCrypto(CryptoCurrency cryptoCurrency)
        {
            
            return  _context.CryptoCurrency.Remove(cryptoCurrency);
        }


        public async Task<PagedList<CryptoCurrency>> GetCryptosPagedAsync(CryptoParams cryptoParams)
        {
            var query = _context.CryptoCurrency.OrderBy(p => p.Rank).AsNoTracking();

            if (cryptoParams == null)
            {
                
                return await PagedList<CryptoCurrency>.CreateAsync(query, 1, await _context.CryptoCurrency.CountAsync());
            }
            
            return await PagedList<CryptoCurrency>.CreateAsync(query, cryptoParams.PageNumber, cryptoParams.PageSize);
        }


        public async Task<List<CryptoCurrency>> GetAllCryptosAsync()
        {
            return await _context.CryptoCurrency.ToListAsync();
        }



        //public async Task<IEnumerable<CryptoDataDto>> GetCryptosDtoHomeAsync()
        //{
        //    List<CryptoCurrency> listOfCryptos = await _context.CryptoCurrency.ToListAsync();

        //    List<CryptoCurrencyData> listOfCryptosData = await _coinmarketcapApi.getCryptoHomeData();

        //    List<CryptoDataDto> listOfCryptosDto = new List<CryptoDataDto>();

        //    foreach (var item in listOfCryptos)
        //    {
        //        listOfCryptosDto.Add(new CryptoDataDto()
        //        {
        //            Symbol = item.Symbol,
        //            Date = DateTime.Now,
        //            Name = item.Name,
        //            CryptoApiReference = item.CryptoApiReference,
        //            Price = listOfCryptosData.Find(p => p.CryptoCurrencyId == item.Id).Open
        //        });
        //    }


        //    return listOfCryptosDto;
        //}

        public async Task<CryptoCurrency> GetCryptoBySymbolAsync(string symbol)
        {
            return await _context.CryptoCurrency.AsNoTracking().SingleOrDefaultAsync(c => c.Symbol == symbol);
        }

        public async Task<List<CryptoCurrency>> GetCryptoByNameLikeAsync(string nameLike)
        {
            if (nameLike.Length > 0)
            {
                return _context.CryptoCurrency.Where(c => c.Name.StartsWith(nameLike)).ToList();
            }

            return await _context.CryptoCurrency.ToListAsync();
        }

        public async Task<CryptoCurrency> GetCryptoByIdAsync(int id)
        {
            return await _context.CryptoCurrency.SingleOrDefaultAsync(c => c.Id == id);
        }

        public Task<bool> AnyCryptosAsync()
        {
            return _context.CryptoCurrency.AnyAsync();
        }

        public async void AddToCrypto(CryptoCurrency crypto)
        {
            _context.CryptoCurrency.Add(crypto);
        }

    }
}
