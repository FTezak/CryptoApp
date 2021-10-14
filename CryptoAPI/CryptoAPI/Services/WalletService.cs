using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoAPI.DTOs;
using CryptoAPI.Entities;
using CryptoAPI.Interfaces;
using CryptoAPI.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoAPI.Services
{
    public class WalletService : IWalletService

    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly ICryptoDataRepository _cryptoDataRepository;
        private readonly IConfiguration _config;
        private readonly CryptocompareApi _cca;
        private readonly BinanceApi _ba;

        public WalletService(IServiceProvider serviceProvider, IConfiguration config)
        {
            _cryptoRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICryptoRepository>();
            _cryptoDataRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICryptoDataRepository>();
            _config = config;
            _cca = new CryptocompareApi(_cryptoRepository, _cryptoDataRepository, config);
            _ba = new BinanceApi(_cryptoRepository);
        }
        
        public async Task<WalletDto> getUserWallet(AppUser sourceUser)
        {

            WalletDto userWallet = new WalletDto();
            userWallet.HasBinance = false;
            userWallet.Cryptos = new List<CryptosWallet>();

            List<string> cryptosInWallet = new List<string>();

            
            
            List<CryptoCurrency> cryptos = await _cryptoRepository.GetAllCryptosAsync();



            var wallet = sourceUser.CryptoWallet;

            Crypting crypting = new Crypting(_config);

            List<CryptosWallet> binanceWallet = new List<CryptosWallet>();

            if (sourceUser.BinanceApiKey != null && sourceUser.BinanceSecretApiKey != null)
            {
                binanceWallet = await _ba.GetBinaceWallet(crypting.DecryptString(sourceUser.BinanceApiKey), crypting.DecryptString(sourceUser.BinanceSecretApiKey));

                if (binanceWallet.Any())
                {
                    userWallet.HasBinance = true;
                }

            }



            List<CryptosWallet> elementsToRemoveFromBinanceWallet = new List<CryptosWallet>();


            foreach (var item in cryptos)
            {
                CryptoWallet walletCrypto = wallet.FirstOrDefault(c => c.CryptoId == item.Id);
                if (walletCrypto != null)
                {


                    CryptosWallet cw = new CryptosWallet()
                    {
                        Symbol = item.Symbol,
                        Name = item.Name,
                        CryptoApiReference = item.CryptoApiReference,
                        amount = walletCrypto.Amount,
                        binanceAmount = 0
                    };

                    foreach (var binanceCrypto in binanceWallet)
                    {
                        if (binanceCrypto.Symbol == item.Symbol)
                        {
                            cw.binanceAmount = binanceCrypto.binanceAmount;
                            elementsToRemoveFromBinanceWallet.Add(binanceCrypto);
                        }
                    }

                    userWallet.Cryptos.Add(cw);
                    cryptosInWallet.Add(cw.Symbol);
                }


                binanceWallet = binanceWallet.Except(elementsToRemoveFromBinanceWallet).ToList();

                foreach (var binanceCrypto in binanceWallet)
                {
                    if (binanceCrypto.Symbol == item.Symbol)
                    {
                        CryptosWallet cw = new CryptosWallet()
                        {
                            Symbol = item.Symbol,
                            Name = item.Name,
                            CryptoApiReference = item.CryptoApiReference,
                            binanceAmount = binanceCrypto.binanceAmount,
                            amount = 0
                        };
                        userWallet.Cryptos.Add(cw);
                        cryptosInWallet.Add(cw.Symbol);
                    }
                }

            }

            CoinmarketcapApi cmca = new CoinmarketcapApi(_cryptoRepository, _config);

            Dictionary<string, decimal> cryproPrices = await _cca.callCurrentPriceApiForCryptosBySymbola(cryptosInWallet);

            foreach (var item in userWallet.Cryptos)
            {
                item.Price = cryproPrices[item.Symbol];
            }

            decimal totalValue = 0;

            foreach (var item in userWallet.Cryptos)
            {
                totalValue += (item.amount + item.binanceAmount) * item.Price;
            }

            userWallet.totalValue = totalValue;

            return userWallet;
        }
    }

}
