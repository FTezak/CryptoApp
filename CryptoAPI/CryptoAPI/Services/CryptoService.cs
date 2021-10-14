using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoAPI.Entities;
using CryptoAPI.Helpers;
using CryptoAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoAPI.Services
{
    public class CryptoService : ICryptoService

    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly ICryptoDataRepository _cryptoDataRepository;

        public CryptoService(IServiceProvider serviceProvider)
        {
            _cryptoRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICryptoRepository>();
            _cryptoDataRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICryptoDataRepository>();
        }

        public Task<decimal> getCurrentCryptoPriceFromDB(CryptoCurrency crypto)
        {
            return _cryptoDataRepository.GetCurrentPriceById(crypto.Id);
        }

        public async Task<PagedList<CryptoCurrency>> GetCryptosPagedAsync(CryptoParams cryptoParams)
        {
            return await _cryptoRepository.GetCryptosPagedAsync(cryptoParams);
        }

        public async Task<List<CryptoCurrency>> GetCryptoByNameLikeAsync(string nameLike)
        {
            return await _cryptoRepository.GetCryptoByNameLikeAsync(nameLike);
        }

        public async Task<List<CryptoCurrency>> GetAllCryptosAsync()
        {
            return await _cryptoRepository.GetAllCryptosAsync();
        }

        public async Task<CryptoCurrency> GetCryptoBySymbolAsync(string symbol)
        {
            return await _cryptoRepository.GetCryptoBySymbolAsync(symbol);
        }

        public Task<bool> SaveAllAsync()
        {
            return _cryptoRepository.SaveAllAsync();
        }
    }

}
