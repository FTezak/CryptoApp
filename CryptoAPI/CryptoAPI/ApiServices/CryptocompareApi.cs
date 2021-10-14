using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoAPI.ApiServices;
using CryptoAPI.DTOs;
using CryptoAPI.Entities;
using CryptoAPI.Helpers;
using CryptoAPI.Interfaces;
using CryptoAPI.Models.CryptoModels;
using Microsoft.Extensions.Configuration;

namespace CryptoAPI.Tasks
{
    public class CryptocompareApi

    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly ICryptoDataRepository _cryptoDataRepository;
        private readonly string _apiKey;
        private HttpAPIClient client = new HttpAPIClient();

        public CryptocompareApi(ICryptoRepository cryptoRepository, ICryptoDataRepository cryptoDataRepository, IConfiguration config)
        {
            _cryptoRepository = cryptoRepository;
            _cryptoDataRepository = cryptoDataRepository;
            _apiKey = config.GetValue<string>("CryptoCompareKey");
        }


        public async Task<bool> callPriceDayApi()
        {
            if (await _cryptoDataRepository.AnyCryptosAsync())
            {
                Console.WriteLine("U bazi već ima zapisa cijene kriptovaluta!");

                return false;
            }

            Console.WriteLine("U bazi nema zapisa cijena kriptovaluta pa upisujem nove!");


            int upisano = 0;

            IEnumerable<CryptoCurrency> cryptoCurrencies = await _cryptoRepository.GetAllCryptosAsync();
            
            foreach (var item in cryptoCurrencies)
            {

                string url = "https://min-api.cryptocompare.com/data/v2/histoday?fsym=" + item.Symbol + "&tsym=USD&limit=2000&api_key=" + _apiKey;

                var response = client.RunAsync<CryptoCompare>(url, null).GetAwaiter().GetResult();
                
                try
                {

                    if (response.cryptoCompareData.cryptoCompareDataData != null)
                    {
                        for (int i = response.cryptoCompareData.cryptoCompareDataData.Length - 1; i >= 0; i--)
                        {
                            CryptoCurrencyData cryptoCurrencyData = new CryptoCurrencyData()
                            {
                                CryptoCurrencyId = item.Id,
                                Date = DateTimeOffset.FromUnixTimeSeconds(response.cryptoCompareData.cryptoCompareDataData[i].time).UtcDateTime,
                                Open = response.cryptoCompareData.cryptoCompareDataData[i].open,
                                Close = response.cryptoCompareData.cryptoCompareDataData[i].close
                            };

                            _cryptoDataRepository.AddToCryptoData(cryptoCurrencyData);
                        }
                        upisano++;

                        Console.WriteLine(upisano + " -> " + item.Symbol);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

            int upisanoUBazu = await _cryptoRepository.SaveAllAsyncint();

            Console.WriteLine("Upisano u bazu: " + upisanoUBazu + " zapisa!");

            Console.WriteLine("----------------> UPISANO: " + upisano);

            return true;

        }

        public async Task<PagedList<CryptoDataDto>> getCryptosDtoHomeData(CryptoParams cryptoParams)
        {
            PercentageDifference percentageDifference = new PercentageDifference();

            IEnumerable<CryptoCurrency> listOfCryptos = await _cryptoRepository.GetCryptosPagedAsync(cryptoParams);

            List<CryptoCurrencyData> listOfCryptosData = await GetCryptosHomeData(listOfCryptos);

            List<CryptoDataDto> listOfCryptosDto = new List<CryptoDataDto>();

            foreach (var item in listOfCryptos)
            {
                listOfCryptosDto.Add(new CryptoDataDto()
                {
                    Symbol = item.Symbol,
                    Date = DateTime.Now,
                    Name = item.Name,
                    CryptoApiReference = item.CryptoApiReference,
                    Price = listOfCryptosData.Find(p => p.CryptoCurrencyId == item.Id).Open,
                    DayPercentage = percentageDifference.CalculatePercentageDifference(await _cryptoDataRepository.GetDayOldPrice(item.Id), listOfCryptosData.Find(p => p.CryptoCurrencyId == item.Id).Open),
                    WeekPercentage = percentageDifference.CalculatePercentageDifference(await _cryptoDataRepository.GetWeekOldPrice(item.Id), listOfCryptosData.Find(p => p.CryptoCurrencyId == item.Id).Open),
                    MonthPercentage = percentageDifference.CalculatePercentageDifference(await _cryptoDataRepository.GetMonthOldPrice(item.Id), listOfCryptosData.Find(p => p.CryptoCurrencyId == item.Id).Open),
                    Favorite = false
                });
            }


            return await PagedList<CryptoDataDto>.CreateFromListAsync(listOfCryptosDto, await _cryptoRepository.GetNumberOfCryptos() , cryptoParams.PageNumber,
                cryptoParams.PageSize);

        }

        public async Task<CryptoDataDto> getCryptosDtoHomeDataForCrypto(string cryptoSym)
        {
            PercentageDifference percentageDifference = new PercentageDifference();

            CryptoCurrency crypto = await _cryptoRepository.GetCryptoBySymbolAsync(cryptoSym);

           

            CryptoCurrencyData CryptoData = await GetCryptosHomeDataForCrypto(crypto);

            CryptoDataDto cryptoDataDto = new CryptoDataDto();

            if (CryptoData != null)
            {
                cryptoDataDto = new CryptoDataDto()
                {
                    Symbol = crypto.Symbol,
                    Date = DateTime.Now,
                    Name = crypto.Name,
                    CryptoApiReference = crypto.CryptoApiReference,
                    Price = CryptoData.Open,
                    DayPercentage = percentageDifference.CalculatePercentageDifference(
                        await _cryptoDataRepository.GetDayOldPrice(crypto.Id),
                        CryptoData.Open),
                    WeekPercentage = percentageDifference.CalculatePercentageDifference(
                        await _cryptoDataRepository.GetWeekOldPrice(crypto.Id),
                        CryptoData.Open),
                    MonthPercentage = percentageDifference.CalculatePercentageDifference(
                        await _cryptoDataRepository.GetMonthOldPrice(crypto.Id),
                        CryptoData.Open),
                    Favorite = false
                };

            }

            return cryptoDataDto;

        }



        public async Task<List<CryptoCurrencyData>> GetCryptosHomeData(IEnumerable<CryptoCurrency> cryptoCurrencies)
        {
            List<CryptoCurrencyData> listOfData = new List<CryptoCurrencyData>();

            List<string> urlParameters = new List<string>();

            foreach (var item in cryptoCurrencies)
            {
                urlParameters.Add("histominute?fsym=" + item.Symbol + "&tsym=USD&limit=1&api_key=" + _apiKey);
            }

            string url = "https://min-api.cryptocompare.com/data/v2/";

            var response = client.RunAsyncList<CryptoCompare>(url, urlParameters).GetAwaiter().GetResult();

            int brojac = 0;
            List<CryptoCurrency> listOfcryptoCurrencies = cryptoCurrencies.ToList();

            foreach (var item in response)
            {
                try
                {

                    if (item.cryptoCompareData.cryptoCompareDataData != null)
                    {

                        CryptoCurrencyData cryptoCurrencyData = new CryptoCurrencyData()
                        {
                            CryptoCurrencyId = listOfcryptoCurrencies.ElementAt(brojac).Id,
                            CryptoCurrency = listOfcryptoCurrencies.ElementAt(brojac),
                            Date = DateTimeOffset.FromUnixTimeSeconds(item.cryptoCompareData.cryptoCompareDataData[1].time).UtcDateTime,
                            Open = item.cryptoCompareData.cryptoCompareDataData[1].open,
                            Close = item.cryptoCompareData.cryptoCompareDataData[1].close
                        };

                        listOfData.Add(cryptoCurrencyData);

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
                brojac++;
            }
            
            return listOfData;
            
        }



        public async Task<CryptoCurrencyData> GetCryptosHomeDataForCrypto(CryptoCurrency cryptoCurrencie)
        {

            string urlParameter = "histominute?fsym=" + cryptoCurrencie.Symbol +
                                  "&tsym=USD&limit=1&api_key=" + _apiKey;

            string url = "https://min-api.cryptocompare.com/data/v2/";

            var response = client.RunAsync<CryptoCompare>(url, urlParameter).GetAwaiter().GetResult();
            
                try
                {

                    if (response.cryptoCompareData.cryptoCompareDataData != null)
                    {

                        CryptoCurrencyData cryptoCurrencyData = new CryptoCurrencyData()
                        {
                            CryptoCurrencyId = cryptoCurrencie.Id,
                            CryptoCurrency = cryptoCurrencie,
                            Date = DateTimeOffset.FromUnixTimeSeconds(response.cryptoCompareData.cryptoCompareDataData[1].time).UtcDateTime,
                            Open = response.cryptoCompareData.cryptoCompareDataData[1].open,
                            Close = response.cryptoCompareData.cryptoCompareDataData[1].close
                        };

                        return cryptoCurrencyData;
                    
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            return null;
            
        }
        
        public async Task<List<CryptoCurrencyData>> GetCryptosMinuteDataForDayBySymbol(string symbol)
        {
            List<CryptoCurrencyData> listOfData = new List<CryptoCurrencyData>();

            string urlParameter = "histominute?fsym=" + symbol + "&tsym=USD&limit=1440&api_key=" + _apiKey;

            string url = "https://min-api.cryptocompare.com/data/v2/";

            var response = client.RunAsync<CryptoCompare>(url, urlParameter).GetAwaiter().GetResult();

            int brojac = 0;

            foreach (var item in response.cryptoCompareData.cryptoCompareDataData)
            {
                try
                {

                    if (item != null)
                    {

                        CryptoCurrencyData cryptoCurrencyData = new CryptoCurrencyData()
                        {
                            Date = DateTimeOffset.FromUnixTimeSeconds(item.time).UtcDateTime,
                            Open = item.open,
                            Close = item.close
                        };

                        listOfData.Add(cryptoCurrencyData);

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                brojac++;
            }

            return listOfData;

        }


        public async Task<bool> callCurrentPriceApi()
        {
            
            Console.WriteLine("Preuzimam trenutne cijene kripto valuta!");

            int upisano = 0;

            IEnumerable<CryptoCurrency> cryptoCurrencies = await _cryptoRepository.GetAllCryptosAsync();

            string valute = "";

            foreach (var item in cryptoCurrencies)
            {
                
                    if (valute != "") valute = valute + ",";
                    valute = valute + item.Symbol;

                    if (valute.Length >= 280)
                    {
                        string urlParameter = "pricemulti?fsyms=" + valute + "&tsyms=USD&api_key=" + _apiKey;

                        string url = "https://min-api.cryptocompare.com/data/";

                        var response = client.RunAsync<Dictionary<string, cryptocomparePricemulti>>(url, urlParameter).GetAwaiter().GetResult();

                        try
                        {

                            if (response != null)
                            {
                                DateTime date = DateTime.Now;
                                foreach (var crypto in response)
                                {
                               
                                    CryptoCurrencyData cryptoCurrencyData = new CryptoCurrencyData()
                                    {
                                        CryptoCurrencyId = cryptoCurrencies.FirstOrDefault(x => x.Symbol.ToUpper() == crypto.Key).Id,
                                        Date = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0),
                                        Open = crypto.Value.price,
                                        Close = 0
                                    };

                                    _cryptoDataRepository.AddToCryptoData(cryptoCurrencyData);
                                }
                                upisano++;

                                Console.WriteLine(upisano + " -> " + upisano);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                        valute = "";
                    }

            }

            int upisanoUBazu = await _cryptoDataRepository.SaveAllAsyncInt();

            Console.WriteLine("Upisano u bazu: " + upisanoUBazu + " zapisa!");

            Console.WriteLine("----------------> UPISANO: " + upisano);

            return true;

        }

        

        public async Task<Dictionary<string, decimal>> callCurrentPriceApiForCryptosBySymbola(List<string> cryptoSyms)
        {
            Dictionary<string, decimal> cryproPrices = new Dictionary<string, decimal>();

            string valute = "";

            int numberOfCryptos = cryptoSyms.Count;

            foreach (var item in cryptoSyms)
            {
                
                    if (valute != "") valute = valute + ",";
                    valute = valute + item;
                    numberOfCryptos--;
                
                if(valute.Length >= 280 || numberOfCryptos <= 0)
                {

                    string urlParameter = "pricemulti?fsyms=" + valute + "&tsyms=USD&api_key=" + _apiKey;

                    string url = "https://min-api.cryptocompare.com/data/";

                    var response = client.RunAsync<Dictionary<string, cryptocomparePricemulti>>(url, urlParameter).GetAwaiter().GetResult();

                    try
                    {

                        if (response != null)
                        {
                            foreach (var cr in response)
                            {
                                cryproPrices.Add(cr.Key, cr.Value.price);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    valute = "";
                }

            }

            return cryproPrices;
        }
        
        public async Task<bool> SetAllTimeHigh()
        {

            Console.WriteLine("Idem upisivati All time high!");


            IEnumerable<CryptoCurrency> cryptoCurrencies = await _cryptoRepository.GetAllCryptosAsync();

            foreach (var item in cryptoCurrencies)
            {

                item.AllTimeHigh = await _cryptoDataRepository.GetAllTimeHighById(item.Id);
            }

            int brojZapisa = await _cryptoRepository.SaveAllAsyncint();

            return true;
        }

    }
}
