using System;
using System.Threading.Tasks;
using CryptoAPI.ApiServices;
using CryptoAPI.Entities;
using CryptoAPI.Interfaces;
using CryptoAPI.Models.CryptoModels;
using Microsoft.Extensions.Configuration;

namespace CryptoAPI.Tasks
{
    public class CoinmarketcapApi
        
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly string _apiKey;
        private HttpAPIClient client = new HttpAPIClient();

        public CoinmarketcapApi(ICryptoRepository cryptoRepository, IConfiguration config)
        {
            _cryptoRepository = cryptoRepository;
            _apiKey = config.GetValue<string>("CoinMarketCapKey");
        }

        public async Task<bool> callMapApi()
        {
            if (await _cryptoRepository.AnyCryptosAsync())
            {
                Console.WriteLine("U bazi već ima zapisa kriptovaluta!");

                return false;
            }

            Console.WriteLine("U bazi nema zapisa kriptovaluta pa upisujem nove!");

            const string url = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/map";
            

            string urlParameters = $"?sort=cmc_rank&CMC_PRO_API_KEY={_apiKey}";
            var response = client.RunAsync<coinmarketcap>(url, urlParameters).GetAwaiter().GetResult();

            if (response != null)
            {
                for (int i = 0; i < 500; i++)
                {
                    Console.WriteLine(response.coinmarketcapData[i].Name + " -> " + response.coinmarketcapData[i].Symbol);
                    _cryptoRepository.AddToCrypto(new CryptoCurrency()
                        {
                            FirstDate = response.coinmarketcapData[i].FirstDate, 
                            Name = response.coinmarketcapData[i].Name, 
                            Symbol = response.coinmarketcapData[i].Symbol,
                            CryptoApiReference = response.coinmarketcapData[i].id,
                            Rank = response.coinmarketcapData[i].Rank


                    });
                }

                await _cryptoRepository.SaveAllAsync();
            }

            return true;
        }
        
    }
}
