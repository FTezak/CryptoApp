using System;
using System.Threading.Tasks;
using CryptoAPI.Tasks;

namespace CryptoAPI.Data
{
    public class Seed
    {
        
        public static async Task SeedCrypto(CryptocompareApi cryptocompareApi)
        {
            Console.WriteLine("Ušao sam u seed za kripto!!!");
            //await coinmarketcapApi.callMapApi();
            await cryptocompareApi.callPriceDayApi();
            //await coinmarketcapApi.SetAllTimeHigh();

        }
    }
}
