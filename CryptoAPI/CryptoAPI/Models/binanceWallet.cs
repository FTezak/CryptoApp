using Newtonsoft.Json;

namespace CryptoAPI.Models.CryptoModels
{
    

        public class BinanceWallet
        {
            [JsonProperty("code")]
            public long Code { get; set; }

            [JsonProperty("msg")]
            public string Msg { get; set; }

            [JsonProperty("snapshotVos")]
            public SnapshotVo[] SnapshotVos { get; set; }
        }

        public class SnapshotVo
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("updateTime")]
            public long UpdateTime { get; set; }

            [JsonProperty("data")]
            public Data Data { get; set; }
        }

        public class Data
        {
            [JsonProperty("totalAssetOfBtc")]
            public string TotalAssetOfBtc { get; set; }

            [JsonProperty("balances")]
            public Balance[] Balances { get; set; }
        }

        public class Balance
        {
            [JsonProperty("asset")]
            public string Asset { get; set; }

            [JsonProperty("free")]
            public decimal Free { get; set; }

            [JsonProperty("locked")]
            public decimal Locked { get; set; }
        }
    

}
