using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Objects;
using Binance.Net.Objects.Spot.SpotData;
using CryptoAPI.DTOs;
using CryptoAPI.Entities;
using CryptoAPI.Interfaces;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;

namespace CryptoAPI.Tasks
{
    public class BinanceApi

    {
        private readonly ICryptoRepository _cryptoRepository;
        

        public BinanceApi(ICryptoRepository cryptoRepository)
        {
            _cryptoRepository = cryptoRepository;
  
        }

        public async Task<List<CryptosWallet>> GetBinaceWallet(string key, string sercretKey)
        {
            List<CryptosWallet> binanceLista = new List<CryptosWallet>();

            var client = new BinanceClient(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(key, sercretKey)
            });

            WebCallResult<IEnumerable<BinanceSpotAccountSnapshot>> result = await client.General.GetDailySpotAccountSnapshotAsync(DateTime.Now.AddHours(-30));

            List<BinanceSpotAccountSnapshot> podaci = new List<BinanceSpotAccountSnapshot>();

            if (result.Success == true)
            {
                 podaci = result.Data.ToList();
            }

            foreach (var item in podaci)
            {
                List<BinanceBalance> bb = item.Data.Balances.ToList();

                foreach (var item2 in bb)
                {
                    if (item2.Total > 0)
                    {
                        CryptoCurrency crypto = await _cryptoRepository.GetCryptoBySymbolAsync(item2.Asset);
                        if (crypto != null)
                        {
                            CryptosWallet cw = new CryptosWallet()
                            {
                                Symbol = crypto.Symbol,
                                Name = crypto.Name,
                                CryptoApiReference = crypto.CryptoApiReference,
                                binanceAmount = item2.Total
                            };
                            binanceLista.Add(cw);
                        }
                    }
                    
                }
            }
            return binanceLista;
        }
    }
}
