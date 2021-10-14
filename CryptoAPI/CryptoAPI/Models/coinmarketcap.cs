using System;
using Newtonsoft.Json;

namespace CryptoAPI.Models.CryptoModels
{
    public class coinmarketcap
    {
        [JsonProperty("data")]
        public CoinmarketcapData[] coinmarketcapData { get; set; }
    }

    
    public class CoinmarketcapData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("first_historical_data")]
        public DateTime FirstDate { get; set; }
    }
}
