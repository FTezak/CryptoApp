using Newtonsoft.Json;

namespace CryptoAPI.Models.CryptoModels
{
    public class CryptoCompare
    {
        

        [JsonProperty("Data")]
        public CryptoCompareData cryptoCompareData { get; set; }
    }

    
    public class CryptoCompareData
    {

        [JsonProperty("Data")]
        public CryptoCompareDataData[] cryptoCompareDataData { get; set; }
        
    }

    public class CryptoCompareDataData
    {
        [JsonProperty("open")]
        public decimal open { get; set; }

        [JsonProperty("close")]
        public decimal close { get; set; }

        [JsonProperty("time")]
        public long time { get; set; }

    }
}
