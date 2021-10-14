using System;
using System.Collections.Generic;

namespace CryptoAPI.Entities
{
    public class CryptoCurrency
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public int CryptoApiReference { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public DateTime FirstDate { get; set; }
        public ICollection<CryptoCurrencyData> CryptoCurrencyDatas { get; set; }
        public ICollection<AppUser> FavoriteOfUser { get; set; }
        public decimal AllTimeHigh { get; set; }
    }
}
