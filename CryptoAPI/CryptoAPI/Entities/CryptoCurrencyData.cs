using System;
using Microsoft.EntityFrameworkCore;

namespace CryptoAPI.Entities
{
    [Index(nameof(Date), nameof(CryptoCurrencyId))]
    public class CryptoCurrencyData
    {
        public int Id { get; set; }
        public CryptoCurrency CryptoCurrency { get; set; }
        public int CryptoCurrencyId { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
    }
}
