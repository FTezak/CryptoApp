using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoAPI.Entities;
using CryptoAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoAPI.Services
{
    public class CryptoDataService : ICryptoDataService

    {
        private readonly ICryptoDataRepository _cryptoDataRepository;

        public CryptoDataService(IServiceProvider serviceProvider)
        {
            _cryptoDataRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICryptoDataRepository>();
        }


        public Task<IEnumerable<CryptoCurrencyData>> GetCryptoDataAsyncBySymbol(string symbol)
        {
            return _cryptoDataRepository.GetCryptoDataAsyncBySymbol(symbol);
        }
    }

}
