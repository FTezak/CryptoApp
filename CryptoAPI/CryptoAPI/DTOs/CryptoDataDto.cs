using System;

namespace CryptoAPI.DTOs
{
    public class CryptoDataDto
    {
        public int CryptoApiReference { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public decimal DayPercentage { get; set; }
        public decimal WeekPercentage { get; set; }
        public decimal MonthPercentage { get; set; }
        public bool Favorite { get; set; }
    }
}
